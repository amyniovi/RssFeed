using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web.Http;
using GiveMeSportFeed.Models;
using GiveMeSportFeed.RssApi.Attributes;
using GiveMeSportFeed.RssApi.Interfaces;
using GiveMeSportFeed.RssApi.Services;
using GiveMeSportFeed.RssApi.Helpers;

namespace GiveMeSportFeed.Controllers
{
    /// <summary>
    /// This API returns filtered and ordered feed items to the angular client in the App folder, 
    /// making sure caching is used on the client if request is within 1 minute
    /// and also it sends a "not modified" 304 status if the collection of feeds is not modified 
    /// which is reflected on the client and server with the matching ETag.
    /// Assumptions: 
    /// 1. We order based on Published Date, not LastUpdatedDate 
    /// 2. Guids are unique and change when the feeds are updated, hence they define uniqueness and could be used for ETag generation
    /// 3. I have not used MVC in this solution but could easily tweak this project to add it. 
    /// I instead chose to use angular for refreshing the feeds (without refreshing the browser) 
    /// and hence simply chose to display the data with angular. If MVC is required i can re-submit.
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
        public async Task<IHttpActionResult> Get(int numberOfFeeds = 10, int minutes = 0)
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

            if (minutes > 0 && minutes < 15)
                dtos = _rssFilterService.FilterLatestByTime(dtos, minutes).ToList();
            //here we assumming there would not be more than 10 Breaking news
            return Ok(_rssFilterService.FilterLatestByNumber(dtos, numberOfFeeds).ToList());
        }
       
    }
}

