using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlirezaRezaee.Web.Data;
using AlirezaRezaee.Web.Models;
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
            var article = await _context.Articles.Where(a => a.ArticleId == id).FirstOrDefaultAsync();

            if (article == null)
                return NotFound();

            ViewData["Title"] = article.Title;

            return View(article);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDone(CreateEditViewModel createEditViewModel)
        {
            if (createEditViewModel.IsArticle)
                CreateArticle(createEditViewModel.Article);
            //else
            CreateShare(createEditViewModel.Share);

            return RedirectToAction(nameof(Index));
        }

        private async void CreateArticle(Article article)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(article);
                await _context.SaveChangesAsync();
            }
        }

        private async void CreateShare(Share share)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(share);
                await _context.SaveChangesAsync();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, bool isArticle = true)
        {
            if (isArticle == true)
            {
                var article = await _context.Articles.Where(a => a.ArticleId == id).FirstOrDefaultAsync();

                if (article == null)
                    return NotFound();

                return View(new CreateEditViewModel { IsArticle = true, Article = article });
            }
            //else will come here
            var share = await _context.Shares.Where(s => s.ShareId == id).FirstOrDefaultAsync();

            if (share == null)
                return NotFound();

            return View(new CreateEditViewModel { IsArticle = false, Share = share });
        }

        [HttpPost]
        [Route("edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditDone(CreateEditViewModel createEditViewModel)
        {
            if (createEditViewModel.IsArticle)
                EditArticle(createEditViewModel.Article);
            //else
            EditShare(createEditViewModel.Share);

            return RedirectToAction(nameof(Index));
        }

        private async void EditArticle(Article article)
        {
            if (ModelState.IsValid)
            {
                _context.Update(article);
                await _context.SaveChangesAsync();
            }
        }

        private async void EditShare(Share share)
        {
            if (ModelState.IsValid)
            {
                _context.Update(share);
                await _context.SaveChangesAsync();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, bool isArticle = true)
        {
            if (isArticle == true)
            {
                var article = await _context.Articles.Where(a => a.ArticleId == id).FirstOrDefaultAsync();

                if (article == null)
                    return NotFound();

                return View(new CreateEditViewModel { IsArticle = true, Article = article });
            }
            //else will come here
            var share = await _context.Shares.Where(s => s.ShareId == id).FirstOrDefaultAsync();

            if (share == null)
                return NotFound();

            return View(new CreateEditViewModel { IsArticle = false, Share = share });
        }

        [HttpPost]
        [Route("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id, bool isArticle)
        {
            if (isArticle == true)
            {
                var article = await _context.Articles.Where(a => a.ArticleId == id).FirstOrDefaultAsync();

                if (article == null)
                    return NotFound();

                _context.Remove(article);
                await _context.SaveChangesAsync();
            }
            //else will come here
            var share = await _context.Shares.Where(s => s.ShareId == id).FirstOrDefaultAsync();

            if (share == null)
                return NotFound();

            _context.Remove(share);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}