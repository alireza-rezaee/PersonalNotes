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

        [Route("/about")]
        public IActionResult About()
        {
            ViewData["FullName"] = _context.Options.First(i => i.OptionName == "FullName").OptionValue;
            ViewData["Title"] = _context.Options.First(i => i.OptionName == "IndexTitle").OptionValue;

            return View(
                new Models.ViewModels.Home.AboutViewModel()
                {
                    AboutContent = _context.Options.First(i => i.OptionName == "AboutAuthor").OptionValue
                });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private PostType? DetectPostType(Post post)
        {
            if (post == null)
                return null;

            if ((post.Article == null && post.Share == null && post.Markdown == null) || (post.Article != null && post.Share != null && post.Markdown != null))
                return null;

            if (post.Article != null)
                return PostType.Article;
            else if (post.Share != null)
                return PostType.Share;
            else //if (post.Markdown != null)
                return PostType.Markdown;
        }
    }
}