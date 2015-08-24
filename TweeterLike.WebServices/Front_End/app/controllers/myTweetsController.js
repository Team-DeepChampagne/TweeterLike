'use strict';
app.controller('myTweetsController', ['$scope', '$location',
'authService', '$http', function ($scope, $location, authService, $http) {

    var serviceBase = 'http://localhost:16270/';

    $scope.postTweetMessage = "";
    $scope.postedTweetSuccessfully = false;
    $scope.changePasswordMessage = "";
    $scope.changePasswordSuccessfully = false;
    $scope.feedLimit = 5;
    var currentUsername = authService.authentication.userName;

    $scope.newTweet = {
        Title: "",
        Comment: ""
    };

    $scope.newPassword = {
        OldPassword: "",
        NewPassword: "",
        ConfirmPassword: ""
    };

    $scope.showMore = function () {
        $scope.feedLimit += 5;
    }

    $http({
        method: 'GET',
        url: serviceBase + 'api/post',
        params: { username: currentUsername }
    }).success(function (result) {
        $scope.tweets = result;
    });

    $scope.postTweet = function () {
        $http.post(serviceBase + 'api/Post', $scope.newTweet).then(function (response) {

            $scope.postedTweetSuccessfully = true;
            $scope.postTweetMessage = "Succesfuly posted!";

            //Reset the form fields
            $scope.newTweet = {
                Title: "",
                Comment: ""
            };
            return response;
        },

         function (response) {
             $scope.postTweetMessage = "Failed to post tweet! Please try again!";
             //Reset the form fields
             $scope.newTweet = {
                 Title: "",
                 Comment: ""
             };
         });
    }


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
  
}]);