"use strict";
app.factory("rolesService", ["$http", "authService", function ($http, authService) {

    var rolesServiceFactory = {};

    var tbRoles = ["Admin", "Project Manager", "Advanced User", "User"];

    rolesServiceFactory.getRoleByUserName = function (userName) {
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
                authService.authData.role = role;
                return role;
            },
            function (error) {
                return error;
            });
    };

    rolesServiceFactory.getRoleByUserId = function (id) {
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

    return rolesServiceFactory;

}]);