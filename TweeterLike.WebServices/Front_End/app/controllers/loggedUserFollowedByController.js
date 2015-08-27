'use strict';
app.controller('loggedUserFollowedByController', ['$rootScope', '$scope', '$location',
'authService', '$http', function ($rootScope, $scope, $location, authService, $http) {

    var serviceBase = 'http://localhost:16270/';

    var currentUsername = authService.authentication.userName;

    $http({
        method: 'GET',
        url: serviceBase + 'api/FollowedBy',
        params: { username: currentUsername }
    }).success(function (result) {
        $scope.loggedUserFollowedBy = result;

    });

}]);