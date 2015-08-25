'use strict';
app.controller('userProfileController', ['$scope', '$location',
'authService', '$http', function ($scope, $location, authService, $http) {

    var serviceBase = 'http://localhost:16270/';

    var currentUsername = authService.authentication.userName;
    $scope.feedLimit = 5;

    $scope.showMore = function () {
        $scope.feedLimit += 5;
    }

    $scope.showUserTweets = function () {
        $http({
            method: 'GET',
            url: serviceBase + 'api/post',
            params: { username: $scope.searchedUser }
        }).success(function (result) {
            $scope.userTweets = result;
        });
    };

}]);