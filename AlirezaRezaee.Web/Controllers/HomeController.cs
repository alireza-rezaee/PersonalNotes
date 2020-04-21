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
using Rezaee.Alireza.Web.Models.ViewModels.Links;
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

        public async Task<IActionResult> Index()
        {
            ViewData["FullName"] = _context.Options.First(i => i.OptionName == "FullName").OptionValue;
            ViewData["Title"] = _context.Options.First(i => i.OptionName == "IndexTitle").OptionValue;

            //<Posts>
            var posts = new List<PostSummaryViewModel>();
            foreach (var post in await _context.Posts.Include(p => p.Article).Include(p => p.Share).Include(p => p.Markdown).OrderByDescending(p => p.PublishDateTime).Take(8).ToListAsync())
            {
                var postType = DetectPostType(post);
                switch (postType)
                {
                    case PostType.Article:
                        posts.Add(new PostSummaryViewModel
                        {
                            Title = post.Title,
                            Type = PostType.Article,
                            PublishDateTime = post.PublishDateTime,
                            LatestUpdateDateTime = post.LatestUpdateDateTime,
                            Summary = post.Summary,
                            ThumbnailUrl = post.ThumbnailUrl,
                            PostUrl = post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Article.UrlTitle}"
                        });
                        break;
                    case PostType.Share:
                        posts.Add(new PostSummaryViewModel
                        {
                            Title = post.Title,
                            Type = PostType.Share,
                            PublishDateTime = post.PublishDateTime,
                            LatestUpdateDateTime = post.LatestUpdateDateTime,
                            Summary = post.Summary,
                            ThumbnailUrl = post.ThumbnailUrl,
                            PostUrl = post.Share.RedirectToUrl
                        });
                        break;
                    case PostType.Markdown:
                        posts.Add(new PostSummaryViewModel
                        {
                            Title = post.Title,
                            Type = PostType.Markdown,
                            PublishDateTime = post.PublishDateTime,
                            LatestUpdateDateTime = post.LatestUpdateDateTime,
                            Summary = post.Summary,
                            ThumbnailUrl = post.ThumbnailUrl,
                            PostUrl = post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Markdown.UrlTitle}"
                        });
                        break;
                    default:
                        continue; //نباید تحت هیچ عنوان به اینجا وارد بشه، باید تدابیری اندیشه بشه که اگر چنین اتفاقی افتاد مدیر سایت باخبر بشه، از طریق ایمیل یا لاگ انداختن
                }
            }
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
                    Posts = posts,
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