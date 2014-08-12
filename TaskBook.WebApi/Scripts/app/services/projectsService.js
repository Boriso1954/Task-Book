"use strict";
app.factory("projectsService", ["$http", function ($http) {

    var projectsServiceFactory = {};

    projectsServiceFactory.getProjectsAndManagers = function () {
        return $http.get("api/Projects/GetProjectsAndManagers")
    };

    projectsServiceFactory.getProjectsAndManagersByProjectId = function (projectId) {
        return $http.get("api/Projects/GetProjectsAndManagers/" + projectId)
    };

    projectsServiceFactory.getProjectById = function (projectId) {
        return $http.get("api/Projects/GetProjectById/" + projectId)
    };
    
    projectsServiceFactory.putProject = function (project) {
        return $http.put("api/projects/UpdateProject/" + project.ProjectId, project)
    };

    projectsServiceFactory.postProject = function (project) {
        return $http.post("api/projects/AddProject", project)
    };

    projectsServiceFactory.deleteProject = function (project) {
        return $http.delete("api/projects/DeleteProject/" + project.ProjectId)
    };

    return projectsServiceFactory;
}]);