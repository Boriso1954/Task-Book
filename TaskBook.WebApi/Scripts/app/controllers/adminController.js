'use strict';
app.controller('adminController', ['$scope', 'projectService', function ($scope, projectService) {

    $scope.projects = [];

    projectService.getProjects().then(function (results) {
        $scope.project = results.data;

    }, function (error) {
        //alert(error.data.message);
    });

}]);