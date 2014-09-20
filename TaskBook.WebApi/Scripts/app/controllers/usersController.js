"use strict";
app.controller("usersController", ["$scope", "$routeParams", "accountService", "taskService", "projectService", "tbUtil",
    function ($scope, $routeParams, accountService, taskService, projectService, tbUtil) {

        $scope.fields = [{
            name: "User Name",
            value: "UserName"
        }, {
            name: "Eamil",
            value: "Email"
        }, {
            name: "First Name",
            value: "FirstName"
        }, {
            name: "Last Name",
            value: "LastName"
        }, {
            name: "Role",
            value: "Role"
        }];

        var authUser = {};
        $scope.manager = {};
        $scope.users = {};

        authUser.UserName = $routeParams.authName;
        $scope.successful = true;
        $scope.message = "";

        var projectId = null;
        accountService.getUserDetailsByUserName(authUser.UserName)
            .then(function (result) {
                $scope.successful = true;
                authUser = result.data;
                if (authUser.Role === "Manager") {
                    // If auth user is manager we know manager's name and project title
                    $scope.manager.userName = authUser.UserName;
                    $scope.manager.projectTitle = authUser.ProjectTitle;
                    projectId = authUser.ProjectId;
                    getUsers(projectId);
                }
                else {
                    // If auth user is not manager we have to ask for manager's details for auth user 
                    projectService.getProjectsAndManagersByProjectId(authUser.ProjectId)
                    .then(function (result) {
                        $scope.successful = true;
                        $scope.manager.userName = result.data.UserName;
                        $scope.manager.projectTitle = result.data.Title;
                        projectId = result.data.ProjectId;
                        getUsers(projectId);
                    }, function (error) {
                        $scope.successful = false;
                        $scope.message = error.data.Message;
                    });
                }
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });

        var getUsers = function (projectId) {
            accountService.getUsersWithRolesByProjectId(projectId)
                   .then(function (result) {
                       $scope.successful = true;
                       $scope.users = result.data;
                       prepareData();
                   }, function (error) {
                       $scope.successful = false;
                       $scope.message = error.data.Message;
                   });
        };

        // Scope for search
        $scope.search = {};
        $scope.searchFor = "$";

        $scope.clearSearch = function () {
            $scope.search["$"] = "";
            $scope.search[$scope.fields[0].value] = "";
            $scope.search[$scope.fields[1].value] = "";
            $scope.search[$scope.fields[2].value] = "";
            $scope.search[$scope.fields[3].value] = "";
            $scope.search[$scope.fields[4].value] = "";
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
            // Prepare unique data for filters
            $scope.userNames = tbUtil.uniqueBy($scope.users, function (x) { return x.UserName }).sort();
            $scope.emails = tbUtil.uniqueBy($scope.users, function (x) { return x.Email }).sort();
            $scope.firstNames = tbUtil.uniqueBy($scope.users, function (x) { return x.FirstName }).sort();
            $scope.lastNames = tbUtil.uniqueBy($scope.users, function (x) { return x.LastName }).sort();
            $scope.roles = tbUtil.uniqueBy($scope.users, function (x) { return x.Role }).sort();
        };

}]);