using AlirezaRezaee.Web.Models.ViewModels.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models.ViewModels.Home
{
    public class IndexViewModel
    {
        public string AboutAuthorSummary { get; set; }

        public string QuranAyah { get; set; }

        public List<IllustratedLinkViewModel> IllustratedLinks { get; set; }
    }
}
