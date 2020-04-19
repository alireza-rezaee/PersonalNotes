using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Web.Models.ViewModels
{
    public class UrlCreateEditViewModel
    {
        [Required]
        public string Url { get; set; }

        public bool IsCustomShortLink { get; set; }

        public string CustomShortLink { get; set; }

        [Required]
        public string Recaptcha { get; set; }
    }
}
