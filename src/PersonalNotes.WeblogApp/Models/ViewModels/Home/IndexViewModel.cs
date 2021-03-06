﻿using AlirezaRezaee.PersonalNotes.WeblogApp.Models.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models.ViewModels.Home
{
    public class IndexViewModel
    {
        public string AboutAuthorSummary { get; set; }

        public string QuranAyah { get; set; }

        public IEnumerable<PostSummaryViewModel> Posts { get; set; }

        public List<Link> Links { get; set; }

        public List<Block> Blocks { get; set; }
    }
}
