﻿'use strict';
app.controller('loggedUserFollowingsController', ['$rootScope', '$scope', '$location',
'authService', '$http', function ($rootScope, $scope, $location, authService, $http) {

    var serviceBase = 'http://localhost:16270/';

    var currentUsername = authService.authentication.userName;

    $http({
        method: 'GET',
        url: serviceBase + 'api/Following',
        params: { username: currentUsername }
    }).success(function (result) {
        $scope.loggedUserFollowings = result;

    });

    $scope.getUserTweets = function (foundUser) {
        $rootScope.foundUser = foundUser;
        if (currentUsername == foundUser) {
            $location.path('/my-tweets');
        } else {
            $location.path('/user-profile');
        }
    };

}]);