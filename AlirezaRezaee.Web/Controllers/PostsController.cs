using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlirezaRezaee.Web.Data;
using AlirezaRezaee.Web.Models.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlirezaRezaee.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var articlePosts = await _context.Articles.Select(article => new PostSummaryViewModel
            {
                Id = article.ArticleId,
                Title = article.Title,
                Summmary = article.Summmary,
                PublishDateTime = article.PublishDateTime,
                LatestUpdateDateTime = article.LatestUpdateDateTime,
                ImageUrl = article.ImageUrl,
                Category = article.ArticleCategories.First().Category.Title
            }).OrderByDescending(article => article.LatestUpdateDateTime).ThenByDescending(article => article.PublishDateTime).Take(8).ToListAsync();

            var sharePosts = await _context.Shares.Select(share => new PostSummaryViewModel
            {
                Id = share.ShareId,
                Title = share.Title,
                Summmary = share.Summmary,
                PublishDateTime = share.PublishDateTime,
                LatestUpdateDateTime = share.LatestUpdateDateTime,
                ImageUrl = share.ImageUrl,
                Category = "بازنشر",
                PostUrl = share.Url
            }).OrderByDescending(share => share.LatestUpdateDateTime).ThenByDescending(share => share.PublishDateTime).Take(8).ToListAsync();

            articlePosts.AddRange(sharePosts);

            return View(articlePosts.OrderByDescending(post => post.LatestUpdateDateTime).ThenByDescending(post => post.PublishDateTime).Take(8).ToList());
        }

        [Route("/article/{Id}")]
        [Route("/article/{Id}/{Title}")]
        public async Task<IActionResult> ViewPost(int id, string title)
        {
            ViewData["Title"] = title;
            return View(await _context.Articles.Where(article => article.ArticleId == id).FirstAsync());
        }
    }
}