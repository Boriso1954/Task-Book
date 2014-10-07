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
                    else { // Manager, Advanced, User
                        $location.path("/tasks/" + authService.authData.userName);
                    }
                }, function (error) {
                    $scope.successful = false;
                    $scope.message = error.data.Message;
                });
        }
        else {
            var path = $location.path();
            if (path){
                $location.path(path);
            }
            else {
                $location.path("/home");
            }
        }
    };

    start();

}]);