/// <reference path="../_references.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/passwordservice.js" />
"use strict";
describe("$passwordService", function () {
    var passwordService;

    // Excuted before each "it" is run.
    beforeEach(function () {

        // Load the module.
        module("TaskBookApp");

        // Inject a service for testing.
        inject(function (_passwordService_) {
            passwordService = _passwordService_;
        });
    });

    it("should contain a passwordService and set of functions", function () {
        expect(passwordService).not.toBeNull();
        expect(angular.isFunction(passwordService.forgotPassword)).toBeTruthy();
        expect(angular.isFunction(passwordService.resetPassword)).toBeTruthy();
    });
});