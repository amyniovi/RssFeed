
    var rssFeedController = function ($scope, rssService, $interval) {

        var onsuccess = function(data) {
            $scope.feeds = data.data;
        };
        var onerror = function() {
            $scope.feeds = "error";
        };

        var onNewsSuccess = function(data) {
            $scope.breakingNews = data.data;
        };
        var onNewsError = function() {
            $scope.breakingNews = "error";
        };

        var getFeeds = function() {
             rssService.getFeeds().then(onsuccess, onerror);
        };

        var getBreakingNews = function() {
            rssService.getBreakingNews().then(onNewsSuccess, onNewsError);
        };
        var refreshFeeds = function () {
            return $interval(getFeeds, 10000);
        };

        var refreshBreakingNews = function() {
            return $interval(getBreakingNews, 10000);
        };
       
        getFeeds();
        getBreakingNews();
        refreshFeeds();
        refreshBreakingNews();
    };

    angular.module("rss")
        .controller("rssFeedController", rssFeedController);
    

