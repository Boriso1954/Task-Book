'use strict';
app.controller('loginController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.loginData = {
        userName: "Admin1",
        password: "admin1",
        rememberMe: true
    };
    $scope.message = "";

    $scope.login = function () {

        authService.login($scope.loginData)
            .then(function (response) {
                authService.getRoleByUserName($scope.loginData.userName)
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
                });
            },
            function (err) {
                $scope.message = err.error_description;
            });
    };

}]);