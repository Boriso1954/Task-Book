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

        tasksServiceFactory.getTaskById = function (id) {
            return $http.get("api/Tasks/GetTask/" + id)
        };

        tasksServiceFactory.putTask = function (task) {
            return $http.put("api/tasks/UpdateTask/" + task.TaskId, task)
        };

        tasksServiceFactory.postTask = function (task) {
            return $http.post("api/tasks/AddTask", task)
        };

        tasksServiceFactory.deleteTask = function (task) {
            return $http.delete("api/tasks/DeleteTask/" + task.TaskId)
        };

        return tasksServiceFactory;
}]);