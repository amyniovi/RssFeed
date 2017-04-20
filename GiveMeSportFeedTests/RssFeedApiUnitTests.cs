using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using GiveMeSportFeed.Controllers;
using GiveMeSportFeed.Models;
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
        [Test]
        public void GetFeed_WhenWrongAddress_ReturnErrorResponse()
        {
            var service = new RssService {ServiceUri = "http://www.givemeort.com/rss.ashx"};
            var controller = new RssFeedController(service);

            var result = controller.Get().Result;

            Assert.IsInstanceOf(typeof(InternalServerErrorResult), result);
        }

        [Test]
        public void GetFeed_whenNullOrZeroItems_ReturnNotFound()
        {
            var service = new FakeRssService(new List<ItemDto>());
            var controller = new RssFeedController(service);

            var result = controller.Get().Result;

            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public void GetFeed_WhenSuccessStatus_ShouldReturn10ItemDtos()
        {
           // var result = _controller.Get().Result;

        }

        [Test]
        public void GetFeed_WhenSuccessStatus_ItemsOrderedByPublishedDate()
        {
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
            private readonly List<ItemDto> _dtos;

            public FakeRssService(List<ItemDto> dtos)
            {
                _dtos = dtos;
            }

            public Task<IEnumerable<ItemDto>> GetRssItems()
            {
                return Task.FromResult((IEnumerable<ItemDto>) _dtos);

            }
        }

    }
}
