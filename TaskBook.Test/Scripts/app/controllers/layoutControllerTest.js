/// <reference path="../_references.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/controllers/layoutcontroller.js" />
"use strict";
describe("$layoutController", function () {
    var $scope,
        $controller,
        $location,
        authService;

    beforeEach(function () {
        module("TaskBookApp");
    });

    beforeEach(inject(function (_$controller_, $rootScope, _$location_, _authService_) {
        $scope = $rootScope.$new();
        $controller = _$controller_;
        $location = _$location_;
        authService = _authService_;

        spyOn(authService, "logOut");
        spyOn($location, "path");

        $controller("layoutController",
              { $scope: $scope, $location: $location, authService: authService });

    }));

    it("should perform logout", function () {
        $scope.logOut();
        expect(authService.logOut).toHaveBeenCalled();
        expect($location.path).toHaveBeenCalledWith("/home");
    });

    it("should perform page refresh", function () {
        $scope.refresh();
        expect($location.path.calls.count()).toBe(2);
    });
});