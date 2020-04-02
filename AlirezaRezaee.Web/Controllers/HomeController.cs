using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AlirezaRezaee.Web.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using AlirezaRezaee.Web.Data;
using AlirezaRezaee.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using AlirezaRezaee.Web.Models.ViewModels.Links;
using AlirezaRezaee.Web.Models.ViewModels.Posts;

namespace AlirezaRezaee.Web.Controllers
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

        public async Task<IActionResult> Index()
        {
            ViewData["FullName"] = _context.Options.First(i => i.OptionName == "FullName").OptionValue;
            ViewData["Title"] = _context.Options.First(i => i.OptionName == "IndexTitle").OptionValue;

            //<Posts>
            var articlePosts = await _context.Articles.Select(article => new PostSummaryViewModel
            {
                Id = article.ArticleId,
                Title = article.Title,
                Summmary = article.Summmary,
                PublishDateTime = article.PublishDateTime,
                LatestUpdateDateTime = article.LatestUpdateDateTime,
                ThumbnailUrl = article.ThumbnailUrl,
                Category = article.ArticleCategories.First().Category.Title
            }).OrderByDescending(article => article.LatestUpdateDateTime).ThenByDescending(article => article.PublishDateTime).Take(8).ToListAsync();

            var sharePosts = await _context.Shares.Select(share => new PostSummaryViewModel
            {
                Id = share.ShareId,
                Title = share.Title,
                Summmary = share.Summmary,
                PublishDateTime = share.PublishDateTime,
                LatestUpdateDateTime = share.LatestUpdateDateTime,
                ThumbnailUrl = share.ThumbnailUrl,
                Category = "بازنشر",
                PostUrl = share.Url
            }).OrderByDescending(share => share.LatestUpdateDateTime).ThenByDescending(share => share.PublishDateTime).Take(8).ToListAsync();

            articlePosts.AddRange(sharePosts);
            //</Posts>

            //<Links>
            var wholeLinks = _context.Links.OrderBy(link => link.Rank)
                .Select(list => new IllustratedLinkViewModel { Title = list.Title, ImagePath = list.ImagePath, Url = list.Url })
                .ToList();

            var numberOfPrimaryLinks = int.Parse(_context.Options.First(i => i.OptionName == "NumberOfPrimaryLinks").OptionValue);
            var illustratedLinks = wholeLinks.Where(link => !string.IsNullOrEmpty(link.ImagePath)).Take(numberOfPrimaryLinks).ToList();
            //</Links>


            return View(
                new Models.ViewModels.Home.IndexViewModel()
                {
                    QuranAyah = _context.Options.First(i => i.OptionName == "QuranAyah").OptionValue,
                    AboutAuthorSummary = _context.Options.First(i => i.OptionName == "AboutAuthorSummary").OptionValue,
                    Posts = articlePosts.OrderByDescending(post => post.LatestUpdateDateTime).ThenByDescending(post => post.PublishDateTime).Take(8).ToList(),
                    IllustratedLinks = illustratedLinks
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
    }
}