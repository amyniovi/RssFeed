using System.Collections.Generic;
using System.ServiceModel.Syndication;
using GiveMeSportFeed.Models;

namespace GiveMeSportFeed.RssApi.Interfaces
{
    public interface IRssFilterService
    {
        IEnumerable<ItemDto> FilterLatestByNumber(IEnumerable<ItemDto> allItems, int numberOfItems);
        IEnumerable<ItemDto> FilterLatestByTime(IEnumerable<ItemDto> allItems, int minutes);
        IEnumerable<ItemDto> ConvertToDtos(IEnumerable<SyndicationItem> syndicationItems);
    }
}