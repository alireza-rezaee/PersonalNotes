using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginWith2faModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginWith2faModel> _logger;

        public LoginWith2faModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginWith2faModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "طول {0} نباید از حد مجاز (حداقل {2} و حداکثر {1} کاراکتر) فراتر رود.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "کد احراز هویت")]
            public string TwoFactorCode { get; set; }

            [Display(Name = "به خاطر سپردن دستگاه")]
            public bool RememberMachine { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"در بارگذاری احراز هویت کاربر خطایی پیش آمد.");
            }

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"در بارگذاری احراز هویت کاربر خطایی پیش آمد.");
            }

            var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation($"User with ID '{user.Id}' logged in with 2fa.");
                return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning($"User with ID '{user.Id}' account locked out.");
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning($"Invalid authenticator code entered for user with ID '{user.Id}'.");
                ModelState.AddModelError(string.Empty, "کد احراز هویت اشتباه است.");
                return Page();
            }
        }
    }
}
