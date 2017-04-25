    var rssService = function ($http) {

        var getFeeds = function () {
            return $http.get("http://localhost:64559/rssfeed")
                .then(function (response) {
                    return response.data;
                });
        };
        var getBreakingNews = function () {

        };

        return {
            getFeeds: getFeeds,
            getBreakingNews: getBreakingNews
        };
    };

    var module = angular.module("rss");
    module.factory("rssService", rssService);

