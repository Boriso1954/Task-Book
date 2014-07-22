'use strict';
app.factory('projectService', ['$http', function ($http) {

    var projectServiceFactory = {};

    projectServiceFactory.getProjects = function () {

        return $http.get('api/projects').then(function (results) {
            return results;
        });
    };

    return projectServiceFactory;
}]);