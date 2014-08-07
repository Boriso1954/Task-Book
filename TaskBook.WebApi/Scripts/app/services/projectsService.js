"use strict";
app.factory("projectsService", ["$http", function ($http) {

    var projectsServiceFactory = {};

    projectsServiceFactory.getProjectsAndManagers = function () {
        return $http.get("api/Projects/GetProjectsAndManagers")
    };

    projectsServiceFactory.getProjectByProjectId = function (projectId) {
        return $http.get("api/Projects/GetProjectsAndManagers/" + projectId)
    };

    return projectsServiceFactory;
}]);