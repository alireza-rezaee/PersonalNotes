using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rezaee.Alireza.Web.Areas.Identity.Data;
using Rezaee.Alireza.Web.Helpers;

namespace Rezaee.Alireza.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/users")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileManager _ifileManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(UserManager<ApplicationUser> userManager, IFileManager ifileManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _ifileManager = ifileManager;
            _signInManager = signInManager;
        }

        [Route("")]
        [Authorize(Roles = Roles.UsersList)]
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }

        [Route("details")]
        [Authorize(Roles = Roles.UsersList)]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.UserDelete)]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound(" کاربری یافت نشد.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(" کاربر مورد نظر تنظیم یافت نشد.");

            //Delete User
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                //Delete User's Profile Image
                if (!user.ProfileImagePath.EndsWith("default.png"))
                    _ifileManager.DeleteFile(user.ProfileImagePath);

                //Check if current user is deleted user then signout him/her
                if (User.Identity.Name == user.UserName)
                    await _signInManager.SignOutAsync();
            }
            else
            {
                throw new InvalidOperationException($"خطای غیر منتظره در هنگام حذف کاربر با شناسه  '{user.Id}'.");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("search")]
        [Authorize(Roles = Roles.UsersList)]
        public IEnumerable<ApplicationUser> Search(string q)
        {
            return _userManager
                .Users
                .Where(u => u.FirstName.Contains(q) || u.LastName.Contains(q) || u.UserName.Contains(q))
                .Take(10)//Maximun records goes here
                .ToList();
        }
    }
}
