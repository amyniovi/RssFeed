using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Xml;
using System.Xml.XmlConfiguration;
using GiveMeSportFeed.Models;
using GiveMeSportFeed.Models.RssModels;
using WebGrease.Css.Extensions;

namespace GiveMeSportFeed.Controllers
{
    /// <summary>
    /// This API doesnt need to accurately inform the user about Errors, hence only using InternalServerError, and NotFound for now.
    /// </summary>
   
    public class RssFeedController : ApiController
    {
        private readonly IRssService _rssService;

        public RssFeedController()
        {
            _rssService = new RssService();
        }

        public RssFeedController(IRssService rssService)
        {
            _rssService = rssService;
        }

        //localhost:PORT/RSSFeed
        public async Task<IHttpActionResult> Get()
        {
            List<ItemDto> dtos;
            try
            {
                dtos = (await _rssService.GetRssItems()).ToList();
            }
            catch
            {
                return InternalServerError();
            }

            if (!dtos.Any())
                return NotFound();

            return Ok(dtos);
        }
    }

    public interface IRssService
    {
        Task<IEnumerable<ItemDto>> GetRssItems();
    }

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

