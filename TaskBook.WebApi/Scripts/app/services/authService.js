"use strict";
app.factory("authService", ["$http", "$q", "localStorageService",
    function ($http, $q, localStorageService) {

    var authServiceFactory = {};

    var _authData = {
        isAuth: false,
        userName: "",
        role: ""
    };

    authServiceFactory.login = function (loginData) {

        var body = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;
        var deferred = $q.defer();

        var config = {
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            }
        };

        $http.post("token", body, config)
            .success(function (response) {
                var value = {
                    token: response.access_token,
                    userName: loginData.userName,
                    rememberMe: loginData.rememberMe
                };

                localStorageService.set("authTbData", value);

                _authData.isAuth = true;
                _authData.userName = loginData.userName;

                deferred.resolve(response);
            })
        .error(function (err, stratus) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    

    authServiceFactory.logOut = function () {

        localStorageService.remove("authTbData");

        _authData.isAuth = false;
        _authData.userName = "";
        _authData.role = "";
    };

    authServiceFactory.fillAuthData = function () {

        var authTbData = localStorageService.get("authTbData");
        if (authTbData) {
            var rememberMe = authTbData.rememberMe;
            if (rememberMe) {
                _authData.isAuth = true;
                _authData.userName = authTbData.userName;
            }
            else {
                _authData.isAuth = false;
                _authData.userName = "";
            }
        }
    };

    authServiceFactory.authData =_authData;

    return authServiceFactory;

}]);