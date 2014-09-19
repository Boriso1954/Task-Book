"use strict";
app.factory("roleService", ["$http", "authService", function ($http, authService) {

    var roleServiceFactory = {};

    var tbRoles = ["Admin", "Manager", "Advanced", "User"];

    roleServiceFactory.getRoleByUserName = function (userName) {
        return $http.get("api/Roles/GetRolesByUserName/" + userName)
            .then(function (response) {
                var roles = response.data;
                var i = -1;
                var role = "";

                for (var r in tbRoles) {
                    i = roles.indexOf(tbRoles[r]);
                    if (i >= 0) {
                        role = tbRoles[r];
                        break;
                    }
                }
                // Keep role for further use
                authService.authData.role = role;
                return role;
            },
            function (error) {
                authService.authData.role = "";
                return error;
            });
    };

    roleServiceFactory.getRoleByUserId = function (id) {
        return $http.get("api/Roles/GetRolesByUserId/" + id)
            .then(function (response) {
                var roles = response.data;
                var i = -1;
                var role = "";

                for (var r in tbRoles) {
                    i = roles.indexOf(tbRoles[r]);
                    if (i >= 0) {
                        role = tbRoles[r];
                        break;
                    }
                }
                return role;
            },
            function (error) {
                return error;
            });
    };

    return roleServiceFactory;

}]);