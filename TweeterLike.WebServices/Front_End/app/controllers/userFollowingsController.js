'use strict';
app.controller('userFollowingsController', ['$rootScope', '$scope', '$location',
'authService', '$http', function ($rootScope, $scope, $location, authService, $http) {

    var serviceBase = 'http://localhost:16270/';

    var currentUsername = authService.authentication.userName;
    $scope.foundUser = $rootScope.foundUser;
    $http({
        method: 'GET',
        url: serviceBase + 'api/Following',
        params: { username: $scope.foundUser }
    }).success(function (result) {
        $scope.userFollowings = result;

    });

}]);