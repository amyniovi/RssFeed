using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using GiveMeSportFeed.Models;
using GiveMeSportFeed.Models.RssModels;
using GiveMeSportFeed.RssApi.Interfaces;
using WebGrease.Css.Extensions;

namespace GiveMeSportFeed.RssApi.Helpers
{
    public class RssFilterService : IRssFilterService
    {
        public IEnumerable<ItemDto> FilterLatestByNumber( IEnumerable<ItemDto> allItems, int numberOfFeeds)
        {
            return allItems
                .OrderByDescending(item => item.PublishedDate)
                .Take(numberOfFeeds);
        }

        public IEnumerable<ItemDto> FilterLatestByTime(IEnumerable<ItemDto> allItems, int minutes)
        {
            return allItems.Where(item => item.PublishedDate > DateTime.Now.AddMinutes(-5));
        }


        public IEnumerable<ItemDto> ConvertToDtos(IEnumerable<SyndicationItem> syndicationItems)
        {
            return syndicationItems.Select(ItemDtoFactory.Create);
        }
    }
}