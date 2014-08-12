"use strict";
app.controller("addManagerController", ["$scope", "$routeParams", "permissionsService", "projectsService", "accountService",
    function ($scope, $routeParams, permissionsService, projectsService, accountService) {

    $scope.user = {};
    $scope.user.ProjectId = $routeParams.projectId;
    $scope.user.Role = "Project manager";
    $scope.successful = true;
    $scope.message = "";

    projectsService.getProjectById($scope.user.ProjectId)
        .then(function (result) {
            $scope.successful = true;
            $scope.user.ProjectTitle = result.data.Title;
        }, function (error) {
            $scope.successful = false;
            $scope.message = error.data.Message;
        });

    permissionsService.getPermissionsByRole($scope.user.Role)
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
            $scope.addManagerForm.$setPristine();
        }, function (error) {
            $scope.successful = false;
            $scope.message = error.data.Message;
        });
    };

}]);