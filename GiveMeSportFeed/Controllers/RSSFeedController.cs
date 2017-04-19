using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using System.Xml.XmlConfiguration;

namespace GiveMeSportFeed.Areas
{
    [RoutePrefix("RssFeed")]
    public class RSSFeedController : ApiController
    {   
        // GET: api/RSSFeed
        public IHttpActionResult Get()
        {
           return Ok();
        }
      
    }
}

