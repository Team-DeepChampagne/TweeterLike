'use strict';
app.factory('showTweetsFeedService', ['$http', function ($http) {

    var serviceBase = 'http://localhost:16270/';
    var ordersServiceFactory = {};

    var _getOrders = function () {

        return $http.get(serviceBase + 'api/ShowTweetsFeed').then(function (results) {
            return results;
        });
    };

    ordersServiceFactory.getOrders = _getOrders;

    return ordersServiceFactory;

}]);