"use strict";
app.factory("userDetailsService", ["$http", function ($http) {

    var userDetailsServiceFactory = {};

    userDetailsServiceFactory.getUserDetailsByUserName = function (userName) {
        return $http.get("api/UserDetails/GetUserDetailsByUserName/" + userName)
    };

    // TODO Obsolete
    userDetailsServiceFactory.getUserPermissionsByUserName = function (userName) {
        return $http.get("api/UserDetails/GetUserPermissionsByUserName/" + userName)
    };

    userDetailsServiceFactory.getPermissionsByRole = function (roleName) {
        return $http.get("api/UserDetails/GetPermissionsByRole/" + roleName)
    };

    userDetailsServiceFactory.putUserDetails = function (user) {
        return $http.put("api/Account/UpdateUser/" + user.UserId, user)
    };

    userDetailsServiceFactory.postUserDetails = function (user) {
        return $http.post("api/Account/AddUser", user)
    };

    return userDetailsServiceFactory;
}]);