using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

namespace Rezaee.Alireza.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("", Name = "SiteIndex")]
        public async Task<IActionResult> Index()
        {
            ViewData["FullName"] = _context.Options.First(i => i.OptionName == "FullName").OptionValue;
            ViewData["Title"] = _context.Options.First(i => i.OptionName == "IndexTitle").OptionValue;

            return View(
                new Models.ViewModels.Home.IndexViewModel()
                {
                    QuranAyah = _context.Options.First(i => i.OptionName == "QuranAyah").OptionValue,
                    AboutAuthorSummary = _context.Options.First(i => i.OptionName == "AboutAuthorSummary").OptionValue,
                    Posts = await PostsController.RetrieveLatestPostsSummary(count: 8, context: _context),
                    Links = await _context.Links.OrderBy(link => link.IsExpanded).ThenBy(link => link.Id).ToListAsync(),
                    Blocks = await BlocksController.GetBlocks(context: _context)
                });
        }

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
            _logger.LogTrace("Hello Trace (0)");
            _logger.LogDebug("hello Debug (1)");
            _logger.LogInformation("Hello Information (2)");
            _logger.LogWarning("Hello Warning (3)");
            _logger.LogError("Hello Error (4)");
            _logger.LogCritical("Hello Critical (5)");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}