'use strict';
app.controller('userProfileController', ['$rootScope', '$scope', '$location',
'authService', '$http', '$controller', function ($rootScope, $scope, $location, authService, $http, $controller) {

    var serviceBase = 'http://localhost:16270/';

    var currentUsername = authService.authentication.userName;
    $scope.feedLimit = 5;
    $scope.foundUser = $rootScope.foundUser;
    $scope.seeReplyForm = false;
    $scope.seeComments = false;

    $scope.showMore = function () {
        $scope.feedLimit += 5;
    }

    $scope.newReply = {
        Comment: ""
    };

    $http({
        method: 'GET',
        url: serviceBase + 'api/post',
        params: { username: $scope.foundUser }
    }).success(function (result) {
        $scope.foundUserTweets = result;
    });
   
    $scope.followUser = function () {
        $http({
            method: 'POST',
            url: serviceBase + 'api/Follow',
            params: { username: $scope.foundUser }
        }).success(function (result) {
           
        });
    };


    $http.get(serviceBase + 'api/Following/Count', { params: {username: $scope.foundUser}}).then(function (response) {
        $scope.followingInfo = response;
        $scope.followingCount = $scope.followingInfo.data;
    },
    function (response) {

    });

    $http.get(serviceBase + 'api/FollowedBy/Count', { params: {username: $scope.foundUser}}).then(function (response) {
        $scope.followedByInfo = response;
        $scope.followedByCount = $scope.followedByInfo.data;
    },
    function (response) {

    });

    $scope.showUserFollowings = function () {
        $location.path('/user-followings');
    };

    $scope.showUserFollowedBy = function () {
        $location.path('/user-followed-by');
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

    $scope.formatDate = function (tweetDate) {
        var sqlDateTimeArr = tweetDate.split("T");
        var dateSplit = sqlDateTimeArr[0].split('-');
        var year = dateSplit[0];
        var month = dateSplit[1];
        var date = dateSplit[2];
        var timeWithMsSplit = sqlDateTimeArr[1].split(".");
        var timeSplit = timeWithMsSplit[0].split(":");
        var hours = timeSplit[0];
        var minutes = timeSplit[1];
        var seconds = timeSplit[2];
        tweetDate = date + '/' + month + '/' + year + ' ' + hours + ':' + minutes + ':' + seconds;
        return tweetDate;
    };

   
}]);