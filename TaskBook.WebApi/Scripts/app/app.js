var app = angular.module("TaskBookApp", ["ngRoute", "LocalStorageModule", "angular-loading-bar"]);

app.config(function ($routeProvider) {

    $routeProvider.when("/", {
        controller: "indexController",
        templateUrl: "/Scripts/app/views/index.html"
    });

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
        controller: "projectsController",
        templateUrl: "/Scripts/app/views/projectsAndMangers.html"
    });

    $routeProvider.otherwise({ redirectTo: "/" });
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);
