using AlirezaRezaee.PersonalNotes.WeblogApp.Helpers.Enums;
using AlirezaRezaee.PersonalNotes.WeblogApp.Models.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models.ViewModels.Tags
{
    public class PostSummaryViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime PublishDateTime { get; set; }

        public DateTime? LatestUpdateDateTime { get; set; }

        public PostType? Type { get; set; }

        public string PostUrl { get; set; }
    }
}
