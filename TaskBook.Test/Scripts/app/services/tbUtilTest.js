/// <reference path="../_references.js" />
/// <reference path="../../../../taskbook.webapi/scripts/app/services/tbutil.js" />
"use strict";
describe("$tbUtil", function () {
    var tbUtil;

    // Excuted before each "it" is run.
    beforeEach(function () {

        // Load the module.
        module("TaskBookApp");

        // Inject a service for testing.
        inject(function (_tbUtil_) {
            tbUtil = _tbUtil_;
        });
    });

    it("should contain tbUtil service and set of functions", function () {
        expect(tbUtil).not.toBeNull();
        expect(angular.isFunction(tbUtil.uniqueBy)).toBeTruthy();
        expect(angular.isFunction(tbUtil.excludeNull)).toBeTruthy();
        expect(angular.isFunction(tbUtil.getColumnFromCollection)).toBeTruthy();
        expect(angular.isFunction(tbUtil.addDays)).toBeTruthy();
        expect(angular.isFunction(tbUtil.getItemsForPage)).toBeTruthy();
    });

    it("should exclude duplications from array", function () {
        var arr = [{ v: "a" }, { v: "b" }, { v: "b" }];
        var result = tbUtil.uniqueBy(arr, function (x) { return x.v });
        expect(result).toEqual(["a", "b"]);
    });

    it("should exclude null from array", function () {
        var arr = [{ v: "a" }, { v: null }, { v: "b" }];
        var result = tbUtil.excludeNull(arr, function (x) { return x.v });
        expect(result).toEqual(["a", "b"]);
    });

    it("should get column values from array of objects", function () {
        var arr = [{ k: 1, v: "a" }, { k: 2, v: "b" }, { k: 3, v: "c" }];
        var result = tbUtil.getColumnFromCollection(arr, function (x) { return x.v });
        expect(result).toEqual(["a", "b", "c"]);
    });

    it("should add days to date", function () {
        var year = 2014;
        var month = 1;
        var day = 1;
        var days = 3;
        var date1 = new Date(year, month, day);
        var date2 = new Date(year, month, day + days);
        var date3 = tbUtil.addDays(date1, days);

        expect(date3).toEqual(date2);
    });

    it("should get items for specified page", function () {
        var arr = [{ k: 1, v: "a" }, { k: 2, v: "b" }, { k: 3, v: "c" }, { k: 4, v: "d" }];
        var result = tbUtil.getItemsForPage(arr, 2, 2);
        expect(result).toEqual([{ k: 3, v: "c" }, { k: 4, v: "d" }]);
    });
});