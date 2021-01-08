using Rezaee.Alireza.Web.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.ViewModels
{
    public class ToastViewModel
    {
        public ToastType Type { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }
    }
}
