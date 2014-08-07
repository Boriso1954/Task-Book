"use strict";
app.controller("projectsController", ["$scope", "$routeParams", "projectsService", function ($scope, $routeParams, projectsService) {

    $scope.project = {};
    $scope.project.projectId = $routeParams.projectId;
    $scope.message = "";
    $scope.successful = false;

    projectsService.getProjectByProjectId($scope.project.projectId).then(function (result) {
        $scope.successful = true;
        $scope.project = result.data;
    }, function (error) {
        $scope.successful = false;
        $scope.message = error.data.Message;
    });

}]);