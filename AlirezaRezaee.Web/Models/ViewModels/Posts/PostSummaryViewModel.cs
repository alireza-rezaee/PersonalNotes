using AlirezaRezaee.Web.Helpers.Enums;
using AlirezaRezaee.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models.ViewModels.Posts
{
    public class PostSummaryViewModel
    {
        public string Title { get; set; }

        public string Summary { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime PublishDateTime { get; set; }

        public DateTime? LatestUpdateDateTime { get; set; }

        public PostType Type { get; set; }

        public string PostUrl { get; set; }
    }
}
