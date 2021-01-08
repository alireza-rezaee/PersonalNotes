using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rezaee.Alireza.Web.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Rezaee.Alireza.Web.Models.ViewModels.Posts;
using Rezaee.Alireza.Web.Helpers.Enums;
using Microsoft.AspNetCore.Authorization;
using Rezaee.Alireza.Web.Attributes;
using Rezaee.Alireza.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Rezaee.Alireza.Web.Extensions;

namespace Rezaee.Alireza.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("", Name = "SiteIndex")]
        public async Task<IActionResult> Index()
        {
            var personalizations = await _context.Personalizations.Where(item => (new string[] { "IndexTitle", "IndexDescription", "SiteCoverSrc" }).Contains(item.Title)).ToListAsync();

            ViewData["Title"] = personalizations.FirstOrDefault(i => i.Title == "IndexTitle").Value;
            ViewData["Description"] = personalizations.FirstOrDefault(i => i.Title == "IndexDescription").Value;
            ViewData["Url"] = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action(nameof(Index), nameof(HomeController).ControllerName())}";
            ViewData["Image"] = new Uri(
                baseUri: new Uri($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}"),
                relativeUri: personalizations.FirstOrDefault(i => i.Title == "SiteCoverSrc").Value).ToString();

            return View(
                new Models.ViewModels.Home.IndexViewModel()
                {
                    Posts = await PostsController.RetrieveLatestPostsSummary(count: 8, context: _context),
                    Links = await _context.Links.OrderBy(link => link.IsExpanded).ThenBy(link => link.Id).ToListAsync(),
                    Blocks = await BlocksController.GetBlocks(context: _context)
                });
        }

        //TODO: Complete roles
        [Route("/management")]
        [AuthorizeRoles(
            //Posts Controller
            Roles.PostCreateArticle,
            Roles.PostEditArticle,
            Roles.PostDeleteArticle,
            Roles.PostCreateShare,
            Roles.PostEditShare,
            Roles.PostDeleteShare,
            Roles.PostCreateMarkdown,
            Roles.PostEditMarkdown,
            Roles.PostDeleteMarkdown,
            //Tags Controller
            Roles.TagCreate,
            Roles.TagDelete,
            Roles.TagEdit,
            //Messages Controller
            Roles.MessagesList,
            Roles.MessageDelete,
            Roles.MessagesSetReadOrNot,
            //Pages Controller
            Roles.PageCreate,
            Roles.PageEdit,
            Roles.PageDelete,
            //Links Controller
            Roles.LinkCreate,
            Roles.LinkEdit,
            Roles.LinkDelete,
            //Blocks Controller
            Roles.BlockCreate,
            //Users Controller
            Roles.UserCreate,
            Roles.UserEdit,
            Roles.UsersList,
            Roles.UserDetails,
            Roles.UserDelete,
            //Roles Controller
            Roles.RoleCreate,
            Roles.RoleEdit,
            Roles.RolesList,
            Roles.RoleDelete
            )]
        public IActionResult Management()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}