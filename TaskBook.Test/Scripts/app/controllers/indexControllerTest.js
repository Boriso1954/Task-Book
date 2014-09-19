/// <reference path="../_references.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/roleservice.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/controllers/indexcontroller.js" />
"use strict";
describe("$indexController", function () {

    var roleService,
        authService,
        $scope,
        $location,
        $q,
        $controller;

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

        var deferred = $q.defer();
        deferred.resolve("Admin");
        spyOn(roleService, "getRoleByUserName").and.returnValue(deferred.promise);

        $controller("indexController",
              { $scope: $scope, $location: $location, authService: authService, roleService: roleService });

        // Resolve promises
        $scope.$apply();

        expect($scope.successful).toBeTruthy();
        expect($scope.message).toBe("");
        expect($location.path).toHaveBeenCalledWith("/projects");
    });

    it("should resolve any role ", function () {
        authService.authData.isAuth = true;
        authService.authData.userName = "user1";

        var deferred = $q.defer();
        deferred.resolve("AnyRole");

        spyOn(roleService, "getRoleByUserName").and.returnValue(deferred.promise);

        $controller("indexController",
              { $scope: $scope, $location: $location, authService: authService, roleService: roleService });

        // Resolve promises
        $scope.$apply();

        expect($scope.successful).toBeTruthy();
        expect($scope.message).toBe("");
        expect($location.path).toHaveBeenCalledWith("/tasks/" + authService.authData.userName);
    });

    it("should throw an error in roleService.getRoleByUserName", function () {
        authService.authData.isAuth = true;

        var errMessage = "Error in roleService";
        var error = {
            data: { Message: errMessage }
        };

        var deferred = $q.defer();
        deferred.reject(error);

        spyOn(roleService, "getRoleByUserName").and.returnValue(deferred.promise);

        $controller("indexController",
              { $scope: $scope, $location: $location, authService: authService, roleService: roleService });

        // Resolve promises
        $scope.$apply();
        
        expect($scope.successful).toBeFalsy();
        expect($location.path).not.toHaveBeenCalled();
        expect($scope.message).toEqual(errMessage);
    });
   
    it("should select a proper page if authentication is wrong", function () {
        authService.authData.isAuth = false;

        $controller("indexController",
              { $scope: $scope, $location: $location, authService: authService, roleService: roleService });

        expect($scope.successful).toBeTruthy();
        expect($scope.message).toBe("");
        expect($location.path.calls.count()).toBe(2);
    });

});