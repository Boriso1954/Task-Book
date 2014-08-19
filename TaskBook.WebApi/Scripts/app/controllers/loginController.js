"use strict";
app.controller("loginController", ["$scope", "$location", "authService", "rolesService",
    function ($scope, $location, authService, rolesService) {

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
                rolesService.getRoleByUserName($scope.loginData.userName)
                    .then(function (result) {
                        var role = result;
                        if (role == "Admin") {
                            $location.path("/projectsAndManagers");
                        }
                        else if (role == "Project Manager") {
                            $location.path("/usersAndTasks/" + $scope.loginData.userName);
                        }
                        else if (role == "Advanced User") {
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