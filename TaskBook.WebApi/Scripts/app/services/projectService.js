"use strict";
app.factory("projectService", ["$http", function ($http) {

    var projectServiceFactory = {};

    projectServiceFactory.getProjectsAndManagers = function () {
        return $http.get("api/Projects/GetProjectsAndManagers")
    };

    projectServiceFactory.getProjectsAndManagersByProjectId = function (projectId) {
        return $http.get("api/Projects/GetProjectsAndManagers/" + projectId)
    };

    projectServiceFactory.getProjectById = function (projectId) {
        return $http.get("api/Projects/GetProjectById/" + projectId)
    };
    
    projectServiceFactory.putProject = function (project) {
        return $http.put("api/projects/UpdateProject/" + project.ProjectId, project)
    };

    projectServiceFactory.postProject = function (project) {
        return $http.post("api/projects/AddProject", project)
    };

    projectServiceFactory.deleteProject = function (project) {
        return $http.delete("api/projects/DeleteProject/" + project.ProjectId)
    };

    return projectServiceFactory;
}]);