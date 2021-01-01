using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Rezaee.Alireza.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rezaee.Alireza.Web.Extensions;
using System.Text;
using Microsoft.AspNetCore.Http;
using Rezaee.Alireza.Web.Helpers;
using Rezaee.Alireza.Web.Areas.Identity.Helpers;

namespace Rezaee.Alireza.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IFileManager _ifileManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IFileManager ifileManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ifileManager = ifileManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "شناسه کاربری")]
            public string Username { get; set; }

            [Phone]
            [Display(Name = "شماره تماس")]
            public string PhoneNumber { get; set; }

            [EmailAddress]
            [Display(Name = "رایانامه")]
            public string Email { get; set; }

            [StringLength(100)]
            [Display(Name = "نام")]
            public string FirstName { get; set; }

            [StringLength(100)]
            [Display(Name = "نام خانوادگی")]
            public string LastName { get; set; }

            [StringLength(100)]
            [Display(Name = "نام نمایشی")]
            public string DisplayName { get; set; }

            [StringLength(100)]
            [Display(Name = "محل زندگی")]
            public string LocationName { get; set; }

            [Display(Name = "زادروز")]
            public DateTime? BirthDate { get; set; }

            [StringLength(150)]
            public string ProfileImagePath { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Input = new InputModel
            {
                Username = await _userManager.GetUserNameAsync(user),
                PhoneNumber = (phoneNumber != null) ? phoneNumber.EnglishNumberToPersian() : "" ,
                Email = await _userManager.GetEmailAsync(user),
                DisplayName = user.DisplayName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImagePath = user.ProfileImagePath,
                BirthDate = user.BirthDate,
                LocationName = user.Location
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(Describer.UnableToLoadUser(_userManager.GetUserId(User), Language.English));
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            //Invalid UserID
            if (user == null)
                return NotFound(Describer.UnableToLoadUser(_userManager.GetUserId(User), Language.English));

            if (ModelState.IsValid)
            {
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (Input.PhoneNumber != phoneNumber)
                {
                    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber.PersianNumberToEnglish());
                    if (!setPhoneResult.Succeeded)
                    {
                        var userId = await _userManager.GetUserIdAsync(user);
                        throw new InvalidOperationException($"تنظیم شماره تماس برای کاربر '{userId}' با خطا مواجه شد..");
                    }
                }

                user.DisplayName = Input.DisplayName;
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.BirthDate = Input.BirthDate;
                user.Location = Input.LocationName;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"به روز رسانی نمایه برای کاربر '{userId}' با خطا مواجه شد..");
                }

                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "نمایه به روز شد.";
                return RedirectToPage();
            }

            //Invalid Model
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostSetProfileImageAsync(IFormFile image)
        {
            var user = await _userManager.GetUserAsync(User);
            //Invalid UserID
            if (user == null)
                return NotFound(Describer.UnableToLoadUser(_userManager.GetUserId(User), Language.English));

            if (image == null)
                return RedirectToPage();

            if (!string.IsNullOrEmpty(user.ProfileImagePath))
                _ifileManager.DeleteFile(user.ProfileImagePath);
            var avatarPath = $"uploads/avatars/{PersianDateTime.Now.ToString("yyyy/MM/dd/yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + new Random().Next(1000000, 9999999)}_{Web.Helpers.File.ValidateName(image.FileName)}";
            await _ifileManager.SaveFile(image, avatarPath);
            user.ProfileImagePath = $"/{avatarPath}";
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                throw new InvalidOperationException($"به روز رسانی تصویر نمایه برای کاربر '{userId}' با خطا مواجه شد..");
            }

            StatusMessage = "تصویر نمایه به روز شد.";
            return RedirectToPage();
        }
    }
}
