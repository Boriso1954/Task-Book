"use strict";
app.factory("accountService", ["$http", function ($http) {

    var accountServiceFactory = {};

    accountServiceFactory.getUserDetailsByUserName = function (userName) {
        return $http.get("api/Account/GetUserByUserName/" + userName)
    };

    accountServiceFactory.getUsersByProjectId = function (projectId) {
        return $http.get("api/Account/GetUsersByProjectId/" + projectId)
    };

    accountServiceFactory.getUsersWithRolesByProjectId = function (projectId) {
        return $http.get("api/Account/GetUsersWithRolesByProjectId/" + projectId)
    };

    accountServiceFactory.putUserDetails = function (user) {
        return $http.put("api/Account/UpdateUser/" + user.UserId, user)
    };

    accountServiceFactory.postUserDetails = function (user) {
        return $http.post("api/Account/AddUser", user)
    };

    accountServiceFactory.deleteUser = function (user) {
        return $http.delete("api/Account/DeleteUser/" + user.UserId)
    };

    return accountServiceFactory;
}]);