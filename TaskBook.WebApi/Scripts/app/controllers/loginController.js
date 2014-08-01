'use strict';
app.controller('loginController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.loginData = {
        userName: "Admin",
        password: "admin1",
        rememberMe: true
    };
    $scope.message = "";

    $scope.login = function () {

        authService.login($scope.loginData).then(function (response) {

            $location.path('/projectsAndManagers');

        },
         function (err) {
             $scope.message = err.error_description;
         });
    };

}]);