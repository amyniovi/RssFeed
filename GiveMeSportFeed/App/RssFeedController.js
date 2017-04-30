///Here the contorller invokes the client service periodically to make sure the RssFeeds are refreshed if they have been updated.
///
var rssFeedController = function ($scope, rssService, $interval) {

    var onsuccess = function(data) {
        $scope.feeds = data.data;
    };

    var onNewsSuccess = function (data) {
        $scope.breakingNews = data.data;
    };

    var onerror = function() {
        $scope.feeds = null;
    };

    var onNewsError = function() {
        $scope.breakingNews = null;
    };

    var getFeeds = function() {
        rssService.getFeeds().then(onsuccess, onerror);
    };

    var getBreakingNews = function() {
        rssService.getBreakingNews().then(onNewsSuccess, onNewsError);
    };

    //every 2 minutes
    var refreshFeeds = function () {
        return $interval(getFeeds, 60000);
    };

    //a bit more than 30 seconds as otherwise cache will be accessed due to cache control header
    var refreshBreakingNews = function() {
        return $interval(getBreakingNews, 40000  );
    };
       
    getFeeds();
    getBreakingNews();
    refreshFeeds();
    refreshBreakingNews();
};

angular.module("rss")
    .controller("rssFeedController", rssFeedController);
    

