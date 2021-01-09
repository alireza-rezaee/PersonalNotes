using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Helpers;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Pages.Account.Manage
{
    public class Disable2faModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<Disable2faModel> _logger;

        public Disable2faModel(
            UserManager<ApplicationUser> userManager,
            ILogger<Disable2faModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(Describer.UnableToLoadUser(_userManager.GetUserId(User), Language.English));
            }

            if (!await _userManager.GetTwoFactorEnabledAsync(user))
            {
                throw new InvalidOperationException($"غیرفعال سازی احراز هویت دو مرحله ای برای کاربر یا شناسه '{_userManager.GetUserId(User)}' ممکن نبود. چرا که اینک احراز هویت دو مرحله ای برای ایشان فعال نیست.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(Describer.UnableToLoadUser(_userManager.GetUserId(User), Language.English));
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new InvalidOperationException($"مواجهه با خطای پیش بینی نشده در حین غیرفعال سازی احراز هویت دو مرحله ای برای کاربر با شناسه '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation($"احراز هویت دو مرحله ای برای کاربر با شناسه '{_userManager.GetUserId(User)}' غیر فعال است.");
            StatusMessage = "احراز هویت دومرحله ای غیرفعال شد. شما میتوانید با تنظیم مجدد اقدام به فعال سازی دوباره کنید.";
            return RedirectToPage("./TwoFactorAuthentication");
        }
    }
}