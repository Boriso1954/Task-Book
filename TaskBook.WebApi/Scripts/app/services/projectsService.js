"use strict";
app.factory("projectsService", ["$http", function ($http) {

    var projectsServiceFactory = {};

    projectsServiceFactory.getProjectsAndManagers = function () {
        return $http.get("api/Projects/GetProjectsAndManagers")
    };

    projectsServiceFactory.getProjectByProjectId = function (projectId) {
        return $http.get("api/Projects/GetProjectsAndManagers/" + projectId)
    };

    projectsServiceFactory.putProject = function (project) {
        return $http.put("api/projects/UpdateProject/" + project.ProjectId, project)
    };

    projectsServiceFactory.postProject = function (project) {
        return $http.put("api/projects/AddProject/" + project)
    };

    projectsServiceFactory.deleteProject = function (project) {
        return $http.delete("api/projects/DeleteProject/" + project.ProjectId)
    };

    return projectsServiceFactory;
}]);