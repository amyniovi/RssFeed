using System.Collections.Generic;
using System.Threading.Tasks;
using GiveMeSportFeed.Models;

namespace GiveMeSportFeed.RssApi.Interfaces
{
    public interface IRssService
    {
        Task<IEnumerable<ItemDto>> GetRssItems();
    }
}