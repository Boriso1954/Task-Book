/// <reference path="../_references.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/taskservice.js" />
"use strict";
describe("$taskService", function () {
    var taskService;

    // Excuted before each "it" is run.
    beforeEach(function () {

        // Load the module.
        module("TaskBookApp");

        // Inject a service for testing.
        inject(function (_taskService_) {
            taskService = _taskService_;
        });
    });

    it("should contain a taskService and set of functions", function () {
        expect(taskService).not.toBeNull();
        expect(angular.isFunction(taskService.getTasksByProjectId)).toBeTruthy();
        expect(angular.isFunction(taskService.getTasksByUserName)).toBeTruthy();
        expect(angular.isFunction(taskService.getTaskById)).toBeTruthy();
        expect(angular.isFunction(taskService.putTask)).toBeTruthy();
        expect(angular.isFunction(taskService.postTask)).toBeTruthy();
        expect(angular.isFunction(taskService.deleteTask)).toBeTruthy();
    });
});