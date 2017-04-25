
    var rssFeedController = function ($scope, rssService) {

        var onsuccess = function(data) {
            $scope.model = data;
        };
        var onerror = function() {
            $scope.model = "error";
        };

        $scope.model = rssService.getFeeds().then(onsuccess, onerror);
    };

    angular.module("rss")
        .controller("rssFeedController", rssFeedController);
    

