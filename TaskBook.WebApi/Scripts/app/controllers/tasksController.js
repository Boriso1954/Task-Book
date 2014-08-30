"use strict";
app.controller("tasksController", ["$scope", "$routeParams", "accountService", "taskService", "projectService", "tbUtil",
    function ($scope, $routeParams, accountService, taskService, projectService, tbUtil) {
    
        $scope.fields = [{
            name: "Title",
            value: "Title"
        }, {
            name: "Description",
            value: "Description"
        }, {
            name: "Created Date",
            value: "CreatedDate"
        }, {
            name: "Due Date",
            value: "DueDate"
        }, {
            name: "Status",
            value: "Status"
        }, {
            name: "Created By",
            value: "CreatedBy"
        }, {
            name: "Assigned To",
            value: "AssignedTo"
        }, {
            name: "Completed Date",
            value: "CompletedDate"
        }];

        $scope.authUser = {};
        $scope.manager = {};
        $scope.tasks = {};

        $scope.authUser.UserName = $routeParams.authName;
        $scope.successful = true;
        $scope.message = "";

        // Pagination variables
        $scope.totalItems = 0;
        $scope.currentPage = 1;
        $scope.pageSize = 5;

        // Manager's data
        var fullTaskList = null;
        var projectId = null;
        accountService.getUserDetailsByUserName($scope.authUser.UserName)
            .then(function (result) {
                $scope.successful = true;
                $scope.authUser = result.data;
                if ($scope.authUser.Role === "Manager") {
                    $scope.manager.userName = $scope.authUser.UserName;
                    $scope.manager.projectTitle = $scope.authUser.ProjectTitle;
                    projectId = $scope.authUser.ProjectId;
                    getTasksByProjectId(projectId);
                }
                else {
                    projectService.getProjectsAndManagersByProjectId($scope.authUser.ProjectId)
                    .then(function (result) {
                        $scope.successful = true;
                        $scope.manager.userName = result.data.UserName;
                        $scope.manager.projectTitle = result.data.Title;
                        if ($scope.authUser.Role === "Advanced") {
                            projectId = result.data.ProjectId;
                            getTasksByProjectId(projectId);
                        }
                        else { // $scope.authUser.Role = "User"
                            var userName = $scope.authUser.UserName;
                            getTasksByUserName(userName);
                        }
                    }, function (error) {
                        $scope.successful = false;
                        $scope.message = error.data.Message;
                    });
                }
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });

        var getTasksByProjectId = function (projectId) {
            taskService.getTasksByProjectId(projectId)
                    .then(function (result) {
                        $scope.successful = true;
                        fullTaskList = result.data;
                        $scope.totalItems = fullTaskList.length;
                        $scope.tasks = tbUtil.getItemsForPage(fullTaskList, $scope.currentPage, $scope.pageSize);
                        prepareData();
                    }, function (error) {
                        $scope.successful = false;
                        $scope.message = error.data.Message;
                    });
        };

        var getTasksByUserName = function (userName) {
            taskService.getTasksByUserName(userName)
                    .then(function (result) {
                        $scope.successful = true;
                        fullTaskList = result.data;
                        $scope.totalItems = fullTaskList.length;
                        $scope.tasks = tbUtil.getItemsForPage(fullTaskList, $scope.currentPage, $scope.pageSize);
                        prepareData();
                    }, function (error) {
                        $scope.successful = false;
                        $scope.message = error.data.Message;
                    });
        };


        // Prepare tasks for the page
        $scope.pageChanged = function () {
            $scope.tasks = tbUtil.getItemsForPage(fullTaskList, $scope.currentPage, $scope.pageSize);
            prepareData();
        };

        // Variables for the calendars
        var createdDateMin = null;
        var createdDateMax = null;

        var dueDateMin = null;
        var dueDateMax = null;

        var completedDateMin = null;
        var completedDateMax = null;

        $scope.dateFor = "$";
        $scope.dateFrom = null;
        $scope.dateTo = null;

        $scope.minDate = null;
        $scope.maxDate = null;

        $scope.refreshDates = function () {
            toggleMinDate($scope.dateFor);
            toggleMaxDate($scope.dateFor);
        };

        $scope.openFrom = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.openedFrom = !$scope.openedFrom;
        };

        $scope.openTo = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.openedTo = !$scope.openedTo;
        };

        $scope.checkFromDate = function () {
            var startDate = $scope.fromDate;
            var endDate = $scope.toDate;

            if (!startDate || !endDate) {
                return;
            }

            if (!angular.isDate(startDate)) {
                startDate = new Date($scope.fromDate);
            }

            if (!angular.isDate(endDate)) {
                endDate = new Date($scope.toDate);
            }

            if (startDate > endDate) {
                $scope.successful = false;
                $scope.message = "You have selected the 'From' date that is more than 'To' date. Please select the correct 'From' date or leave it empty.";
                $scope.fromDate = null;
            }
            else {
                $scope.successful = true;
                $scope.message = "";
            }

        }

        $scope.checkToDate = function () {
            var startDate = $scope.fromDate;
            var endDate = $scope.toDate;

            if (!startDate || !endDate) {
                return;
            }

            if (!angular.isDate(startDate)) {
                startDate = new Date($scope.fromDate);
            }

            if (!angular.isDate(endDate)) {
                endDate = new Date($scope.toDate);
            }

            if (endDate < startDate) {
                $scope.successful = false;
                $scope.message = "You have selected the 'To' date that is less than 'From' date. Please select the correct 'To' date or leave it empty";
                $scope.toDate = null;
            }
            else {
                $scope.successful = true;
                $scope.message = "";
            }
        };

        // Scope for search
        $scope.search = {};
        $scope.searchFor = "$";

        $scope.clearSearch = function () {
            $scope.search["$"] = "";
            $scope.search[$scope.fields[0].value] = "";
            $scope.search[$scope.fields[1].value] = "";
            $scope.search[$scope.fields[4].value] = "";
            $scope.search[$scope.fields[5].value] = "";
            $scope.search[$scope.fields[6].value] = "";
        };

        // Scope for sort
        $scope.sort = {};
        $scope.sort.column = $scope.fields[0].value;
        $scope.sort.reverse = false;

        $scope.toggleSort = function (index) {
            var column = $scope.fields[index].value;
            if ($scope.sort.column === column) {
                $scope.sort.reverse = !$scope.sort.reverse;
            }
            $scope.sort.column = column;
        }

        // Sort icons
        var iconClass = "pull-right glyphicon glyphicon-sort-by-attributes";
        var iconClassAlt = iconClass + "-alt";

        $scope.selectIconClass = function (index) {
            var column = $scope.fields[index].value;
            if ($scope.sort.column === column) {
                if ($scope.sort.reverse) {
                    return iconClassAlt;
                }
                return iconClass;
            }
            return "";
        };

        var prepareData = function () {
            var temp = null;

            // Prepare unique data for filters
            $scope.titles = tbUtil.uniqueBy($scope.tasks, function (x) { return x.Title }).sort();
            $scope.statuses = tbUtil.uniqueBy($scope.tasks, function (x) { return x.Status }).sort();
            $scope.createdByUsers = tbUtil.uniqueBy($scope.tasks, function (x) { return x.CreatedBy }).sort();
            $scope.assignedToUsers = tbUtil.uniqueBy($scope.tasks, function (x) { return x.AssignedTo }).sort();

            // Calculate the actual date range for each date column. Extend the range by 2 day to be sure the range is fully covered.
            temp = tbUtil.excludeNull($scope.tasks, function (x) { return x.CreatedDate }).sort();
            createdDateMin = tbUtil.addDays(temp[0], -1);
            createdDateMax = tbUtil.addDays(temp.reverse()[0], 1);

            temp = tbUtil.excludeNull($scope.tasks, function (x) { return x.DueDate }).sort();
            dueDateMin = tbUtil.addDays(temp[0], -1);
            dueDateMax = tbUtil.addDays(temp.reverse()[0], 1);

            temp = tbUtil.excludeNull($scope.tasks, function (x) { return x.CompletedDate }).sort();

            // Completed date can be null
            if (temp) {
                completedDateMin = tbUtil.addDays(temp[0], -1);
                completedDateMax = tbUtil.addDays(temp.reverse()[0], 1);
            }
        };
       
        var toggleMinDate = function (dateFor) {
            if (dateFor === "CreatedDate") {
                $scope.minDate = createdDateMin;
            }
            else if (dateFor === "DueDate") {
                $scope.minDate = dueDateMin;
            }
            else if (dateFor === "CompletedDate") {
                $scope.minDate = completedDateMin;
            }
            else { // No column selected
                $scope.minDate = null;
            }
        };

        var toggleMaxDate = function (dateFor) {
            if (dateFor === "CreatedDate") {
                $scope.maxDate = createdDateMax;
            }
            else if (dateFor === "DueDate") {
                $scope.maxDate = dueDateMax;
            }
            else if (dateFor === "CompletedDate") {
                $scope.maxDate = completedDateMax;
            }
            else { // No column selected
                $scope.maxDate = null;
            }
        };

}]);