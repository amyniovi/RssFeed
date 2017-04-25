angular.module("rss", ["http-etag"])
.config(function(httpEtagProvider) {
        httpEtagProvider.defineCache("persistentCache",
        {
            cacheService: "localStorage"
        });
    });
