﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Helpers;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Pages.Account.Manage
{
    public class SetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SetPasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "طول مجاز {0} باید حداقل {2} کاراکتر و حداکثر {1} باشد.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "گذرواژه جدید")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تکرار گذرواژه جدید")]
            [Compare("NewPassword", ErrorMessage = "گذرواژه جدید و تکرار آن دارای مطابقت نیستند.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(Describer.UnableToLoadUser(_userManager.GetUserId(User), Language.English));
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToPage("./ChangePassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(Describer.UnableToLoadUser(_userManager.GetUserId(User), Language.English));
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "گذرواژه ثبت شد.";

            return RedirectToPage();
        }
    }
}
