"use strict";
app.factory("tasksService", ["$http",
    function ($http) {

    var tasksServiceFactory = {};

    tasksServiceFactory.getTasks = function () {
        return $http.get("api/Tasks/GetTasks")
    };

    tasksServiceFactory.getTasksByProjectId = function (projectId) {
        return $http.get("api/Tasks/GetTasks/" + projectId)
    };

    return tasksServiceFactory;
}]);