"use strict";
app.factory("tbUtil",function () {

    var tbUtilFactory = {};

    tbUtilFactory.uniqueBy = function (arr, fn) {
        var unique = {};
        var distinct = [];
        arr.forEach(function (x) {
            var key = fn(x);
            if (!unique[key]) {
                distinct.push(key);
                unique[key] = true;
            }
        });
        return distinct;
    };

    tbUtilFactory.excludeNull = function (arr, fn) {
        var withoutNull = [];
        arr.forEach(function (x) {
            var key = fn(x);
            if (key) {
                withoutNull.push(key);
            }
        });
        return withoutNull;
    };
    
    tbUtilFactory.getColumnFromCollection = function (arr, fn) {
        var result = [];
        arr.forEach(function (x) {
            var key = fn(x);
            result.push(key);
        });
        return result;
    };

    tbUtilFactory.addDays = function (theDate, days) {

        var d = theDate;
        if (!angular.isDate(d)) {
            d = new Date(theDate);
        }
        return new Date(d.getTime() + days * 24 * 60 * 60 * 1000);
    };

    tbUtilFactory.getItemsForPage = function (items, currentPage, pageSize) {
        var itemsInPage = [];
        if (items && items.length > 0) {
            var start = (currentPage - 1) * pageSize;
            var end = start + pageSize;
            itemsInPage = items.slice(start, end);
        }
        return itemsInPage;
    };

    return tbUtilFactory;

    
});