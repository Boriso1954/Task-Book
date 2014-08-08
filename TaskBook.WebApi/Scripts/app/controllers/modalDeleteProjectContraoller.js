"use strict";
app.controller("modalDeleteProjectController", ["$scope", "$modalInstance", function ($scope, $modalInstance) {
    $scope.ok = function () {
        $modalInstance.close("ok");
    };

    $scope.cancel = function () {
        $modalInstance.dismiss("cancel");
    };
}]);