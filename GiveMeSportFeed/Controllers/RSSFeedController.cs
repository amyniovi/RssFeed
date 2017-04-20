using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Xml.XmlConfiguration;
using GiveMeSportFeed.Models;
using GiveMeSportFeed.RssApi.Interfaces;
using GiveMeSportFeed.RssApi.Services;

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
}

