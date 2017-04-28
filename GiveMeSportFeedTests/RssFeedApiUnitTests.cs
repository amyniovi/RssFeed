using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication; 
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using GiveMeSportFeed.Controllers;
using GiveMeSportFeed.Models;
using GiveMeSportFeed.RssApi.Attributes;
using GiveMeSportFeed.RssApi.Interfaces;
using GiveMeSportFeed.RssApi.Services;
using GiveMeSportFeed.RssApi.Helpers;
using NUnit.Framework;

namespace GiveMeSportFeedTests
{
    /// <summary>
    /// I chose to Unit Test the controller using a Fake Rss Service
    /// Alternatively we could selfhost the existing service to test it by invoking it.
    /// </summary>
    [TestFixture]
    public class RssFeedApiUnitTests
    {
        private readonly List<SyndicationItem> _sampleData = new List<SyndicationItem>();
        private DateTime _now = DateTime.Now;
        private readonly IRssFilterService _rssFilterService = new RssFilterService();

        [SetUp]
        public void SetUp()
        {
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddHours(-1), Id = "http://www.givemesport.com/1036207-kurt-zouma-produced-an-epic-reaction-to-nemanja-matics-screamer-vs-tottenham-hotspur?autoplay=on" });
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddDays(-1), Id = "http://www.givemesport.com/1036052-eight-guards-were-picked-before-kyle-lowry-in-the-2006-nba-draft-where-are-they-now?autoplay=on" });
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddMilliseconds(-900), Id = "http://www.givemesport.com/1036027-why-john-cena-defeated-aj-styles-for-the-wwe-title-at-the-royal-rumble?autoplay=on" });
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddSeconds(-2), Id = "http://www.givemesport.com/1036021-football-manager-gamer-simulates-100-seasons-and-the-results-are-incredible?autoplay=on" });
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddDays(-7), Id = "http://www.givemesport.com/1036011-branislav-ivanovic-makes-history-in-zenit-saint-petersburg-20-fc-ural?autoplay=on" });
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddHours(-2), Id = "http://www.givemesport.com/1035993-adrien-broner-makes-a-fool-of-himself-while-getting-arrested-in-connection-to-shooting?autoplay=on" });
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddHours(-2), Id = "http://www.givemesport.com/1035982-how-magic-johnson-may-have-tampered-with-indiana-pacers-star-paul-george?autoplay=on" });
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddHours(-3), Id = "http://www.givemesport.com/1035966-wwe-scrapped-a-huge-wrestlemania-33-plan-featuring-the-miz?autoplay=on" });
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddHours(-9), Id = "http://www.givemesport.com/1035954-carl-froch-predicts-the-outcome-of-conor-mcgregor-versus-floyd-mayweather?autoplay=on" });
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddMinutes(-25), Id = "http://www.givemesport.com/1035946-jerry-west-thinks-russell-westbrook-is-better-than-michael-jordan-in-one-key-aspect-of-his-game?autoplay=on" });
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddMinutes(-20), Id = "http://www.givemesport.com/1035909-why-finn-balor-wont-reunite-with-luke-gallows-and-karl-anderson-on-raw?autoplay=on" });
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddMinutes(-15), Id = "http://www.givemesport.com/1035906-cm-punk-comments-on-why-he-went-to-the-wwe?autoplay=on" });
        }

        //This test uses the actual RssService
        [Test]
        public void GetFeed_WhenWrongAddress_ReturnErrorResponse()
        {
            var service = new RssService() { ServiceUri = "http://www.givemeort.com/rss.ashx" };
            var controller = new RssFeedController(service, _rssFilterService);

            var result = controller.Get().Result;

            Assert.IsInstanceOf(typeof(InternalServerErrorResult), result);
        }

        [Test]
        public void GetFeed_whenNullOrZeroItems_ReturnNotFound()
        {
            var fakeRssService = new FakeRssService(new List<SyndicationItem>());
            var controller = new RssFeedController(fakeRssService, _rssFilterService);

            var result = controller.Get().Result;

            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public void GetFeed_WhenSuccessStatus_ShouldReturn10ItemDtos()
        {
            var fakeRssService = new FakeRssService(_sampleData);
            var controller = new RssFeedController(fakeRssService, _rssFilterService);

            var result = controller.Get().Result as OkNegotiatedContentResult<List<ItemDto>>;

            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<List<ItemDto>>), result);
            Assert.That(result?.Content.Count == 10);
        }

        [Test]
        public void GetFeed_WhenSuccessStatus_ItemsOrderedByPublishedDate()
        {
            var fakeRssService = new FakeRssService(_sampleData);
            var controller = new RssFeedController(fakeRssService, _rssFilterService);

            var result = controller.Get().Result as OkNegotiatedContentResult<List<ItemDto>>;

            Assert.That(result?.Content.ToList(), Is.Ordered.Descending.By("PublishedDate"));
        }

        [Test]
        public void GetFeed_CheckServiceCallIsMadeOnce()
        {
            var fakeRssService = new FakeRssService(_sampleData);
            var controller = new RssFeedController(fakeRssService, _rssFilterService);

            var result = controller.Get().Result as OkNegotiatedContentResult<List<ItemDto>>;

            Assert.That(fakeRssService.Counter == 1);
        }

        [Test]
        public void GetFeed_SameGuidsandETagMatch_ReturnNotModifiedStatus()
        {
            var fakeRssService = new FakeRssService(_sampleData);
            var rssFeedController = new RssFeedController(fakeRssService, _rssFilterService);

            var result = rssFeedController.Get().Result as OkNegotiatedContentResult<List<ItemDto>>;

            var response = new HttpResponseMessage
            {
                Content = new ObjectContent(typeof(List<ItemDto>), result?.Content, new JsonMediaTypeFormatter())
            };
            EntityTagHeaderValue etag = GetTestDataEtag(_sampleData);
            var executedContext = CreateHttpActionExecutionContextWrapper(rssFeedController, etag, response);

            var filter = new UseETag();
            filter.OnActionExecuted(executedContext);

            var postEtagresponse = executedContext.Response;
            Assert.That(postEtagresponse.StatusCode == HttpStatusCode.NotModified);
            Assert.That(postEtagresponse.Headers.ETag.ToString() == etag.ToString());
        }

        [Test]
        public void GetFeed_EtagMisMatch_ReturnOkStatus()
        {
            var fakeRssService = new FakeRssService(_sampleData);
            var rssFeedController = new RssFeedController(fakeRssService, _rssFilterService);
            //we get the 10 latest of the _sampleData
            var result = rssFeedController.Get().Result as OkNegotiatedContentResult<List<ItemDto>>;

            var response = new HttpResponseMessage
            {
                Content = new ObjectContent(typeof(List<ItemDto>), result?.Content, new JsonMediaTypeFormatter())
            };
            //we alter the _sampleData to make sure Etag doesnt match
            var orderedSampleData = _sampleData.OrderByDescending(x => x.PublishDate).ToList();
            orderedSampleData.RemoveAt(0);

            EntityTagHeaderValue etag = GetTestDataEtag(orderedSampleData);
            var executedContext = CreateHttpActionExecutionContextWrapper(rssFeedController, etag, response);

            var filter = new UseETag();
            filter.OnActionExecuted(executedContext);

            var postEtagresponse = executedContext.Response;
            Assert.That(postEtagresponse.StatusCode == HttpStatusCode.OK);
            Assert.That(postEtagresponse.Headers.ETag.ToString() != etag.ToString());
        }

        private static HttpActionExecutedContext CreateHttpActionExecutionContextWrapper(RssFeedController rssFeedController,
            EntityTagHeaderValue etag, HttpResponseMessage response)
        {
            rssFeedController.Request = new HttpRequestMessage();
            rssFeedController.Request.Headers.IfNoneMatch.Add(etag);

            var httpConfiguration = new HttpConfiguration(new HttpRouteCollection("~/App_Start/RouteConfig"));
            rssFeedController.Configuration = httpConfiguration;

            var httpcontext = new HttpActionContext()
            {
                ControllerContext = new HttpControllerContext
                {
                    Request = rssFeedController.Request,
                    Controller = rssFeedController,
                    Configuration = rssFeedController.Configuration,
                    RequestContext = rssFeedController.RequestContext
                }
            };
            var executedContext = new HttpActionExecutedContext()
            {
                ActionContext = httpcontext,
                Response = response
            };
            return executedContext;
        }

        private EntityTagHeaderValue GetTestDataEtag(List<SyndicationItem> sampleData)
        {
            var allDtos = _rssFilterService.ConvertToDtos(sampleData).ToList();
            var filteredDtos = _rssFilterService.FilterLatestByNumber(allDtos, 10).ToList();
            var etag = new EntityTagHeaderValue(new UseETag()
                .HashETag(filteredDtos.Select(x => x.Guid)));
            return etag;
        }

        internal class FakeRssService : IRssService
        {
            private readonly List<SyndicationItem> _syndicationitems;
            public int Counter { get; private set; }

            public FakeRssService(List<SyndicationItem> syndicationitems)
            {
                _syndicationitems = syndicationitems;
            }

            public Task<IEnumerable<SyndicationItem>> GetAllRssItems()
            {
                Counter++;
                return Task.FromResult(_syndicationitems as IEnumerable<SyndicationItem>);
            }
        }

    }
}
