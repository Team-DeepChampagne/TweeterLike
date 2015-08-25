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
    }

    $scope.formatDate = function (tweet) {
        var sqlDateTimeArr = tweet.CreateAt.split("T");
        var dateSplit = sqlDateTimeArr[0].split('-');
        var year = dateSplit[0];
        var month = dateSplit[1];
        var date = dateSplit[2];
        var timeWithMsSplit = sqlDateTimeArr[1].split(".");
        var timeSplit = timeWithMsSplit[0].split(":");
        var hours = timeSplit[0];
        var minutes = timeSplit[1];
        var seconds = timeSplit[2];
        return tweet.CreateAt = date + '/' + month + '/' + year + ' ' + hours + ':' + minutes + ':' + seconds;
    }
    
  
}]);