using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models.ViewModels.Recommendeds
{
    public class RecommendedPostsViewModel
    {
        public int Id { get; set; }

        public int Rank { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }
    }
}
