'use strict';
app.controller('projectsController', ['$scope', 'projectsService', function ($scope, projectsService) {

    $scope.projectsAndManagers = [];

    projectsService.getProjectsAndManagers().then(function (results) {
        $scope.projectsAndManagers = results.data;

    }, function (error) {
        //alert(error.data.message);
    });

}]);