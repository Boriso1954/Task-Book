"use strict";
app.controller("editUserController", ["$scope", "$routeParams", "$modal", "$location", "$timeout", "accountService", "permissionService", "authService",
    function ($scope, $routeParams, $modal, $location, $timeout, accountService, permissionService, authService) {

        $scope.user = {};
        $scope.roles = [];

        $scope.user.UserName = $routeParams.userName;
        $scope.successful = true;
        $scope.message = "";

        var managerName = authService.authData.userName;

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
                $location.path("/users/" + managerName);
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