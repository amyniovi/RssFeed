///This is the client service. Local Storage is the persistent Cache
///This service is responsible for providing the controller with the data either from the server or from the persistent cache. 
///if Etags match localstorage is used
///if the request is made within half a minute (see UseEtag cache control), local storage is used without even sending a request to the server.

var rssService = function ($http) {

    var getFeeds = function() {

        return $http.get("http://localhost:64559/rssfeed/?numberOfFeeds=10",
            {
                etagCache: "persistentCache"
            })
            .then(function(data, itemCache) {
                    itemCache.set(data);
                    return data;
                },
                function(data, itemCache) {
                    if (data.status !== 304)
                        return "Internal Server Error";
                    return itemCache.get();
                });
    };

    var getBreakingNews = function() {

        return $http.get("http://localhost:64559/rssfeed/?minutes=5",
            {
                etagCache: "persistentCache"
            })
            .then(function(data, itemCache) {
                    itemCache.set(data);
                    return data;
                },
                function(data, itemCache) {
                    if (data.status !== 304)
                        return "Internal Server Error";
                    return itemCache.get();
                });
    };

    return {
        getFeeds: getFeeds,
        getBreakingNews: getBreakingNews
    };
};

 angular.module("rss").factory("rssService", rssService);

