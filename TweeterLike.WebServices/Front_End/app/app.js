var app = angular.module('TwitterLike',
['ngRoute', 'LocalStorageModule', 'angular-loading-bar']);

app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "app/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "app/views/signup.html"
    });

    $routeProvider.when("/tweets-feed", {
        controller: "tweetsFeedController",
        templateUrl: "app/views/tweets-feed.html"
    });

    $routeProvider.when("/my-tweets", {
        controller: "myTweetsController",
        templateUrl: "app/views/my-tweets.html"
    });

    $routeProvider.when("/my-profile", {
        controller: "myProfileController",
        templateUrl: "app/views/my-profile.html"
    });

    $routeProvider.when("/user-search", {
        controller: "userSearchController",
        templateUrl: "app/views/user-search.html"
    });

    $routeProvider.when("/user-profile", {
        controller: "userProfileController",
        templateUrl: "app/views/user-profile.html"
    });

    $routeProvider.when("/logged-user-followings", {
        controller: "loggedUserFollowingsController",
        templateUrl: "app/views/logged-user-followings.html"
    });

    $routeProvider.when("/user-followings", {
        controller: "userFollowingsController",
        templateUrl: "app/views/user-followings.html"
    });

    $routeProvider.when("/logged-user-followed-by", {
        controller: "loggedUserFollowedByController",
        templateUrl: "app/views/logged-user-followed-by.html"
    });

    $routeProvider.when("/user-followed-by", {
        controller: "userFollowedByController",
        templateUrl: "app/views/user-followed-by.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});