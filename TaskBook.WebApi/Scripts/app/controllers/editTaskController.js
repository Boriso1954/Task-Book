"use strict";
app.controller("editTaskController", ["$scope", "$routeParams", "$modal", "$location", "$timeout", "tasksService", "accountService", "tbUtil", "authService",
    function ($scope, $routeParams, $modal, $location, $timeout, tasksService, accountService, tbUtil, authService) {

        $scope.task = {};
        $scope.task.id = $routeParams.id;
        $scope.usersForProject = {};
        $scope.message = "";
        $scope.successful = true;
        $scope.statuses = ["New", "In Progress", "Completed"];

        var managerName = authService.authData.userName;

        tasksService.getTaskById($scope.task.id)
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
            else { // In Progress
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
            tasksService.putTask(task)
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

        var deleteTask = function () {
            var task = $scope.task;
            tasksService.deleteTask(task)
            .then(function (result) {
                $scope.message = "task has been deleted successfully. You will be redirected to the task list in 3 seconds.";
                startTimer();
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });
        };

        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $location.path("/tasks/" + managerName);
            }, 3000);
        };

}]);