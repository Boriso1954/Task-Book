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

    $routeProvider.when("/projects/edit/:projectId", {
        controller: "editProjectController",
        templateUrl: "/Scripts/app/views/editProject.html"
    });

    $routeProvider.when("/projects/new", {
        controller: "createProjectController",
        templateUrl: "/Scripts/app/views/createProject.html"
    });

    $routeProvider.when("/managers/:userName", {
        controller: "managerDetailsController",
        templateUrl: "/Scripts/app/views/managerDetails.html"
    });

    $routeProvider.when("/managers/new/:projectId", {
        controller: "addManagerController",
        templateUrl: "/Scripts/app/views/addManager.html"
    });


    $routeProvider.otherwise({ redirectTo: "/" });
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);
