using NUnit.Framework;

namespace GiveMeSportFeedTests
{
    [TestFixture]
    public class RssFeedApiUnitTests
    {
        [Test]
        public void GetFeed_WhenNoData_ReturnNotFoundAndEmptyItemDtoList()
        {
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
