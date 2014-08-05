"use strict";
app.controller("userDetailsController", ["$scope", "$routeParams", "userDetailsService", "authService", function ($scope, $routeParams, userDetailsService, authService) {

    $scope.user = {
        UserId: "",
        UserName: "",
        Umail: "",
        FirstName: "",
        LastName: "",
        ProjectId: "",
        ProjectTitle: "",
        Role: ""
    };

    $scope.user.UserName = $routeParams.userName;

    $scope.message = "";

    userDetailsService.getUserDetailsByUserName($scope.user.UserName)
        .then(function (result) {
            $scope.user = result.data;
            authService.getRoleByUserId($scope.user.UserId)
                .then(function (result) {
                    $scope.user.Role = result;
                });
        }, function (error) {
            $scope.message = error.data.message;
        });
}]);