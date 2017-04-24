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
    public class UseETag : ActionFilterAttribute
    {
        private readonly string _quotes = "\"";

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            if (context.Request.Method != HttpMethod.Get) return;

            HttpHeaderValueCollection<EntityTagHeaderValue> clientETags = context.Request.Headers.IfNoneMatch;

            if (clientETags == null || !clientETags.Any()) return;

            var responseContent = context.Response.Content as ObjectContent;
            var dtos = responseContent?.Value as List<ItemDto>;
            var guids = dtos?.Select(dto => dto.Guid);

            if (responseContent == null || dtos == null || !dtos.Any()) return;
            //\"-1958282966\""
            var serverETag = HashETag(guids);

            AddCacheControl(context, serverETag, clientETags);
        }

        private static void AddCacheControl(HttpActionExecutedContext context, string serverETag,
            HttpHeaderValueCollection<EntityTagHeaderValue> clientETags)
        {
            context.Response.Headers.ETag = new EntityTagHeaderValue(serverETag);
            context.Response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = new TimeSpan(0, 5, 0),
                Public = true
            };

            if (clientETags.First(entityTag => entityTag.Tag != serverETag) != null) return;

            context.Response.StatusCode = HttpStatusCode.NotModified;
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