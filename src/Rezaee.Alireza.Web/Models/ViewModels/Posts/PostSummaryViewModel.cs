using AlirezaRezaee.PersonalNotes.WeblogApp.Helpers.Enums;
using AlirezaRezaee.PersonalNotes.WeblogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models.ViewModels.Posts
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

        public string PostEditUrl { get; set; }

        public string postDeleteUrl { get; set; }

        public string postEditTypeUrl { get; set; }

        public Pin Pin { get; set; }

        public Posterpins Posterpins { get; set; }

        public List<PostSummaryInShortViewModel> RelatedPosts { get; set; }
    }
}
