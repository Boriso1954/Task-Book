"use strict";
app.factory("accountService", ["$http", function ($http) {

    var accountServiceFactory = {};

    accountServiceFactory.getUserDetailsByUserName = function (userName) {
        return $http.get("api/Account/GetUserByUserName/" + userName)
    };

    accountServiceFactory.putUserDetails = function (user) {
        return $http.put("api/Account/UpdateUser/" + user.UserId, user)
    };

    accountServiceFactory.postUserDetails = function (user) {
        return $http.post("api/Account/AddUser", user)
    };

    return accountServiceFactory;
}]);