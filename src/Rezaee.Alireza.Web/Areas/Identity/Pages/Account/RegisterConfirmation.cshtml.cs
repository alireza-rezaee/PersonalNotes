using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Data;
using AlirezaRezaee.PersonalNotes.WeblogApp.Services.Email;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {

        public void OnGet()
        {
            
        }
    }
}
