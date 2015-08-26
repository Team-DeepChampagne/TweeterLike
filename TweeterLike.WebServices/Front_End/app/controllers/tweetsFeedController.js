'use strict';
app.controller('tweetsFeedController', ['$scope', '$location',
'authService', '$http', function ($scope, $location, authService, $http) {

    var serviceBase = 'http://localhost:16270/';
    var newPassword = {
        NewPassword: '1234',
        ConfirmPassword: '1234'
    };

    $http.post(serviceBase + 'api/Account/SetPassword', newPassword).then(function (results) {
        return results;
    });

}]);