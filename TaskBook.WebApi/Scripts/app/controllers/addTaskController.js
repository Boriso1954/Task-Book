"use strict";
app.controller("addTaskController", ["$scope", "$routeParams", "taskService", "accountService", "tbUtil",
    function ($scope, $routeParams, taskService, accountService, tbUtil) {

        $scope.task = {};
        $scope.usersForProject = {};

        $scope.message = "";
        $scope.successful = true;

        var authName = $routeParams.authName;
        var authUser = {};

        // Authentication user's data
        accountService.getUserDetailsByUserName(authName)
            .then(function (result) {
                $scope.successful = true;
                authUser = result.data;

                // List of users for the project
                accountService.getUsersByProjectId(authUser.ProjectId)
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

        $scope.addTask = function () {
            var newTask = {};
            newTask.Title = $scope.task.Title;
            newTask.Description = $scope.task.Description;
            newTask.ProjectId = authUser.ProjectId;
            newTask.CreatedDate = new Date();
            newTask.DueDate = $scope.task.DueDate;
            newTask.CreatedBy = authUser.UserName;
            newTask.AssignedTo = $scope.task.AssignedTo;
            newTask.Status = $scope.task.Status;

            taskService.postTask(newTask)
            .then(function (result) {
                $scope.successful = true;
                $scope.message = "New task has been added.";
                $scope.addTaskForm.$setPristine();
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });
        };

        $scope.openDueDate = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.openedDueDate = !$scope.openedDueDate;
        };
}]);