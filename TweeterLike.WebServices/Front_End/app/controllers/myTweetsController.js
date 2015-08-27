'use strict';
app.controller('myTweetsController', ['$scope', '$location',
'authService', '$http', function ($scope, $location, authService, $http) {

    var serviceBase = 'http://localhost:16270/';

    $scope.postTweetMessage = "";
    $scope.postedTweetSuccessfully = false;
    $scope.feedLimit = 5;
    var currentUsername = authService.authentication.userName;

    $scope.newTweet = {
        Title: "",
        Comment: ""
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