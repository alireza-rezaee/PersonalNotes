using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models.ViewModels.Posts
{
    public class CreateEditViewModel
    {
        public bool IsArticle { get; set; }

        public Article Article { get; set; }

        public Share Share { get; set; }
    }
}
