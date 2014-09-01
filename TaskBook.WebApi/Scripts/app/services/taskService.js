"use strict";
app.factory("taskService", ["$http",
    function ($http) {

        var taskServiceFactory = {};

        //taskServiceFactory.getTasks = function () {
        //    return $http.get("api/Tasks/GetTasks")
        //};

        taskServiceFactory.getTasksByProjectId = function (projectId) {
            return $http.get("api/Tasks/GetTasks/" + projectId)
        };

        taskServiceFactory.getTasksByUserName = function (userName) {
            return $http.get("api/Tasks/GetTasks/" + userName)
        };

        taskServiceFactory.getTaskById = function (id) {
            return $http.get("api/Tasks/GetTask/" + id)
        };

        taskServiceFactory.putTask = function (task) {
            return $http.put("api/tasks/UpdateTask/" + task.TaskId, task)
        };

        taskServiceFactory.postTask = function (task) {
            return $http.post("api/tasks/AddTask", task)
        };

        taskServiceFactory.deleteTask = function (task) {
            return $http.delete("api/tasks/DeleteTask/" + task.TaskId)
        };

        return taskServiceFactory;
}]);