"use strict";
app.factory("tasksService", ["$http",
    function ($http) {

    var tasksServiceFactory = {};

    tasksServiceFactory.getTasks = function () {
        return $http.get("api/Tasks/GetTasks")
    };

    return tasksServiceFactory;
}]);