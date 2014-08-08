﻿"use strict";
app.controller("projectsAndManagersController", ["$scope", "projectsService", function ($scope, projectsService) {

    $scope.projectsAndManagers = [];
    $scope.message = 0;
    $scope.successful = true;

    projectsService.getProjectsAndManagers().then(function (result) {
        $scope.successful = true;
        $scope.projectsAndManagers = result.data;
    }, function (error) {
        $scope.successful = false;
        $scope.message = error.data.Message;
    });

    $scope.fields = [{
        name: "Project",
        value: "ProjectTitle"
    }, {
        name: "Manager",
        value: "ManagerName"
    }];

    $scope.search = {};
    $scope.searchFor = "$";
    
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

    $scope.clearSearch = function () {
        $scope.search["$"] = "";
        $scope.search[$scope.fields[0].value] = "";
        $scope.search[$scope.fields[1].value] = "";
    };
}]);