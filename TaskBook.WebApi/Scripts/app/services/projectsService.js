'use strict';
app.factory('projectsService', ['$http', function ($http) {

    var projectsServiceFactory = {};

    projectsServiceFactory.getProjectsAndManagers = function () {

        return $http.get('api/ProjectsAndManagers').then(function (results) {
            return results;
        });
    };

    return projectsServiceFactory;
}]);