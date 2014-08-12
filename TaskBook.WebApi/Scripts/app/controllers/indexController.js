﻿"use strict";
app.controller("indexController", ["$scope", "$location", "authService", "rolesService",
    function ($scope, $location, authService, rolesService) {

    $scope.message = "";
    $scope.successful = true;

    var start = function () {
        if (authService.authData.isAuth) {
            rolesService.getRoleByUserName(authService.authData.userName)
                .then(function (result) {
                    var role = result;
                    if (role == "Admin") {
                        $location.path("/projectsAndManagers");
                    }
                    else if (role == "Manager") {
                        // TODO
                    }
                    else if (role == "AdvancedUser") {
                        // TODO
                    }
                    else { // User
                        // TODO
                    }
                }, function (error) {
                    $scope.successful = false;
                    $scope.message = error.data.Message;
                });
        }
        else {
            $location.path("/home");
        }
    };

    start();

}]);