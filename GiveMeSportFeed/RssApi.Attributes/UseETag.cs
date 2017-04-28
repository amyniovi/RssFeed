using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Filters;
using GiveMeSportFeed.Models;
using WebGrease.Css.Extensions;

namespace GiveMeSportFeed.RssApi.Attributes
{
    /// <summary>
    /// This attribute is responsible for intercepting the httpresponse of the controllers that are marked with it to check for matching ETag.
    /// If the Etag of the request matches that of the response, null content with a 304 status is returned. 
    /// if the Etag doesnt match, then the response content and status are left as is but with the headers changed : 
    /// caching headers and the server etag header  are added.
    /// </summary>
    public class UseETag : ActionFilterAttribute
    {
        private readonly string _quotes = "\"";

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            if (context.Request.Method != HttpMethod.Get) return;

            HttpHeaderValueCollection<EntityTagHeaderValue> clientETags = context.Request.Headers.IfNoneMatch;

            var responseContent = context.Response.Content as ObjectContent;
            var dtos = responseContent?.Value as List<ItemDto>;
            var guids = dtos?.Select(dto => dto.Guid);

            if (responseContent == null || dtos == null || !dtos.Any()) return;
          
            var serverETag = HashETag(guids);

            AddCacheControl(context, serverETag, clientETags);
        }

        private static void AddCacheControl(HttpActionExecutedContext context, string serverETag,
            HttpHeaderValueCollection<EntityTagHeaderValue> clientETags)
        {
            context.Response.Headers.ETag = new EntityTagHeaderValue(serverETag);
            context.Response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = new TimeSpan(0, 0, 30),
                Public = true
            };

            if (clientETags == null || !clientETags.Any()) return;

            if (clientETags.FirstOrDefault(entityTag => entityTag.ToString() != serverETag) != null) return;

            context.Response.StatusCode = HttpStatusCode.NotModified;
            context.Response.Content = null;
        }

        public string HashETag(IEnumerable<string> guids)
        {
            const int prime = 31;
            var result = 1;
            guids.ForEach(guid => result = result * prime + guid.GetHashCode());
            return _quotes + result.ToString() + _quotes;
        }
    }
}