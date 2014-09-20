/// <reference path="../_references.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/roleservice.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/controllers/logincontroller.js" />
"use strict";
describe("$loginController", function () {

    var roleService,
        authService,
        $scope,
        $location,
        $q,
        $controller;

    var mockAuthService = {
        $get: function (rel) { }
    };

    beforeEach(function () {
        module("TaskBookApp");
    });

    beforeEach(inject(function (_$controller_, $rootScope, _$q_, _$location_, _authService_, _roleService_) {
        $scope = $rootScope.$new();
        $location = _$location_;
        $controller = _$controller_;
        $q = _$q_;
        authService = _authService_;
        roleService = _roleService_;

        spyOn($location, "path");

    }));

    it("should resolve Admin role ", function () {
        authService.authData.isAuth = true;
        authService.authData.userName = "user1";

        var authDeferred = $q.defer();
        authDeferred.resolve();
        spyOn(authService, "login").and.returnValue(authDeferred.promise);

        var deferred = $q.defer();
        deferred.resolve("Admin");
        spyOn(roleService, "getRoleByUserName").and.returnValue(deferred.promise);

        $controller("loginController",
              { $scope: $scope, $location: $location, authService: authService, roleService: roleService });
        
        // Run tested function
        $scope.login();

        // Resolve promises
        $scope.$apply();

        expect($scope.successful).toBeTruthy();
        expect($scope.message).toBe("");
        expect($location.path).toHaveBeenCalledWith("/projects");
    });

    it("should resolve non-Manager role", function () {
        authService.authData.isAuth = true;
        authService.authData.userName = "user1";

        var authDeferred = $q.defer();
        authDeferred.resolve();
        spyOn(authService, "login").and.returnValue(authDeferred.promise);

        var deferred = $q.defer();
        deferred.resolve("NonManager");
        spyOn(roleService, "getRoleByUserName").and.returnValue(deferred.promise);

        $controller("loginController",
              { $scope: $scope, $location: $location, authService: authService, roleService: roleService });

        // Run tested function
        $scope.loginData.userName = "user1";
        $scope.login();

        // Resolve promises
        $scope.$apply();

        expect($scope.successful).toBeTruthy();
        expect($scope.message).toBe("");
        expect($location.path).toHaveBeenCalledWith("/tasks/" + $scope.loginData.userName);
    });

    it("should throw an error in roleService.getRoleByUserName", function () {
        authService.authData.isAuth = true;

        var authDeferred = $q.defer();
        authDeferred.resolve();
        spyOn(authService, "login").and.returnValue(authDeferred.promise);

        var errMessage = "Error in roleService";
        var error = {
            data: { Message: errMessage }
        };

        var deferred = $q.defer();
        deferred.reject(error);
        spyOn(roleService, "getRoleByUserName").and.returnValue(deferred.promise);

        $controller("loginController",
              { $scope: $scope, $location: $location, authService: authService, roleService: roleService });

        // Run tested function
        $scope.loginData.userName = "user1";
        $scope.login();

        // Resolve promises
        $scope.$apply();

        expect($scope.successful).toBeFalsy();
        expect($location.path).not.toHaveBeenCalled();
        expect($scope.message).toEqual(errMessage);
    });

    it("should throw an error in authService.login", function () {
        authService.authData.isAuth = true;

        var errMessage = "Error in authService";
        var error = { error_description: errMessage };

        var authDeferred = $q.defer();
        authDeferred.reject(error);
        spyOn(authService, "login").and.returnValue(authDeferred.promise);

        $controller("loginController",
              { $scope: $scope, $location: $location, authService: authService, roleService: roleService });

        // Run tested function
        $scope.login();

        // Resolve promises
        $scope.$apply();

        expect($scope.successful).toBeFalsy();
        expect($location.path).not.toHaveBeenCalled();
        expect($scope.message).toEqual(errMessage);
    });
    
});