"use strict";
app.factory("userDetailsService", ["$http", function ($http) {

    var userDetailsServiceFactory = {};

    userDetailsServiceFactory.getUserDetailsByUserName = function (userName) {

        return $http.get("api/users/GetUserDetailsByUserName/" + userName).then(function (result) {
            return result;
        });
    };

    return userDetailsServiceFactory;
}]);