"use strict";
app.controller("indexController", ["$scope", "$location", "authService", "roleService",
    function ($scope, $location, authService, roleService) {

    $scope.message = "";
    $scope.successful = true;

    var start = function () {
        if (authService.authData.isAuth) {
            roleService.getRoleByUserName(authService.authData.userName)
                .then(function (result) {
                    var role = result;
                    if (role == "Admin") {
                        $location.path("/projects");
                    }
                    else if (role == "Manager") {
                        $location.path("/tasks/" + authService.authData.userName);
                    }
                    else if (role == "Advanced") {
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