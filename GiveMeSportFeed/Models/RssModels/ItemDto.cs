using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiveMeSportFeed.Models
{
    public class ItemDto
    {
        public string Guid { get; set; }
        public bool IsPermaLink { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Comments { get; set; }
    }
}