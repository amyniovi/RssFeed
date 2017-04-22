using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using GiveMeSportFeed.Models;

namespace GiveMeSportFeed.RssApi.Helpers
{
    public interface IRssFilterService
    {
        IEnumerable<ItemDto> Filter10Latest(IEnumerable<ItemDto> allItems);
        IEnumerable<ItemDto> ConvertToDtos(IEnumerable<SyndicationItem> syndicationItems);
    }
}