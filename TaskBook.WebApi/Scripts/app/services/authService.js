'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var authServiceFactory = {};

    var _authData = {
        isAuth: false,
        userName: "",
        firstName: "",
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

        $http.post('token', body, config)
            .success(function (response) {
                var value = {
                    token: response.access_token,
                    userName: loginData.userName
                };

                localStorageService.set('authTbData', value);

                _authData.isAuth = true;
                _authData.userName = loginData.userName;
                _authData.firstName = "";
                _authData.role = "Admin";

                deferred.resolve(response);
            })
        .error(function (err, stratus) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var getRole = function (userName) {

    };

    authServiceFactory.fillAuthData = function () {

        var authTbData = localStorageService.get("authTbData");
        if (authTbData) {
            _authData.isAuth = true;
            _authData.userName = authTbData.userName;
        }
    };

    authServiceFactory.authData=_authData;

    return authServiceFactory;

}]);