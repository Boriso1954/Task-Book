"use strict";
app.controller("editUserController", ["$scope", "$routeParams", "accountService", "permissionService", 
    function ($scope, $routeParams, accountService, permissionService) {

        $scope.user = {};
        $scope.roles = [];

        $scope.user.UserName = $routeParams.userName;
        $scope.successful = true;
        $scope.message = "";

        accountService.getUserDetailsByUserName($scope.user.UserName)
            .then(function (result) {
                $scope.successful = true;
                $scope.user = result.data;
                if ($scope.user.Role == "Manager") {
                    $scope.roles = [$scope.user.Role];
                }
                else {
                    $scope.roles = ["Advanced", "User"];
                }
                getPermissionsByRole($scope.user.Role);
            });

        $scope.getPermissions = function () {
            getPermissionsByRole($scope.user.Role);
        };

        $scope.editUser = function () {
            var user = $scope.user;
            accountService.putUserDetails(user)
            .then(function (result) {
                $scope.successful = true;
                $scope.message = "User's details have been updated.";
                $scope.editUserForm.$setPristine();
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });
        };

        var getPermissionsByRole = function (role) {
            permissionService.getPermissionsByRole(role)
            .then(function (result) {
                $scope.successful = true;
                $scope.user.Permissions = result.data;
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });
        };
}]);