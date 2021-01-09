using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Helpers;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (TempData["HasLoggedOut"] != null && (bool)TempData["HasLoggedOut"] == true)
                return Page();
            return NotFound();
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            TempData["HasLoggedOut"] = true;
            _logger.LogInformation($"User with ID '{_userManager.GetUserId(User)}' logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
