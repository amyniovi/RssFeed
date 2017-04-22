using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using GiveMeSportFeed.Models;

namespace GiveMeSportFeed.RssApi.Interfaces
{
    public interface IRssService
    {
        Task<IEnumerable<SyndicationItem>> GetAllRssItems();
    }
}