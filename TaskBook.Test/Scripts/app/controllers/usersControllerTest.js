/// <reference path="../_references.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/tbutil.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/projectservice.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/taskservice.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/accountservice.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/controllers/userscontroller.js" />
"use strict";
describe("$usersController", function () {
    var $scope,
        $controller,
        $routeParams,
        $q,
        accountService,
        taskService,
        projectService,
        tbUtil;

    var authUser = {
        data: {
            UserName: "User1",
            Role: "Manager",
            ProjectTitle: "Project1",
            ProjectId: 1
        }
    };

    var users = {
        data: [{ UserName: "User1", Role: "Manager" }, { UserName: "User2", Role: "User" }]
    };

    var project = {
        data: {UserName: "Manager1", Title: "Project2", ManagerName: "Manager1", ProjectTitle: "Project2"}
    };

    var errMessage = "Error message";
    var error = {
        data: { Message: errMessage }
    };

    beforeEach(function () {
        module("TaskBookApp");
    });

    beforeEach(inject(function (_$controller_, $rootScope, _$routeParams_, _$q_, _accountService_, _taskService_, _projectService_, _tbUtil_) {
        $scope = $rootScope.$new();
        $controller = _$controller_;
        $routeParams = _$routeParams_;
        $q = _$q_;
        accountService = _accountService_;
        taskService = _taskService_;
        projectService = _projectService_;
        tbUtil = _tbUtil_;

        $routeParams.authName = "user1";
    }));

    it("should return users for Manager role", function () {

        // Auth user is a manager
        authUser.data.Role = "Manager";

        // Promises should be created before controller is instantiated 
        var authDeferred = $q.defer();
        authDeferred.resolve(authUser);
        spyOn(accountService, "getUserDetailsByUserName").and.returnValue(authDeferred.promise);

        var usersDeferred = $q.defer();
        usersDeferred.resolve(users);
        spyOn(accountService, "getUsersWithRolesByProjectId").and.returnValue(usersDeferred.promise);

        // Create the controller instance
        $controller("usersController",
              { $scope: $scope, $routeParams: $routeParams, accountService: accountService, taskService: taskService, projectService: projectService, tbUtil: tbUtil });

        // Resolve promises
        $scope.$apply();

        expect($scope.successful).toBeTruthy();
        expect($scope.message).toBe("");
        expect($scope.manager.userName).toBe(authUser.data.UserName);
        expect($scope.manager.projectTitle).toBe(authUser.data.ProjectTitle);
        expect($scope.users).toEqual(users.data);
    });

    it("should return users for non-Manager role", function () {

        // Auth user is not a manager
        authUser.data.Role = "NonManager";

        var authDeferred = $q.defer();
        authDeferred.resolve(authUser);
        spyOn(accountService, "getUserDetailsByUserName").and.returnValue(authDeferred.promise);

        var usersDeferred = $q.defer();
        usersDeferred.resolve(users);
        spyOn(accountService, "getUsersWithRolesByProjectId").and.returnValue(usersDeferred.promise);

        var projectDeferred = $q.defer();
        projectDeferred.resolve(project);
        spyOn(projectService, "getProjectsAndManagersByProjectId").and.returnValue(projectDeferred.promise);

        // Create the controller instance
        $controller("usersController",
              { $scope: $scope, $routeParams: $routeParams, accountService: accountService, taskService: taskService, projectService: projectService, tbUtil: tbUtil });

        // Resolve promises
        $scope.$apply();

        expect($scope.successful).toBeTruthy();
        expect($scope.message).toBe("");
        expect($scope.manager.userName).toBe(project.data.ManagerName);
        expect($scope.manager.projectTitle).toBe(project.data.ProjectTitle);
        expect($scope.users).toEqual(users.data);
    });

    it("should throw error for Manager role", function () {

        // Auth user is a manager
        authUser.data.Role = "Manager";

        // Make the error message specific for this test
        error.data.Message += "1";

        // Promises should be created before controller is instantiated 
        var authDeferred = $q.defer();
        authDeferred.reject(error);
        spyOn(accountService, "getUserDetailsByUserName").and.returnValue(authDeferred.promise);

        // Create the controller instance
        $controller("usersController",
              { $scope: $scope, $routeParams: $routeParams, accountService: accountService, taskService: taskService, projectService: projectService, tbUtil: tbUtil });

        // Resolve promises
        $scope.$apply();

        expect($scope.successful).toBeFalsy();
        expect($scope.message).toEqual(error.data.Message);
    });

    it("should throw error for non-Manager role", function () {

        // Auth user is not a manager
        authUser.data.Role = "NonManager";

        // Make the error message specific for this test
        error.data.Message += "2";

        var authDeferred = $q.defer();
        authDeferred.resolve(authUser);
        spyOn(accountService, "getUserDetailsByUserName").and.returnValue(authDeferred.promise);

        var projectDeferred = $q.defer();
        projectDeferred.reject(error);
        spyOn(projectService, "getProjectsAndManagersByProjectId").and.returnValue(projectDeferred.promise);

        // Create the controller instance
        $controller("usersController",
              { $scope: $scope, $routeParams: $routeParams, accountService: accountService, taskService: taskService, projectService: projectService, tbUtil: tbUtil });

        // Resolve promises
        $scope.$apply();

        expect($scope.successful).toBeFalsy();
        expect($scope.message).toEqual(error.data.Message);
    });

    it("should throw error while users are getting", function () {

        // Auth user is a manager. IMPORTANT for this test!!!
        authUser.data.Role = "Manager";
        
        // Make the error message specific for this test
        error.data.Message += "3";

        // Promises should be created before controller is instantiated 
        var authDeferred = $q.defer();
        authDeferred.resolve(authUser);
        spyOn(accountService, "getUserDetailsByUserName").and.returnValue(authDeferred.promise);

        var usersDeferred = $q.defer();
        usersDeferred.reject(error);
        spyOn(accountService, "getUsersWithRolesByProjectId").and.returnValue(usersDeferred.promise);

        // Create the controller instance
        $controller("usersController",
              { $scope: $scope, $routeParams: $routeParams, accountService: accountService, taskService: taskService, projectService: projectService, tbUtil: tbUtil });

        // Resolve promises
        $scope.$apply();

        expect($scope.successful).toBeFalsy();
        expect($scope.message).toEqual(error.data.Message);

        // These should be completed before the error
        expect($scope.manager.userName).toBe(authUser.data.UserName);
        expect($scope.manager.projectTitle).toBe(authUser.data.ProjectTitle);
    });

    it("should clear search filters", function () {
        // Create the controller instance
        $controller("usersController",
              { $scope: $scope, $routeParams: $routeParams, accountService: accountService, taskService: taskService, projectService: projectService, tbUtil: tbUtil });

        $scope.clearSearch();

        expect($scope.search["$"]).toBe("");
        expect($scope.search[$scope.fields[0].value]).toBe("");
        expect($scope.search[$scope.fields[1].value]).toBe("");
        expect($scope.search[$scope.fields[2].value]).toBe("");
        expect($scope.search[$scope.fields[3].value]).toBe("");
        expect($scope.search[$scope.fields[4].value]).toBe("");
    });

    it("should proper change sort column and order", function () {
        // Create the controller instance
        $controller("usersController",
              { $scope: $scope, $routeParams: $routeParams, accountService: accountService, taskService: taskService, projectService: projectService, tbUtil: tbUtil });

        // Should reverse sort order
        $scope.sort.column = $scope.fields[0].value;
        $scope.sort.reverse = false;
        $scope.toggleSort(0);
        expect($scope.sort.reverse).toBeTruthy();
        expect($scope.sort.column).toBe($scope.fields[0].value);

        // Should change a sort column
        $scope.sort.column = $scope.fields[0].value;
        $scope.sort.reverse = false;
        $scope.toggleSort(1);
        expect($scope.sort.reverse).toBeFalsy();
        expect($scope.sort.column).toBe($scope.fields[1].value);

    });

    it("should proper select a sort icon", function () {
        var returnedIconClass;
        var iconClass = "pull-right glyphicon glyphicon-sort-by-attributes";
        var iconClassAlt = iconClass + "-alt";

        // Create the controller instance
        $controller("usersController",
              { $scope: $scope, $routeParams: $routeParams, accountService: accountService, taskService: taskService, projectService: projectService, tbUtil: tbUtil });

        // Should select ascending icon (the same column, reverse is false )
        $scope.sort.column = $scope.fields[0].value;
        $scope.sort.reverse = false;
        returnedIconClass = $scope.selectIconClass(0);
        expect(returnedIconClass).toBe(iconClass);

        // Should select descending icon (the same column, reverse is true )
        $scope.sort.column = $scope.fields[0].value;
        $scope.sort.reverse = true;
        returnedIconClass = $scope.selectIconClass(0);
        expect(returnedIconClass).toBe(iconClassAlt);

        // Should select no icon (the column is changed, reverse is false )
        $scope.sort.column = $scope.fields[1].value;
        returnedIconClass = $scope.selectIconClass(0);
        expect(returnedIconClass).toBe("");
    });

});