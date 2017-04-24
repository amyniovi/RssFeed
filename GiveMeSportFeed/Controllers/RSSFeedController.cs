using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Xml.XmlConfiguration;
using GiveMeSportFeed.Models;
using GiveMeSportFeed.RssApi.Attributes;
using GiveMeSportFeed.RssApi.Interfaces;
using GiveMeSportFeed.RssApi.Services;
using GiveMeSportFeed.RssApi.Helpers;

namespace GiveMeSportFeed.Controllers
{
    /// <summary>
    /// This API doesnt need to accurately inform the user about Errors, hence only using InternalServerError, and NotFound for now.
    /// </summary>
    [UseETag]
    public class RssFeedController : ApiController
    {
        private readonly IRssService _rssService;
        private readonly IRssFilterService _rssFilterService;

        public RssFeedController()
        {
            _rssService = new RssService();
            _rssFilterService = new RssFilterService();
        }

        public RssFeedController(IRssService rssService, IRssFilterService rssFilterService)
        {
            _rssService = rssService;
            _rssFilterService = rssFilterService;
        }

        //localhost:PORT/RSSFeed
        public async Task<IHttpActionResult> Get()
        {
            List<SyndicationItem> items;
            List<ItemDto> dtos;
            try
            {
                items = (await _rssService.GetAllRssItems()).ToList();
                dtos = _rssFilterService.ConvertToDtos(items).ToList();
            }
            catch(Exception e)
            {
                return InternalServerError();
            }

            if (!dtos.Any())
                return NotFound();

            return Ok(_rssFilterService.Filter10Latest(dtos).ToList());
        }
    }
}

