using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using GiveMeSportFeed.Models;
using GiveMeSportFeed.Models.RssModels;
using GiveMeSportFeed.RssApi.Interfaces;
using WebGrease.Css.Extensions;

namespace GiveMeSportFeed.RssApi.Services
{
    public class RssService : IRssService
    {
        public string ServiceUri { get; set; } = "http://www.givemesport.com/rss.ashx";

        public async Task<IEnumerable<ItemDto>> GetRssItems()
        {
            List<ItemDto> dtoList;
            using (var httpClient = new HttpClient())
            {
                var stream = await httpClient.GetStreamAsync(new Uri(ServiceUri));
                dtoList = ParseRssStream(stream).ToList();
            }

            return dtoList;
        }

        public IEnumerable<ItemDto> ParseRssStream(Stream stream)
        {
            var itemDtos = new List<ItemDto>();
            using (var xmlReader = XmlReader.Create(stream))
            {
                SyndicationFeed theFeed = SyndicationFeed.Load(xmlReader);
                if (theFeed == null || !theFeed.Items.Any())
                {
                    return new List<ItemDto>();
                }

                theFeed.Items.ForEach(syndItem => itemDtos.Add(ItemDtoFactory.Create(syndItem)));
            }
            return itemDtos;
        }
    }
}