"use strict";
app.factory("userDetailsService", ["$http", function ($http) {

    var userDetailsServiceFactory = {};

    userDetailsServiceFactory.getUserDetailsByUserName = function (userName) {

        return $http.get("api/UserDetails/GetUserDetailsByUserName/" + userName).then(function (result) {
            return result;
        });
    };

    userDetailsServiceFactory.putUserDetails = function (user) {
        return $http.put("api/Account/UpdateUser/" + user.UserId, user).then(function (result) {
            return result;
        });
    }

    return userDetailsServiceFactory;
}]);