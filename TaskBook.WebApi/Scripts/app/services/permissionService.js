"use strict";
app.factory("permissionService", ["$http", function ($http) {

    var permissionServiceFactory = {};

    permissionServiceFactory.getPermissionsByRole = function (roleName) {
        return $http.get("api/Permissions/GetByRole/" + roleName)
    };

    return permissionServiceFactory;
}]);