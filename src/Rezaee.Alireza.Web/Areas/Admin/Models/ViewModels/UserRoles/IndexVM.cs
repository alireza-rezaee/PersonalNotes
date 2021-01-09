using AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Admin.Models.ViewModels.UserRoles
{
    public class IndexVM
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
