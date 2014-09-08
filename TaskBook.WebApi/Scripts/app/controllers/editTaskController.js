"use strict";
app.controller("editTaskController", ["$scope", "$routeParams", "$modal", "$location", "$timeout", "taskService", "accountService", "authService", "tbUtil",
    function ($scope, $routeParams, $modal, $location, $timeout, taskService, accountService, authService, tbUtil) {

        $scope.task = {};
        $scope.task.id = $routeParams.id;
        $scope.usersForProject = {};
        $scope.message = "";
        $scope.successful = true;
        $scope.statuses = ["New", "In Progress", "Completed"];

        var authName = authService.authData.userName;

        taskService.getTaskById($scope.task.id)
            .then(function (result) {
                $scope.successful = true;
                $scope.task = result.data;

                // List of users for the project
                accountService.getUsersByProjectId($scope.task.ProjectId)
                    .then(function (result) {
                        $scope.successful = true;
                        var users = result.data;
                        $scope.usersForProject = tbUtil.getColumnFromCollection(users, function (x) { return x.UserName }).sort();
                    }, function (error) {
                        $scope.successful = false;
                        $scope.message = error.data.Message;
                    });

            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });

        $scope.statusChanged = function(status) {
            if (status === "New" || status === "In Progress") {
                $scope.task.CompletedDate = null;
            }
            else { // Completed
                $scope.task.CompletedDate = new Date().toDateString();
            }
        };

        $scope.openDueDate = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.openedDueDate = !$scope.openedDueDate;
        };

        $scope.save = function () {
            var task = $scope.task;
            taskService.putTask(task)
            .then(function (result) {
                $scope.successful = true;
                $scope.message = "Task details have been updated.";
                $scope.editTaskForm.$setPristine();
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });
        };

        $scope.openModalDeleteTaskDialog = function (size) {

            var modalInstance = $modal.open({
                templateUrl: "/Scripts/app/views/modalDeleteTask.html",
                controller: "modalDeleteTaskController",
                size: size
            });

            modalInstance.result.then(function (result) {
                if (result == "ok") {
                    deleteTask();
                }
            }, function () {
                // Do nothing
            });
        };

        $scope.IsDeletionAllowed = function () {
            var result = false;
            if (authService.authData.role == "User") {
                // User cannot delete task
                // Actually Admin cannot delete task too, but this page is unavailable for Admin
                result = false;
            }
            else {
                // Manager and Advanced User can delete task
                result = true;
            }
            return result;
        };

        $scope.IsUpdateAllowed = function () {
            var result = true;
            // Manager and Advanced User can edit any task
            // User can edit anly own tasks, but other tasks are unavailable for User
            return result;
        };

        var deleteTask = function () {
            var task = $scope.task;
            taskService.deleteTask(task)
            .then(function (result) {
                $scope.message = "Task has been deleted successfully. You will be redirected to the task list in 3 seconds.";
                startTimer();
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });
        };

        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $location.path("/tasks/" + authName);
            }, 3000);
        };

}]);