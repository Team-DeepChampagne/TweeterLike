'use strict';
app.controller('myTweetsController', ['$scope', '$location',
'authService', '$http', '$route', function ($scope, $location, authService, $http, $route) {

    var serviceBase = 'http://localhost:16270/';

    $scope.postTweetMessage = "";
    $scope.postedTweetSuccessfully = false;
    $scope.deleteTweetMessage = "";
    $scope.deletedTweetSuccessfully = false;
    $scope.feedLimit = 5;
    $scope.currentlyShown = 5;
    $scope.seeReplyForm = false;
    $scope.seeComments = false;

    var currentUsername = authService.authentication.userName;

    $scope.newTweet = {
        Title: "",
        Comment: ""
    };

    $scope.newReply = {
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

            $route.reload(); 
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

    
    $http.get(serviceBase + 'api/Following/Count', { params: { username: currentUsername } }).then(function (response) {
        $scope.followingInfo = response;
        $scope.followingCount = $scope.followingInfo.data;
    },
     function (response) {

     });

    $http.get(serviceBase + 'api/FollowedBy/Count', { params: { username: currentUsername } }).then(function (response) {
        $scope.followedByInfo = response;
        $scope.followedByCount = $scope.followedByInfo.data;
    },
    function (response) {

    });

    $scope.showLoggedUserFollowing = function () {
        $location.path('/logged-user-followings');
    };

    $scope.showLoggedUserFollowedBy = function () {
        $location.path('/logged-user-followed-by');
    };

    $scope.deletePost = function (id) {

        var confirmation = confirm('Are you sure you want to delete this tweet?');

        if (confirmation) {
            $http({
                method: 'DELETE',
                url: serviceBase + 'api/post/' + id
            }).success(function (result) {
                $scope.deleteTweetMessage = "Tweet deleted!";
                $scope.deletedTweetSuccessfully = true;
                $route.reload();
            });
        }
    };

    $scope.postReply = function (currentPostId) {
        $http.post(serviceBase + 'api/reply', $scope.newReply, { params: { postId: currentPostId } }).then(function (response) {
            $scope.newReply = {
                Comment: ""
            };
        },
       function (response) {

       });
    };

    $scope.getReplies = function (currentPostId, tweet) {
        $http({
            method: 'GET',
            url: serviceBase + 'api/reply/',
            params: { postId: currentPostId }
        }).success(function (result) {
            tweet.replies = result;
        });
    };

}]);