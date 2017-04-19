using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;

namespace GiveMeSportFeed.Models.RssModels
{
    public class ItemDtoFactory
    {
        public ItemDto Create(SyndicationItem syndicationItem)
        {
            //implement
            return new ItemDto();
        }
    }
}