"use strict";
app.factory("passwordService", ["$http", function ($http) {

    var passwordServiceFactory = {};
   
    passwordServiceFactory.forgotPassword = function (data) {
        return $http.post("api/Account/ForgotPassword", data)
    };

    passwordServiceFactory.resetPassword = function (data) {
        return $http.post("api/Account/ResetPassword", data)
    };

    return passwordServiceFactory;
}]);