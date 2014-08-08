"use strict";
app.controller("projectsController", ["$scope", "$routeParams", "$modal", "$location", "$timeout", "projectsService",
    function ($scope, $routeParams, $modal, $location, $timeout, projectsService) {

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
    };

    $scope.open = function (size) {

        var modalInstance = $modal.open({
            templateUrl: "/Scripts/app/views/modalDeleteProject.html",
            controller: "modalDeleteProjectController",
            size: size
        });

        modalInstance.result.then(function (result) {
            if (result == "ok") {
                deleteProject();
            }
        }, function () {
            // Do nothing
        });
    };
    
    var deleteProject = function () {
        var project = $scope.project;
        projectsService.deleteProject(project)
        .then(function (result) {
            $scope.message = "Project has been deleted successfully. You will be redicted to the project list in 3 seconds.";
            startTimer();
        }, function (error) {
            $scope.successful = false;
            $scope.message = error.data.Message;
        });
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path("/projectsAndManagers");
        }, 3000);
    };

}]);