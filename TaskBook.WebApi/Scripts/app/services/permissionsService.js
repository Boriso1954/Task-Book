"use strict";
app.factory("permissionsService", ["$http", function ($http) {

    var permissionsServiceFactory = {};

    permissionsServiceFactory.getPermissionsByRole = function (roleName) {
        return $http.get("api/Permissions/GetByRole/" + roleName)
    };

    return permissionsServiceFactory;
}]);