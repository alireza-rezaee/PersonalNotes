using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<AuthorNoteModel> Notes { get; set; }

        public string AboutAuthorSummary { get; set; }

        public string QuranAyah { get; set; }
    }
}
