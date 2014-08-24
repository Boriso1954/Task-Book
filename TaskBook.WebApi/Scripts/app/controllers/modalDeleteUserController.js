"use strict";
app.controller("modalDeleteUserController", ["$scope", "$modalInstance", function ($scope, $modalInstance) {
    $scope.ok = function () {
        $modalInstance.close("ok");
    };

    $scope.cancel = function () {
        $modalInstance.dismiss("cancel");
    };
}]);