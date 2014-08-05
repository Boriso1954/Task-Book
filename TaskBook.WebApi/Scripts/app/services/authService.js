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

                localStorageService.set("authTbData", value);

                _authData.isAuth = true;
                _authData.userName = loginData.userName;
                _authData.firstName = "";

                deferred.resolve(response);
            })
        .error(function (err, stratus) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var tbRoles = ["Admin", "Manager", "AdvancrdUser", "User"];

    authServiceFactory.getRoleByUserName = function (userName) {
        return $http.get("api/Account/GetUserRolesByUserName/" + userName)
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
                _authData.role = role;
                return role;
            },
            function (response) {
                // Error
            });
    };

    authServiceFactory.getRoleByUserId = function (id) {
        return $http.get("api/Account/GetUserRolesByUserId/" + id)
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
            function (response) {
                // Error
            });
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
            _authData.isAuth = true;
            _authData.userName = authTbData.userName;
        }
    };

    authServiceFactory.authData =_authData;

    return authServiceFactory;

}]);