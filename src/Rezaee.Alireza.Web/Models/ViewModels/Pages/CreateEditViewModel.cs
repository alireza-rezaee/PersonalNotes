using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models.ViewModels.Pages
{
    public class CreateEditViewModel
    {
        public Page Page { get; set; }

        public IFormFile CoverImage { get; set; }
    }
}
