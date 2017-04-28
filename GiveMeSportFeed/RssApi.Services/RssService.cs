using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using GiveMeSportFeed.RssApi.Interfaces;

namespace GiveMeSportFeed.RssApi.Services
{
    public class RssService : IRssService
    {
        public string ServiceUri { get; set; } = "http://www.givemesport.com/rss.ashx";

        public async Task<IEnumerable<SyndicationItem>> GetAllRssItems()
        {
            using (var httpClient = new HttpClient())
            {
                var stream = await httpClient.GetStreamAsync(new Uri(ServiceUri));

                IEnumerable<SyndicationItem> syndicationItems;
                using (var xmlReader = XmlReader.Create(stream))
                {
                    var theFeed = SyndicationFeed.Load(xmlReader);
                    syndicationItems = theFeed?.Items;
                }

                return syndicationItems;
            }
        }
    }
}