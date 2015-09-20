"use strict";
app.controller("layoutController", ["$scope", "$location", "$http", "$modal", "authService",
    function ($scope, $location, $http, $modal, authService) {

    $scope.version = "";
    $scope.created = "";
    var getAppInfo = function () {
        $http.get("api/Info/GetAppVersion")
            .then(function (result) {
                $scope.version = result.data[0];
                $scope.created = result.data[1];
            });
    };

    getAppInfo();

    $scope.logOut = function () {
        authService.logOut();
        $location.path("/home");
    }

    $scope.refresh = function () {
        var path = $location.path();
        $location.path(path);
    };

    $scope.openLoginDialog = function (size) {

        var modalInstance = $modal.open({
            templateUrl: "Scripts/app/views/modalLogin.html",
            controller: "modalLoginController",
            size: size
        });
    };

    $scope.authentication = authService.authData;

}]);