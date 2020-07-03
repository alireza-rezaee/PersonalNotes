using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.ViewModels.Pages
{
    public class IndexViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public string ImageCoverPath { get; set; }

        public string Url { get; set; }
    }
}
