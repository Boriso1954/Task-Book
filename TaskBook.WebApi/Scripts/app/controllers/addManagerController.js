"use strict";
app.controller("addManagerController", ["$scope", "$routeParams", "userDetailsService", "projectsService",
    function ($scope, $routeParams, userDetailsService, projectsService) {

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

    userDetailsService.getPermissionsByRole($scope.user.Role)
          .then(function (result) {
              $scope.successful = true;
              $scope.user.Permissions = result.data;
          }, function (error) {
              $scope.successful = false;
              $scope.message = error.data.Message;
          });

    $scope.add = function () {
        var user = $scope.user;
        userDetailsService.postUserDetails(user)
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