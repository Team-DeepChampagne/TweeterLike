'use strict';
app.controller('userProfileController', ['$rootScope', '$scope', '$location',
'authService', '$http', '$controller', function ($rootScope, $scope, $location, authService, $http, $controller) {

    var serviceBase = 'http://localhost:16270/';

    var currentUsername = authService.authentication.userName;
    $scope.feedLimit = 5;
    $scope.foundUser = $rootScope.foundUser;

    $scope.showMore = function () {
        $scope.feedLimit += 5;
    }

    $http({
        method: 'GET',
        url: serviceBase + 'api/post',
        params: { username: $scope.foundUser }
    }).success(function (result) {
        $scope.foundUserTweets = result;
    });
   
    $scope.followUser = function () {
        $http({
            method: 'POST',
            url: serviceBase + 'api/Follow',
            params: { username: $scope.foundUser }
        }).success(function (result) {
           
        });
    };


    $http.get(serviceBase + 'api/Following/Count').then(function (response) {
        $scope.followingInfo = response;
        $scope.followingCount = $scope.followingInfo.data;
    },
    function (response) {

    });

    $http.get(serviceBase + 'api/FollowedBy/Count').then(function (response) {
        $scope.followedByInfo = response;
        $scope.followedByCount = $scope.followedByInfo.data;
    },
    function (response) {

    });
   
}]);