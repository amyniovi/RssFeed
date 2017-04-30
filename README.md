# README #

Instructions for RssFeed

### What is this repository for? ###

This repo is a simple Angular app invoking a Web Api which in turn invokes the GiveMeSport RssFeed Service to get the Rss data. There is a separate Unit Tests project , please run the unit tests.

### How do I get set up? ###

Just set the GiveMeSportFeed as your start up project and run the app
thats all!!! you dont need to navigate to a URL just use localhost and port. 

You will see the Rss Feeds displayed (with a bit of a delay at first due to the GiveMeSport service having to return the feeds)
The Breaking News section is also on the page, but if there are no feeds published within the last 5 mins you will unfortunately not see them on the page.

All the dependencies are the nuget packages that are going to be restored once you run
and also the Bundles do all the work to include the scripts needed. So nothing extra to be done on your side.


### Contribution guidelines ###



### Who do I talk to? ###