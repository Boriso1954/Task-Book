"use strict";
app.controller("forgotPasswordController", ["$scope", "passwordService", 
    function ($scope, passwordService) {

        $scope.send = function () {
            var data = $scope.data;
            passwordService.forgotPassword(data)
            .then(function (result) {
                $scope.successful = true;
                $scope.message = "Please check your email to reset your password.";
                $scope.forgotPswForm.$setPristine();
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.ExceptionMessage;
            });
        };

    }]);