"use strict";
app.controller("loginController", ["$scope", "$location", "authService", "rolesService",
    function ($scope, $location, authService, rolesService) {

    $scope.loginData = {
        userName: "Admin1",
        password: "admin1",
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
                        else if (response == "Manager") {
                            // TODO
                        }
                        else if (response == "AdvancedUser") {
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