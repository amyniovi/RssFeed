var rssService = function ($http) {

    var getFeeds = function () {

        return $http.get("http://localhost:64559/rssfeed/?numberOfFeeds=10",
               {
                   etagCache: "persistentCache"
               })
               .then(function (data, itemCache) {
                   itemCache.set(data);
                   return data;
               },
                   function (data, itemCache) {
                       if (data.status !== 304)
                           return "Internal Server Error";
                       return itemCache.get();
                   })
               .ifCached(function (data, itemCache) {
                   var cachedFeed = itemCache.get();
                   return cachedFeed;
               });

        //  return $interval(callRssService, 3000);

    };
  
    var getBreakingNews = function () {

        return $http.get("http://localhost:64559/rssfeed/?minutes=5",
           {
               etagCache: "persistentCache"
           })
           .then(function (data, itemCache) {
               itemCache.set(data);
               return data;
           },
               function (data, itemCache) {
                   if (data.status !== 304)
                       return "Internal Server Error";
                   return itemCache.get();
               })
           .ifCached(function (data, itemCache) {
               var cachedFeed = itemCache.get();
               return cachedFeed;
           });
    };

    return {
        getFeeds: getFeeds,
        getBreakingNews: getBreakingNews
    };
};

var module = angular.module("rss");
module.factory("rssService", rssService);

