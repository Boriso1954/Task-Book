var app = angular.module('TaskBookApp', ['ngRoute', 'LocalStorageModule']);

app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/Scripts/app/views/home.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});
