var app = angular.module('TaskBookApp', ['ngRoute', 'LocalStorageModule']);

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

    $routeProvider.otherwise({ redirectTo: "/home" });
});
