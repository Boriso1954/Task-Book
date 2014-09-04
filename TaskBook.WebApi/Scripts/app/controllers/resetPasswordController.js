"use strict";
app.controller("resetPasswordController", ["$scope", "$routeParams", "passwordService",
    function ($scope, $routeParams, passwordService) {

        $scope.data = {};

        $scope.data.userName = $routeParams.userName;
        $scope.data.code = $routeParams.code;

        $scope.successful = true;
        $scope.message = "";

        $scope.reset = function () {
            var data = $scope.data;
            passwordService.resetPassword(data)
            .then(function (result) {
                $scope.successful = true;
                $scope.message = "The password has been reset successfully.";
                $scope.resetPswForm.$setPristine();
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });
        };

    }]);