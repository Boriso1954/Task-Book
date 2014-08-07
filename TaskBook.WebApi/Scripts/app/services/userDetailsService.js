"use strict";
app.factory("userDetailsService", ["$http", function ($http) {

    var userDetailsServiceFactory = {};

    userDetailsServiceFactory.getUserDetailsByUserName = function (userName) {
        return $http.get("api/UserDetails/GetUserDetailsByUserName/" + userName)
    };

    //userDetailsServiceFactory.getUserPermissionsByUserName = function (userName) {
    //    return $http.get("api/UserDetails/GetUserPermissionsByUserName/" + userName).then(function (result) {
    //        return result;
    //    });
    //};

    userDetailsServiceFactory.getUserPermissionsByUserName = function (userName) {
        return $http.get("api/UserDetails/GetUserPermissionsByUserName/" + userName)
    };

    userDetailsServiceFactory.putUserDetails = function (user) {
        return $http.put("api/Account/UpdateUser/" + user.UserId, user)
    };

    return userDetailsServiceFactory;
}]);