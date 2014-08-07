var app = angular.module("TaskBookApp", ["ngRoute", "LocalStorageModule", "angular-loading-bar", "ui.bootstrap"]);

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

    $routeProvider.when("/projects/:projectId", {
        controller: "projectsController",
        templateUrl: "/Scripts/app/views/project.html"
    });

    $routeProvider.when("/userDetails/:userName", {
        controller: "userDetailsController",
        templateUrl: "/Scripts/app/views/userDetails.html"
    });

    $routeProvider.otherwise({ redirectTo: "/" });
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);
