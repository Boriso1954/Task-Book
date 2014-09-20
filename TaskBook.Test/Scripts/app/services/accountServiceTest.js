/// <reference path="../_references.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/accountservice.js" />
"use strict";
describe("$accountService", function () {
    var accountService;

    // Excuted before each "it" is run.
    beforeEach(function () {

        // Load the module.
        module("TaskBookApp");

        // Inject a service for testing.
        inject(function (_accountService_) {
            accountService = _accountService_;
        });
    });

    it("should contain a accountService and set of functions", function () {
        expect(accountService).not.toBeNull();
        expect(angular.isFunction(accountService.getUserDetailsByUserName)).toBeTruthy();
        expect(angular.isFunction(accountService.getUsersByProjectId)).toBeTruthy();
        expect(angular.isFunction(accountService.getUsersWithRolesByProjectId)).toBeTruthy();
        expect(angular.isFunction(accountService.putUserDetails)).toBeTruthy();
        expect(angular.isFunction(accountService.postUserDetails)).toBeTruthy();
        expect(angular.isFunction(accountService.deleteUser)).toBeTruthy();
    });
});