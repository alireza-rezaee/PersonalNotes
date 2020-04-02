using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models.ViewModels.Shares
{
    public class CreateEditViewModel
    {
        public Share Share { get; set; }

        public IFormFile Thumbnail { get; set; }
    }
}
