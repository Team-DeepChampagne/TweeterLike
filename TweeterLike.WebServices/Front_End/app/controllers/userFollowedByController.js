'use strict';
app.controller('userFollowedByController', ['$rootScope', '$scope', '$location',
'authService', '$http', function ($rootScope, $scope, $location, authService, $http) {

    var serviceBase = 'http://localhost:16270/';

    var currentUsername = authService.authentication.userName;
    $scope.foundUser = $rootScope.foundUser;
    $http({
        method: 'GET',
        url: serviceBase + 'api/FollowedBy',
        params: { username: $scope.foundUser }
    }).success(function (result) {
        $scope.userFollowedBy = result;

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