using System.Collections.Generic;
using GiveMeSportFeed.Areas;
using GiveMeSportFeed.Models;
using NUnit.Framework;

namespace GiveMeSportFeedTests
{
    [TestFixture]
    public class RssFeedApiUnitTests
    {
        private RSSFeedController _controller = new RSSFeedController();
        [Test]
        public void GetFeed_WhenNoData_ReturnNotFoundAndEmptyItemDtoList()
        {
            var result = _controller.Get() as List<ItemDto>;

            Assert.That(result == new List<ItemDto>());
        }

        [Test]
        public void GetFeed_WhenNotSuccesful_ReturnEmptyItemDtoList()
        {
        }

        [Test]
        public void GetFeed_WhenSuccessStatus_ShouldReturn10ItemDtos()
        {
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

    }
}
