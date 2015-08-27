'use strict';
app.controller('userSearchController', ['$rootScope', '$scope', '$location',
'authService', '$http', function ($rootScope, $scope, $location, authService, $http) {
    
    var serviceBase = 'http://localhost:16270/';

    var currentUsername = authService.authentication.userName;

    $scope.findUsers = function () {
        $http({
            method: 'GET',
            url: serviceBase + 'api/user',
            params: { partialName: $scope.searchedUser }
        }).success(function (result) {
            $scope.foundUsers = result;
        });
    };
    
    $scope.getUserTweets = function (foundUser) {
        $rootScope.foundUser = foundUser;
        $location.path('/user-profile');
    }

}]);