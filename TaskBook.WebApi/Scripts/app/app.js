﻿var app = angular.module("TaskBookApp", ["ngRoute", "LocalStorageModule", "angular-loading-bar", "ui.bootstrap", "angular.filter"]);



app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/Scripts/app/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/Scripts/app/views/login.html"
    });

    $routeProvider.when("/forgotPassword", {
        controller: "forgotPasswordController",
        templateUrl: "/Scripts/app/views/forgotPassword.html"
    });

    $routeProvider.when("/projectsAndManagers", {
        controller: "projectsAndManagersController",
        templateUrl: "/Scripts/app/views/projectsAndManagers.html"
    });

    $routeProvider.when("/projects/edit/:projectId", {
        controller: "editProjectController",
        templateUrl: "/Scripts/app/views/editProject.html"
    });

    $routeProvider.when("/projects/new", {
        controller: "createProjectController",
        templateUrl: "/Scripts/app/views/createProject.html"
    });

    $routeProvider.when("/managers/edit/:userName", {
        controller: "managerDetailsController",
        templateUrl: "/Scripts/app/views/managerDetails.html"
    });

    $routeProvider.when("/managers/new/:projectId", {
        controller: "addManagerController",
        templateUrl: "/Scripts/app/views/addManager.html"
    });

    $routeProvider.when("/usersAndTasks/:userName", {
        controller: "usersAndTasksManagerController",
        templateUrl: "/Scripts/app/views/usersAndTasksManager.html"
    });


    $routeProvider.otherwise({ redirectTo: "/" });
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.filter("dateRange", function () {
    return function (items, dateColumn, fromDate, toDate) {
        var filtered = [];

        var minDate = new Date(-100000000 * 86400000);
        var maxDate = new Date(100000000 * 86400000);

        var startDate = fromDate ? new Date(fromDate) : minDate;
        var endDate = toDate ? new Date(toDate) : maxDate;

        for (var i = 0; i < items.length; i++) {
            if (dateColumn === "$") {
                filtered.push(items[i]);
            }
            else {
                var currentDate = new Date(items[i][dateColumn]);
                if (currentDate >= startDate && currentDate <= endDate) {
                    filtered.push(items[i]);
                }
            }
        }
        return filtered;
    };
});

app.run(["authService", function (authService) {
    authService.fillAuthData();
}]);
