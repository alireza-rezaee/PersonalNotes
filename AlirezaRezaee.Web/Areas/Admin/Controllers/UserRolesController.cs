using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rezaee.Alireza.Web.Areas.Admin.Models.ViewModels.UserRoles;
using Rezaee.Alireza.Web.Areas.Identity.Data;
using Rezaee.Alireza.Web.Helpers;

namespace Rezaee.Alireza.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/user-roles")]
    [Authorize]
    public class UserRolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRolesController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Route("")]
        [Authorize(Roles = Roles.UserRolesList)]
        public async Task<IActionResult> Index(string username, string rolename)
        {
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(rolename))
                return NotFound();
            else if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(rolename))
                throw new ArgumentException(message: "Only one \"username\" or \"rolename\" is acceptable at the same time.");
            else if (!string.IsNullOrEmpty(username))
            {
                //Came from UsersController
                ViewData["username"] = username;

                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                    return NotFound($"کاربر با شناسه '{username}' یافت نشد.");

                return View(new IndexVM()
                {
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }
            else //if (!string.IsNullOrEmpty(rolename))
            {
                //Came from RolesController
                ViewData["rolename"] = rolename;

                return View(new IndexVM()
                {
                    Users = await _userManager.GetUsersInRoleAsync(rolename)
                });
            }
        }

        [HttpPost("remove-user-from-role")]
        [Authorize(Roles = Roles.UserRoleAssign)]
        public async Task<IActionResult> RemoveUserFromRole(string username, string rolename)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(rolename))
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                    return NotFound($"کاربر با شناسه '{username}' یافت نشد.");

                await _userManager.RemoveFromRoleAsync(user, rolename);
                return View();
            }
            else
                return NotFound();
        }

        [HttpPost("add-user-to-role")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.UserRoleUnassign)]
        public async Task<IActionResult> AddUserToRole(string username, string[] roles)
        {
            if (string.IsNullOrEmpty(username))
                return NotFound();

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound($"کاربر با شناسه '{username}' یافت نشد.");

            //We don't have link in Users page for external users , but for avoid manually url changes
            if (user.PasswordHash == null)
                return NotFound($"کاربر با ورود خارجی قابلیت داشتن نقش را ندارد.");

            await _userManager.AddToRolesAsync(user, roles);
            return RedirectToAction(nameof(Index), new { username });
        }
    }
}
