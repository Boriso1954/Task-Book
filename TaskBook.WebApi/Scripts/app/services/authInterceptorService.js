"use strict";
app.factory("authInterceptorService", ["$q", "$location", "localStorageService",
    function ($q, $location, localStorageService) {

    var authInterceptorServiceFactory = {};

    authInterceptorServiceFactory.request = function (config) {

        config.headers = config.headers || {};

        var authData = localStorageService.get("authTbData");
        if (authData) {
            config.headers.Authorization = "Bearer " + authData.token;
        }

        return config;
    }

    authInterceptorServiceFactory.responseError = function (rejection) {
        $location.path("/login/" + rejection.status);
        return $q.reject(rejection);
    }

    return authInterceptorServiceFactory;
}]);