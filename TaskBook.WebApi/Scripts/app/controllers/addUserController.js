"use strict";
app.controller("addUserController", ["$scope", "$routeParams", "permissionService", "projectService", "accountService",
    function ($scope, $routeParams, permissionService, projectService, accountService) {

        $scope.user = {};
        $scope.roles = [];

        $scope.successful = true;
        $scope.message = "";

        if ($routeParams.projectId !== "no") {
            // Add Manager
            $scope.user.ProjectId = $routeParams.projectId;
            projectService.getProjectById($scope.user.ProjectId)
            .then(function (result) {
                $scope.successful = true;
                $scope.user.ProjectTitle = result.data.Title;
                $scope.roles = ["Manager"];
                $scope.user.Role = $scope.roles[0];
                getPermissionsByRole($scope.user.Role);
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });
        }
        else {
            // Add Advanced or User
            var authName = $routeParams.authName;
            accountService.getUserDetailsByUserName(authName)
                .then(function (result) {
                    $scope.successful = true;
                    $scope.user.ProjectId = result.data.ProjectId;
                    $scope.user.ProjectTitle = result.data.ProjectTitle;
                    $scope.roles = ["Advanced", "User"];
                    $scope.user.Role = $scope.roles[0];
                    getPermissionsByRole($scope.user.Role);
                }, function (error) {
                    $scope.successful = false;
                    $scope.message = error.data.Message;
                });
        };

        $scope.getPermissions = function () {
            getPermissionsByRole($scope.user.Role);
        };

        $scope.add = function () {
            var user = $scope.user;
            accountService.postUserDetails(user)
            .then(function (result) {
                $scope.successful = true;
                $scope.message = "Manager details have been added.";
                $scope.addUserForm.$setPristine();
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });
        };

        var getPermissionsByRole = function (role) {
            permissionService.getPermissionsByRole(role)
            .then(function (result) {
                $scope.successful = true;
                $scope.user.Permissions = result.data;
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });
        };

}]);