"use strict";
app.controller("addUserController", ["$scope", "$routeParams", "permissionService", "projectService", "accountService",
    function ($scope, $routeParams, permissionService, projectService, accountService) {

    $scope.user = {};
    $scope.user.ProjectId = $routeParams.projectId;
    $scope.user.Role = "Manager"; //TODO: correct this
    $scope.successful = true;
    $scope.message = "";

    projectService.getProjectById($scope.user.ProjectId)
        .then(function (result) {
            $scope.successful = true;
            $scope.user.ProjectTitle = result.data.Title;
        }, function (error) {
            $scope.successful = false;
            $scope.message = error.data.Message;
        });

    permissionService.getPermissionsByRole($scope.user.Role)
        .then(function (result) {
            $scope.successful = true;
            $scope.user.Permissions = result.data;
        }, function (error) {
            $scope.successful = false;
            $scope.message = error.data.Message;
        });

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

}]);