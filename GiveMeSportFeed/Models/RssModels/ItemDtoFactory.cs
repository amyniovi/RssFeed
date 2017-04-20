using System.Linq;
using System.ServiceModel.Syndication;

namespace GiveMeSportFeed.Models.RssModels
{/// <summary>
/// assumming isPermaLInk = true for all, 
/// assumming one Link per item
/// </summary>
    public static class ItemDtoFactory
    {
        public static ItemDto Create(SyndicationItem syndicationItem)
        {
            return new ItemDto()
            {
                Comments = syndicationItem.Summary.Text,
                Guid = syndicationItem.Id,
                IsPermaLink = true,
                Link = syndicationItem.Links.FirstOrDefault()?.ToString(),
                PublishedDate = syndicationItem.PublishDate.DateTime,
                Title = syndicationItem.Title.Text
            };
        }
    }
}