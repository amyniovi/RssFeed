using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace GiveMeSportFeed.Controllers
{
    public class RssMockController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/MockData.xml");
            string response; 
            using (StreamReader reader = new StreamReader(File.Open(filePath, FileMode.Open, FileAccess.Read)))
            {
                response = reader.ReadToEnd();
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(response, Encoding.UTF8, "application/xml")
            };
        }
    }
}
