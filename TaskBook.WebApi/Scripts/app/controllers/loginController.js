"use strict";
app.controller("loginController", ["$scope", "$location", "$routeParams", "authService", "roleService",
    function ($scope, $location, $routeParams, authService, roleService) {

        $scope.loginData = {
            userName: "Manager1",
            password: "user12",
            rememberMe: false
        };

        $scope.successful = true;
        $scope.message = "";

        $scope.login = function () {
            authService.login($scope.loginData)
                .then(function () {
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
                                $location.path("/tasks/" + $scope.loginData.userName);
                            }
                            else { // User
                                $location.path("/tasks/" + $scope.loginData.userName);
                            }
                        }, function (error) {
                            $scope.successful = false;
                            $scope.message = error.data.Message;
                        });
                }, function (error) {
                    $scope.successful = false;
                    $scope.message = error.error_description;
                });
        };

    }]);