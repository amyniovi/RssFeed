using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using GiveMeSportFeed.Controllers;
using GiveMeSportFeed.Models;
using GiveMeSportFeed.RssApi.Interfaces;
using GiveMeSportFeed.RssApi.Services;
using GiveMeSportFeed.RssApi.Helpers;
using NUnit.Framework;

namespace GiveMeSportFeedTests
{
    /// <summary>
    /// I chose to Unit Test the controller using a Fake Rss Service
    /// Alternatively we could selfhost the existing service and test that.
    /// </summary>
    [TestFixture]
    public class RssFeedApiUnitTests
    {
        private List<SyndicationItem> _sampleData = new List<SyndicationItem>();
        private DateTime _now = DateTime.Now;
        private IRssFilterService _rssFilterService = new RssFilterService();
        [SetUp]
        public void SetUp()
        {
            _sampleData.Add(new SyndicationItem() {PublishDate  = _now.AddHours(-1) });
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddDays(-1)});
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddMilliseconds(-900)});
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddSeconds(-2)});
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddDays(-7)});
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddHours(-2)});
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddHours(-2)});
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddHours(-3)});
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddHours(-9)});
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddMinutes(-25)});
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddMinutes(-20)});
            _sampleData.Add(new SyndicationItem() { PublishDate = _now.AddMinutes(-15)});

        }
       
        [Test]
        public void GetFeed_WhenWrongAddress_ReturnErrorResponse()
        {
            var service = new RssService() {ServiceUri = "http://www.givemeort.com/rss.ashx"};
            var controller = new RssFeedController(service,_rssFilterService);

            var result = controller.Get().Result;

            Assert.IsInstanceOf(typeof(InternalServerErrorResult), result);
        }

        [Test]
        public void GetFeed_whenNullOrZeroItems_ReturnNotFound()
        {
            var fakeRssService = new FakeRssService(new List<SyndicationItem>());
            var controller = new RssFeedController(fakeRssService,_rssFilterService );

            var result = controller.Get().Result;

            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public void GetFeed_WhenSuccessStatus_ShouldReturn10ItemDtos()
        {
            var fakeRssService = new FakeRssService(_sampleData);
            var controller = new RssFeedController(fakeRssService,_rssFilterService);

            var result = controller.Get().Result as OkNegotiatedContentResult<List<ItemDto>>;

            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<List<ItemDto>>), result);
            Assert.That(result.Content.Count==10);
        }

        [Test]
        public void GetFeed_WhenSuccessStatus_ItemsOrderedByPublishedDate()
        {
            var fakeRssService = new FakeRssService(_sampleData);
            var controller = new RssFeedController(fakeRssService, _rssFilterService);

            var result = controller.Get().Result as OkNegotiatedContentResult<List<ItemDto>>;

            Assert.That(result.Content.ToList(), Is.Ordered.Descending.By("PublishedDate"));
        }

        [Test]
        public void GetFeed_SameGuidsandETagMatch_ReturnNotModifiedStatus()
        {
        }

        [Test]
        public void GetFeed_AtLeastOneDiffGuid_ReturnListofItems()
        {
        }

        internal class FakeRssService : IRssService
        {
            private readonly List<SyndicationItem> _syndicationitems;

            public FakeRssService(List<SyndicationItem> syndicationitems)
            {
                _syndicationitems = syndicationitems;
            }

            public Task<IEnumerable<SyndicationItem>> GetAllRssItems()
            {
                return Task.FromResult(_syndicationitems as IEnumerable<SyndicationItem>);

            }
        }

    }
}
