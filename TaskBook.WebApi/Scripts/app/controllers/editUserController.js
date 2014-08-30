"use strict";
app.controller("editUserController", ["$scope", "$routeParams", "$modal", "$location", "$timeout", "accountService", "permissionService", "authService",
    function ($scope, $routeParams, $modal, $location, $timeout, accountService, permissionService, authService) {

        $scope.user = {};
        $scope.roles = [];

        $scope.user.UserName = $routeParams.userName;
        $scope.successful = true;
        $scope.message = "";

        var authName = authService.authData.userName;

        accountService.getUserDetailsByUserName($scope.user.UserName)
            .then(function (result) {
                $scope.successful = true;
                $scope.user = result.data;
                if ($scope.user.Role == "Admin") {
                    $scope.roles = ["Admin"];
                }
                else if ($scope.user.Role == "Manager") {
                    $scope.roles = ["Manager"];
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

        $scope.openModalDeleteUserDialog = function (size) {

            var modalInstance = $modal.open({
                templateUrl: "/Scripts/app/views/modalDeleteUser.html",
                controller: "modalDeleteUserController",
                size: size
            });

            modalInstance.result.then(function (result) {
                if (result == "ok") {
                    deleteUser();
                }
            }, function () {
                // Do nothing
            });
        };

        $scope.IsDeletionAllowed = function () {
            var result = false;
            if ($scope.user.Role == "Manager") {
                // Manager's account can be deleted only with project by Admin
                result = false;
            }
            else if (authService.authData.role == "Manager") {
                // Only Manager can delete user account
                result = true;
            }
            return result;
        };

        $scope.IsUpdateAllowed = function () {
            var result = false;
            if (authService.authData.role == "Admin" || authService.authData.role == "Manager") {
                // Admin can edit Manager's account
                // Manager can edit User's and Advaced User's accounts
                result= true;
            }
            else if (authService.authData.userName.toLowerCase() == $scope.user.UserName.toLowerCase()) {
                // User and Advanced User can edit only own account
                result = true;
            }
            return result;
        };

        var deleteUser = function () {
            var user = $scope.user;
            accountService.deleteUser(user)
            .then(function (result) {
                $scope.message = "User has been deleted successfully. You will be redirected to the task list in 3 seconds.";
                startTimer();
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });
        };

        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $location.path("/users/" + authName);
            }, 3000);
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