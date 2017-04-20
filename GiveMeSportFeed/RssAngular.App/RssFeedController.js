var app = angular.module("rss", []);


function rssFeedControler($scope) {

    $scope.model = "sports feed";

 };

 app.controller("rssFeedControler", rssFeedControler);