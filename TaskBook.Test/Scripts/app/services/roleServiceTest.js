/// <reference path="../_references.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/roleservice.js" />
"use strict";
describe("$roleService", function () {
    var roleService,
        authService,
        $httpBackend;

    beforeEach(function () {
        module("TaskBookApp");
    });

    // Get service and $httpBackend
    beforeEach(inject(function (_roleService_, _$httpBackend_) {
        $httpBackend = _$httpBackend_;
        roleService = _roleService_;
    }));

    beforeEach(inject(function (_authService_) {
        authService = _authService_;
    }));

    // make sure no expectations were missed in the tests
    // (e.g. expectGET or expectPOST)
    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it("should contain roleService and authService services", function () {
        expect(roleService).not.toBeNull();
        expect(angular.isFunction(roleService.getRoleByUserName)).toBeTruthy();
        expect(angular.isFunction(roleService.getRoleByUserId)).toBeTruthy();

        expect(authService).not.toBeNull();
    });

    it("should get user's role successfully", function () {
        var userName = "user1";

        // Set up data for the http call to return user's role(s)
        var returnRoles = ["Manager"];

        // expectGET to make sure this is called once.
        $httpBackend.expectGET("api/Roles/GetRolesByUserName/" + userName).respond(returnRoles);

        // Make the call.
        var returnedPromise = roleService.getRoleByUserName(userName);

        // Set up a handler for the response, that will put the result
        // into a variable in this scope for you to test.
        var result;
        returnedPromise.then(function(response) {
            result = response;
        });

        // Flush the backend to "execute" the $http.get request
        $httpBackend.flush();

        // Check the result 
        expect(result).toBe(returnRoles[0]);
        expect(authService.authData.role).toBe(result);
    });

    it("should get user's role with error", function () {
        var userName = "user1";
        $httpBackend.expectGET("api/Roles/GetRolesByUserName/" + userName).respond(500, "Get user's role error");
        var returnedPromise = roleService.getRoleByUserName(userName);
        var result;
        returnedPromise.then(function (response) {
            result = response;
        });
        $httpBackend.flush();

        expect(result.status).toBe(500);
        expect(authService.authData.role).toBe("");
    });

    it("should get user's role by user ID successfully", function () {
        var userId = "AAAA-BBBB";
        var returnRoles = ["Manager"];

        $httpBackend.expectGET("api/Roles/GetRolesByUserId/" + userId).respond(returnRoles);

        var returnedPromise = roleService.getRoleByUserId(userId);

        var result;
        returnedPromise.then(function (response) {
            result = response;
        });

        $httpBackend.flush();

        expect(result).toBe(returnRoles[0]);
    });

    it("should get user's role by user ID with error", function () {
        var userId = "AAAA-BBBB";
        $httpBackend.expectGET("api/Roles/GetRolesByUserId/" + userId).respond(500, "Get user's role error");
        var returnedPromise = roleService.getRoleByUserId(userId);
        var result;
        returnedPromise.then(function (response) {
            result = response;
        });
        $httpBackend.flush();

        expect(result.status).toBe(500);
    });

    it("getRolesByUserName returns a role that is not in the role list", function () {
        var userName = "user1";
        var returnRoles = ["XYZ"];

        $httpBackend.expectGET("api/Roles/GetRolesByUserName/" + userName).respond(returnRoles);

        var returnedPromise = roleService.getRoleByUserName(userName);

        var result;
        returnedPromise.then(function (response) {
            result = response;
        });

        $httpBackend.flush();

        // Check the result 
        expect(result).toBe("");
        expect(authService.authData.role).toBe("");
    });

    it("getRolesByUserId  returns a role that is not in the role list", function () {
        var userId = "AAAA-BBBB";
        var returnRoles = ["XYZ"];

        $httpBackend.expectGET("api/Roles/GetRolesByUserId/" + userId).respond(returnRoles);

        var returnedPromise = roleService.getRoleByUserId(userId);

        var result;
        returnedPromise.then(function (response) {
            result = response;
        });

        $httpBackend.flush();

        expect(result).toBe("");
    });

});