﻿"use strict";
app.controller("userDetailsController", ["$scope", "$routeParams", "userDetailsService", "authService", function ($scope, $routeParams, userDetailsService, authService) {

    $scope.user = {};

    $scope.user.UserName = $routeParams.userName;
    $scope.successful = false;
    $scope.message = "";

    userDetailsService.getUserDetailsByUserName($scope.user.UserName)
       .then(function (result) {
           $scope.successful = true;
           $scope.user = result.data;
           userDetailsService.getUserPermissionsByUserName($scope.user.UserName)
               .then(function (result) {
                   $scope.successful = true;
                   $scope.user.Permissions = result.data;
                   }, function (error) {
                       $scope.successful = false;
                       $scope.message = error.data.Message;
                   })
               }, function (error) {
                   $scope.successful = false;
                   $scope.message = error.data.Message;
               });

    $scope.send = function () {
        var user = $scope.user;
        userDetailsService.putUserDetails(user)
        .then(function (result) {
            $scope.successful = true;
            $scope.message = "User's details have been updated.";
            $scope.userForm.$setPristine();
        }, function (error) {
            $scope.successful = false;
            $scope.message = error.data.Message;
        });
    }
}]);