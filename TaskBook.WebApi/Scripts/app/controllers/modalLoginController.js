"use strict";
app.controller("modalLoginController", ["$scope", "$modalInstance", "$location", "authService", "roleService",
    function ($scope, $modalInstance, $location, authService, roleService) {

        $scope.successful = true;
        $scope.message = "";

        $scope.loginData = {
            userName: "",
            password: "",
            rememberMe: false
        };

        $scope.login = function () {
            authService.login($scope.loginData)
                .then(function () {
                    roleService.getRoleByUserName($scope.loginData.userName)
                        .then(function (result) {
                            var role = result;
                            if (role == "Admin") {
                                $location.path("/projects");
                            }
                            else { // Manager, Advanced, User
                                $location.path("/tasks/" + $scope.loginData.userName);
                            }

                            // Close the modal window
                            $modalInstance.close();

                        }, function (error) {
                            $scope.successful = false;
                            $scope.message = error.data.Message;
                        });
                }, function (error) {
                    $scope.successful = false;
                    $scope.message = error.error_description;
                });
        };

        $scope.cancel = function () {
            $modalInstance.dismiss("cancel");
        };

    }]);