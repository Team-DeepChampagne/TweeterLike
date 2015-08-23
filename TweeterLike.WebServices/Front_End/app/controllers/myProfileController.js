'use strict';
app.controller('myProfileController', ['$scope', '$location',
'authService', '$http', function ($scope, $location, authService, $http) {

    var serviceBase = 'http://localhost:16270/';
    $scope.message = "";
    $scope.savedSuccessfully = false;

    $scope.newPassword = {
        OldPassword: "",
        NewPassword: "",
        ConfirmPassword: ""
    };

    $scope.changePassword = function () {
        $http.post(serviceBase + 'api/Account/ChangePassword', $scope.newPassword).then(function (response) {

            $scope.savedSuccessfully = true;
            $scope.message = "Password changed!";

            //Reset the form fields
            $scope.newPassword = {
                OldPassword: "",
                NewPassword: "",
                ConfirmPassword: ""
            };
            return response;   
        },
        
         function (response) {
            
            $scope.message = "Failed to change password! Please try again!";
             //Reset the form fields
             $scope.newPassword = {
                 OldPassword: "",
                 NewPassword: "",
                 ConfirmPassword: ""
             };
         });
    }
  
}]);