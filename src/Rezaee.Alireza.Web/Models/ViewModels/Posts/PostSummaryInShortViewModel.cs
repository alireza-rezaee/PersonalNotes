using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models.ViewModels.Posts
{
    public class PostSummaryInShortViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime PublishDateTime { get; set; }

        public DateTime? LatestUpdateDateTime { get; set; }

        public string PostUrl { get; set; }
    }
}
