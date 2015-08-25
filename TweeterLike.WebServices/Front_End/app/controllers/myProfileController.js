'use strict';
app.controller('myProfileController', ['$scope', '$location',
'authService', '$http', function ($scope, $location, authService, $http) {

    var serviceBase = 'http://localhost:16270/';

    $scope.changePasswordMessage = "";
    $scope.changePasswordSuccessfully = false;
    var currentUsername = authService.authentication.userName;
    $scope.showChangePasswordForm = false;

    $scope.newPassword = {
        OldPassword: "",
        NewPassword: "",
        ConfirmPassword: ""
    };

    $scope.showChangePassword = function () {
        $scope.showChangePasswordForm = !$scope.showChangePasswordForm;
    };

    $scope.changePassword = function () {
        $http.post(serviceBase + 'api/Account/ChangePassword', $scope.newPassword).then(function (response) {

            $scope.changePasswordSuccessfully = true;
            $scope.changePasswordMessage = "Password changed!";

            //Reset the form fields
            $scope.newPassword = {
                OldPassword: "",
                NewPassword: "",
                ConfirmPassword: ""
            };
            return response;
        },

         function (response) {
             $scope.changePasswordMessage = "Failed to change password! Please try again!";
             //Reset the form fields
             $scope.newPassword = {
                 OldPassword: "",
                 NewPassword: "",
                 ConfirmPassword: ""
             };
         });
    }

    $http({
        method: 'GET',
        url: serviceBase + 'api/user',
        params: { username: currentUsername }
    }).success(function (result) {
        $scope.userInfo = result;
    });

    $http({
        method: 'GET',
        url: serviceBase + 'api/Following'
    }).success(function (result) {
        $scope.usersYouAreFollowing = result;
    });

    $scope.deleteUser = function () {

        var confirmation = confirm('Are you sure? Your account will be deleted forever and all of your data will be lost!');

        if (confirmation) {
            $http({
                method: 'DELETE',
                url: serviceBase + 'api/user',
                params: { username: currentUsername }
            }).success(function (result) {
                $scope.deleteSuccess = result;
                authService.logOut();
                $location.path('/home');
            });
        }
    };

}]);