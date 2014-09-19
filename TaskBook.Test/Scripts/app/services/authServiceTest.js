/// <reference path="../_references.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/authservice.js" />
"use strict";
describe("$authService", function () {
    var authService,
        $httpBackend,
        promise,
        tokenUrl = "token",
        successCallback,
        errorCallback,
        localStorageService;
    
    var loginData = {
        userName: "user1",
        password: "psw123",
        rememberMe: false
    };

    var response = {
        access_token: "zzz"
    };

    var value = {
        token: response.access_token,
        userName: loginData.userName,
        rememberMe: loginData.rememberMe
    };

    beforeEach(function () {
        module("TaskBookApp");
    });

    beforeEach(inject(function (_authService_, _$httpBackend_) {
        $httpBackend = _$httpBackend_;
        successCallback = jasmine.createSpy();
        errorCallback = jasmine.createSpy();
        authService = _authService_;
    }));

    beforeEach(inject(function (_localStorageService_) {
        localStorageService = _localStorageService_;
        localStorageService.set = jasmine.createSpy();
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it("should contain authService and localStorageService services", function () {
        expect(authService).not.toBeNull();
        expect(angular.isFunction(authService.logOut)).toBeTruthy();
        expect(angular.isFunction(authService.login)).toBeTruthy();
        expect(angular.isFunction(authService.fillAuthData)).toBeTruthy();

        expect(localStorageService).not.toBeNull();
        expect(angular.isFunction(localStorageService.set)).toBeTruthy();
    });

    it("returns http token requests successfully and resolves the promise", function () {

        $httpBackend.expectPOST(tokenUrl).respond(response);
        promise = authService.login(loginData);
        promise.then(successCallback, errorCallback);

        $httpBackend.flush();

        expect(successCallback).toHaveBeenCalledWith(response);
        expect(errorCallback).not.toHaveBeenCalled();
        expect(authService.authData.isAuth).toBeTruthy();
        expect(localStorageService.set).toHaveBeenCalledWith("authTbData", value);
    });

    it("returns http token requests with an error and rejects the promise", function () {
        $httpBackend.expectPOST(tokenUrl).respond(500, "Token error");
        promise = authService.login(loginData);
        promise.then(successCallback, errorCallback);

        $httpBackend.flush();

        expect(successCallback).not.toHaveBeenCalled();
        expect(errorCallback).toHaveBeenCalled();
        expect(authService.authData.isAuth).toBeFalsy();
        expect(localStorageService.set).not.toHaveBeenCalled();
    });
});