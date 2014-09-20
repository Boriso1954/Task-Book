/// <reference path="../_references.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/authInterceptorService.js" />
"use strict";
describe("$authInterceptorService", function () {
    var authInterceptorService,
        localStorageService;

    var authData = { token: "zzz" };
    var config = {};

    beforeEach(function () {
        module("TaskBookApp");
    });

    beforeEach(inject(function (_authInterceptorService_, _localStorageService_) {
        authInterceptorService = _authInterceptorService_;
        localStorageService = _localStorageService_
        localStorageService.get = jasmine.createSpy().and.returnValue(authData);
    }));

    it("should contain authInterceptorService and localStorageService services", function () {
        expect(authInterceptorService).not.toBeNull();
        expect(angular.isFunction(authInterceptorService.request)).toBeTruthy();
        expect(angular.isFunction(authInterceptorService.responseError)).toBeTruthy();

        expect(localStorageService).not.toBeNull();
        expect(angular.isFunction(localStorageService.get)).toBeTruthy();
    });

    it("should includ in the request the bearer", function () {
        authInterceptorService.request(config);
        expect(config.headers.Authorization).toBe("Bearer " + authData.token);
    });
});