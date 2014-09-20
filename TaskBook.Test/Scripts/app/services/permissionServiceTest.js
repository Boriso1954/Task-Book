/// <reference path="../_references.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/permissionservice.js" />
"use strict";
describe("$permissionService", function () {
    var permissionService;

    // Excuted before each "it" is run.
    beforeEach(function () {

        // Load the module.
        module("TaskBookApp");

        // Inject a service for testing.
        inject(function (_permissionService_) {
            permissionService = _permissionService_;
        });
    });

    it("should contain a permissionService and set of functions", function () {
        expect(permissionService).not.toBeNull();
        expect(angular.isFunction(permissionService.getPermissionsByRole)).toBeTruthy();
    });
});