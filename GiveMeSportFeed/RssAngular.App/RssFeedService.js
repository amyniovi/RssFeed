

function RssFeedService() {

    var getUpdatedFeeds = function () {
        console.log("i work");
    }
    var getBreakingNews = function () {

    }

    return {
        getUpdatedFeeds: getUpdatedFeeds,
        getBreakingNews: getBreakingNews
    };
}

angular
.module("rss")
.service("RssFeedService", RssFeedService);