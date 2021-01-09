using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "رایانامه")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "طول {0} نباید از حد مجاز (حداقل {2} و حداکثر {1} کاراکتر) فراتر رود.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "گذرواژه")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تکرار گذرواژه")]
            [Compare("Password", ErrorMessage = "گذرواژه و تکرار آن دارای مطابقت نیستند.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("کد بازیابی گذرواژه باید تامین شود.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
