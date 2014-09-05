'use strict';
app.controller('layoutController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.logOut = function () {
        authService.logOut();
        $location.path("/home");
    }

    $scope.refresh = function () {
        var path = $location.path();
        $location.path(path);
    };

    $scope.authentication = authService.authData;

}]);