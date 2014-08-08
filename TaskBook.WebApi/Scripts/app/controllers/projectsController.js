"use strict";
app.controller("projectsController", ["$scope", "$routeParams", "projectsService", function ($scope, $routeParams, projectsService) {

    $scope.project = {};
    $scope.project.ProjectId = $routeParams.projectId;
    $scope.message = "";
    $scope.successful = true;

    projectsService.getProjectByProjectId($scope.project.ProjectId).then(function (result) {
        $scope.successful = true;
        $scope.project = result.data;
    }, function (error) {
        $scope.successful = false;
        $scope.message = error.data.Message;
    });

    $scope.send = function () {
        var project = $scope.project;
        projectsService.putProject(project)
        .then(function (result) {
            $scope.successful = true;
            $scope.message = "Project details have been updated.";
            $scope.projectForm.$setPristine();
        }, function (error) {
            $scope.successful = false;
            $scope.message = error.data.Message;
        });
    }

}]);