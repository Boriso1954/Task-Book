﻿"use strict";
app.controller("addProjectController", ["$scope", "projectsService", function ($scope, projectsService) {

        $scope.project = {};
        $scope.message = "";
        $scope.successful = true;

        $scope.addProject = function () {
            var newProject = {}; 
            newProject.Title = $scope.project.ProjectTitle;
            projectsService.postProject(newProject)
            .then(function (result) {
                $scope.successful = true;
                $scope.message = "New project has been added.";
                $scope.addProjectForm.$setPristine();
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });
        };
}]);