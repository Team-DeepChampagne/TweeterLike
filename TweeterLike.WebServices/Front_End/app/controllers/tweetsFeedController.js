'use strict';
app.controller('tweetsFeedController', ['$scope', '$location',
'authService', '$http', '$rootScope', function ($scope, $location, authService, $http, $rootScope) {

    var serviceBase = 'http://localhost:16270/';
    $scope.feedLimit = 5;
    var currentUsername = authService.authentication.userName;
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
        url: serviceBase + 'api/post'
    }).success(function (result) {
        $scope.followingsTweets = result;
    });

    $scope.getUserTweets = function (foundUser) {
        $rootScope.foundUser = foundUser;
        if (currentUsername == foundUser) {
            $location.path('/my-tweets');
        } else {
            $location.path('/user-profile');
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