"use strict";
app.controller("managerDetailsController", ["$scope", "$routeParams", "accountService", "permissionsService", 
    function ($scope, $routeParams, accountService, permissionsService) {

    $scope.user = {};

    $scope.user.UserName = $routeParams.userName;
    $scope.successful = true;
    $scope.message = "";

    accountService.getUserDetailsByUserName($scope.user.UserName)
       .then(function (result) {
           $scope.successful = true;
           $scope.user = result.data;
           permissionsService.getPermissionsByRole($scope.user.Role)
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
        accountService.putUserDetails(user)
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