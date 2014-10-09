"use strict";
app.controller("permissionsController", ["$scope", "$routeParams", "permissionService",
    function ($scope, $routeParams, permissionService) {

        $scope.role = $routeParams.role;

        permissionService.getPermissionsByRole($scope.role) 
            var role = $scope.role;
            permissionService.getPermissionsByRole(role)
            .then(function (result) {
                $scope.successful = true;
                $scope.permissions = result.data;
            }, function (error) {
                $scope.successful = false;
                $scope.message = error.data.Message;
            });

    }]);