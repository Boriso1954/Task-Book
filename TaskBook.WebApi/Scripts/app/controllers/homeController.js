"use strict";
app.controller("homeController", ["$scope",  "$modal",
    function ($scope, $modal) {

        $scope.openLoginDialog = function (size) {

            var modalInstance = $modal.open({
                templateUrl: "Scripts/app/views/modalLogin.html",
                controller: "modalLoginController",
                size: size
            });
        };

    }]);