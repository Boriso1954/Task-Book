'use strict';
app.controller('indexController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    if (authService.authData.isAuth) {
        authService.getRoleByUserName(authService.authData.userName)
        .then(function (result) {
            var role = result;
            if (role == "Admin") {
                $location.path("/projectsAndManagers");
            }
            else if (role == "Manager") {
                // TODO
            }
            else if (role == "AdvancedUser") {
                // TODO
            }
            else { // User
                // TODO
            }
        });
    }
    else {
        $location.path("/home");
    }
}]);