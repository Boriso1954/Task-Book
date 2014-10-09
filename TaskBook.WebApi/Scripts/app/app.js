var app = angular.module('TaskBookApp', ["ngRoute", "LocalStorageModule", "angular-loading-bar", "ui.bootstrap", "angular.filter"]);

app.config(["$routeProvider", function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "Scripts/app/views/home.html"
    });
    
    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "Scripts/app/views/login.html"
    });

    $routeProvider.when("/forgotPassword", {
        controller: "forgotPasswordController",
        templateUrl: "Scripts/app/views/forgotPassword.html"
    });

    $routeProvider.when("/resetPassword/:userName/:code", {
        controller: "resetPasswordController",
        templateUrl: "Scripts/app/views/resetPassword.html"
    });

    $routeProvider.when("/projects", {
        controller: "projectsController",
        templateUrl: "Scripts/app/views/projects.html"
    });

    $routeProvider.when("/projects/edit/:projectId", {
        controller: "editProjectController",
        templateUrl: "Scripts/app/views/editProject.html"
    });

    $routeProvider.when("/projects/new", {
        controller: "addProjectController",
        templateUrl: "Scripts/app/views/addProject.html"
    });

    $routeProvider.when("/users/new/:projectId/:authName", {
        controller: "addUserController",
        templateUrl: "Scripts/app/views/addUser.html"
    });
        
    $routeProvider.when("/users/edit/:userName", {
        controller: "editUserController",
        templateUrl: "Scripts/app/views/editUser.html"
    });

    $routeProvider.when("/users/:authName", {
        controller: "usersController",
        templateUrl: "Scripts/app/views/users.html"
    });

    $routeProvider.when("/tasks/:authName", {
        controller: "tasksController",
        templateUrl: "Scripts/app/views/tasks.html"
    });

    $routeProvider.when("/tasks/new/:authName", {
        controller: "addTaskController",
        templateUrl: "Scripts/app/views/addTask.html"
    });

    $routeProvider.when("/tasks/edit/:id", {
        controller: "editTaskController",
        templateUrl: "Scripts/app/views/editTask.html"
    });

    $routeProvider.otherwise({ redirectTo: "/" });
}]);

app.config(["$httpProvider", function ($httpProvider) {
    $httpProvider.interceptors.push("authInterceptorService");
}]);

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
