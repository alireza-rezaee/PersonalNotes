using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models.ViewModels.Links
{
    public class IndexViewModel
    {
        public List<IllustratedLinkViewModel> IllustratedLinks { get; set; }

        public List<PlainLinkViewModel> PlainLinks { get; set; }
    }
}
