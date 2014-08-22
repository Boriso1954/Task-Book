"use strict";
app.controller("loginController", ["$scope", "$location", "authService", "roleService",
    function ($scope, $location, authService, roleService) {

    $scope.loginData = {
        userName: "Manager1",
        password: "user12",
        rememberMe: true
    };

    $scope.successful = true;
    $scope.message = "";

    $scope.login = function () {
        authService.login($scope.loginData)
            .then(function (response) {
                roleService.getRoleByUserName($scope.loginData.userName)
                    .then(function (result) {
                        var role = result;
                        if (role == "Admin") {
                            $location.path("/projects");
                        }
                        else if (role == "Manager") {
                            $location.path("/tasks/" + $scope.loginData.userName);
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
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });
    };

}]);