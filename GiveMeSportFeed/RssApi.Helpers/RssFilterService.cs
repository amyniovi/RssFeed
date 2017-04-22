using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using GiveMeSportFeed.Models;
using GiveMeSportFeed.Models.RssModels;
using WebGrease.Css.Extensions;

namespace GiveMeSportFeed.RssApi.Helpers
{
    public class RssFilterService : IRssFilterService
    {
        public IEnumerable<ItemDto> Filter10Latest( IEnumerable<ItemDto> allItems)
        {
            return allItems
                .OrderByDescending(item => item.PublishedDate)
                .Take(10);
        }

        public IEnumerable<ItemDto> ConvertToDtos(IEnumerable<SyndicationItem> syndicationItems)
        {
            return syndicationItems.Select(ItemDtoFactory.Create);
        }
    }
}