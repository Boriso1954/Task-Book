"use strict";
app.controller("managerDetailsController", ["$scope", "$routeParams", "userDetailsService", function ($scope, $routeParams, userDetailsService) {

    $scope.user = {};

    $scope.user.UserName = $routeParams.userName;
    $scope.successful = true;
    $scope.message = "";

    userDetailsService.getUserDetailsByUserName($scope.user.UserName)
       .then(function (result) {
           $scope.successful = true;
           $scope.user = result.data;
           userDetailsService.getPermissionsByRole($scope.user.Role)
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

    $scope.editUser = function () {
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