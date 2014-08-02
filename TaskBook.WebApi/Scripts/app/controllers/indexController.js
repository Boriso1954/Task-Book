'use strict';
app.controller('indexController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    if (authService.authData.isAuth) {
        authService.getRole(authService.authData.userName)
        .then(function (result) {
            var role = result;
            if (role == "Admin") {
                $location.path("/projectsAndManagers");
            }
            else if (response == "Manager") {
                // TODO
            }
            else if (response == "AdvancedUser") {
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