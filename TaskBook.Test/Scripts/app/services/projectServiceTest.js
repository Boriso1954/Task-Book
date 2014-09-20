/// <reference path="../_references.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/projectservice.js" />
"use strict";
describe("$projectService", function () {
    var projectService;

    // Excuted before each "it" is run.
    beforeEach(function () {

        // Load the module.
        module("TaskBookApp");

        // Inject a service for testing.
        inject(function (_projectService_) {
            projectService = _projectService_;
        });
    });

    it("should contain a projectService and set of functions", function () {
        expect(projectService).not.toBeNull();
        expect(angular.isFunction(projectService.getProjectsAndManagers)).toBeTruthy();
        expect(angular.isFunction(projectService.getProjectsAndManagersByProjectId)).toBeTruthy();
        expect(angular.isFunction(projectService.getProjectById)).toBeTruthy();
        expect(angular.isFunction(projectService.putProject)).toBeTruthy();
        expect(angular.isFunction(projectService.postProject)).toBeTruthy();
        expect(angular.isFunction(projectService.deleteProject)).toBeTruthy();
    });
});