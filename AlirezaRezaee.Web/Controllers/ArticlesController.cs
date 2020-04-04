using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AlirezaRezaee.Web.Data;
using AlirezaRezaee.Web.Extensions;
using AlirezaRezaee.Web.Helpers;
using AlirezaRezaee.Web.Models.ViewModels.Articles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CreateEditViewModel = AlirezaRezaee.Web.Models.ViewModels.Articles.CreateEditViewModel;

namespace AlirezaRezaee.Web.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _ifileManager;

        public ArticlesController(ApplicationDbContext context, IFileManager ifileManager)
        {
            _context = context;
            _ifileManager = ifileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Articles.Select(article =>
                new ArticleSummary
                {
                    ArticleId = article.ArticleId,
                    Title = article.Title,
                    Summary = article.Summary,
                    ThumbnailUrl = article.ThumbnailUrl,
                    PublishDateTime = article.PublishDateTime,
                    LatestUpdateDateTime = article.LatestUpdateDateTime,
                    ArticleCategories = article.ArticleCategories
                }).ToListAsync());
        }

        [Route("articles/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{articleTitle}")]
        [Route("{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{articleId:regex(^[[0-9]]+[[02468]]$)}/{articleTitle?}")]
        public async Task<IActionResult> Details(int year, int month, int day, int? articleId, string articleTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var article = (articleId != null)
                ? await _context.Articles.Where(a => a.PublishDateTime.Year == dateTime.Year && a.PublishDateTime.Month == dateTime.Month && a.PublishDateTime.Day == dateTime.Day && a.ArticleId == articleId).FirstOrDefaultAsync()
                : await _context.Articles.Where(a => a.PublishDateTime.Year == dateTime.Year && a.PublishDateTime.Month == dateTime.Month && a.PublishDateTime.Day == dateTime.Day && a.Title == articleTitle)
                .FirstOrDefaultAsync();

            if (article == null)
                return NotFound();

            return View(article);
        }

        [HttpGet]
        [Route("articles/delete/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{articleTitle}")]
        [Route("delete/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{articleId:regex(^[[0-9]]+[[02468]]$)}/{articleTitle?}")]
        public async Task<IActionResult> Delete(int year, int month, int day, int? articleId, string articleTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();
            var article = (articleId != null)
                ? await _context.Articles.Where(a => a.PublishDateTime.Year == dateTime.Year && a.PublishDateTime.Month == dateTime.Month && a.PublishDateTime.Day == dateTime.Day && a.ArticleId == articleId).FirstOrDefaultAsync()
                : await _context.Articles.Where(a => a.PublishDateTime.Year == dateTime.Year && a.PublishDateTime.Month == dateTime.Month && a.PublishDateTime.Day == dateTime.Day && a.Title == articleTitle)
                .FirstOrDefaultAsync();

            if (article == null)
                return NotFound();

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("articles/delete/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{articleTitle}")]
        [Route("delete/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{articleId:regex(^[[0-9]]+[[02468]]$)}/{articleTitle?}")]
        public async Task<IActionResult> DeleteDone(int year, int month, int day, int? articleId, string articleTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var article = (articleId != null)
                ? await _context.Articles.Where(a => a.PublishDateTime.Year == dateTime.Year && a.PublishDateTime.Month == dateTime.Month && a.PublishDateTime.Day == dateTime.Day && a.ArticleId == articleId).FirstOrDefaultAsync()
                : await _context.Articles.Where(a => a.PublishDateTime.Year == dateTime.Year && a.PublishDateTime.Month == dateTime.Month && a.PublishDateTime.Day == dateTime.Day && a.Title == articleTitle)
                .FirstOrDefaultAsync();

            if (article == null)
                return NotFound();

            _context.Remove(article);
            await _context.SaveChangesAsync();

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditViewModel createViewModel)
        {
            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Now;
                var dateTime = persianDateTime.ToDateTime();

                try
                {
                    if (createViewModel.Article.UrlTitle == null)
                        createViewModel.Article.UrlTitle = createViewModel.Article.Title;
                    foreach (char c in Path.GetInvalidFileNameChars())
                        createViewModel.Article.UrlTitle = createViewModel.Article.UrlTitle.Replace(c, '-');


                    createViewModel.Thumbnail.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                    createViewModel.Cover.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });


                    var randomNumber = new Random();
                    var imagePath = Path.Combine("uploads/images/", persianDateTime.ToString("yyyy/MM/dd"));
                    var thumbnailPath = Path.Combine(imagePath, persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999) + "-" + createViewModel.Thumbnail.FileName);
                    var coverPath = Path.Combine(imagePath, persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999) + "-" + createViewModel.Cover.FileName);

                    _ifileManager.SaveFile(createViewModel.Thumbnail, thumbnailPath);
                    _ifileManager.SaveFile(createViewModel.Cover, coverPath);

                    createViewModel.Article.ThumbnailUrl = thumbnailPath;
                    createViewModel.Article.CoverUrl = coverPath;

                    createViewModel.Article.PublishDateTime = dateTime;

                    _context.Add(createViewModel.Article);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    TempData["ArticleCreateStatus"] = e.Message;
                }

                return RedirectToAction("Details",
                    new
                    {
                        year = dateTime.Year,
                        month = dateTime.Month,
                        day = dateTime.Day,
                        articleTitle = createViewModel.Article.Title
                    });
            }

            return View(createViewModel);
        }

        [HttpGet]
        [Route("articles/edit/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{articleTitle}")]
        [Route("edit/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{articleId:regex(^[[0-9]]+[[02468]]$)}/{articleTitle?}")]
        public async Task<IActionResult> Edit(int year, int month, int day, int? articleId, string articleTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var article = (articleId != null)
                ? await _context.Articles.Where(a => a.PublishDateTime.Year == dateTime.Year && a.PublishDateTime.Month == dateTime.Month && a.PublishDateTime.Day == dateTime.Day && a.ArticleId == articleId).FirstOrDefaultAsync()
                : await _context.Articles.Where(a => a.PublishDateTime.Year == dateTime.Year && a.PublishDateTime.Month == dateTime.Month && a.PublishDateTime.Day == dateTime.Day && a.Title == articleTitle)
                .FirstOrDefaultAsync();

            if (article == null)
                return NotFound();

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("articles/edit/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{articleTitle}")]
        [Route("edit/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{articleId:regex(^[[0-9]]+[[02468]]$)}/{articleTitle?}")]
        public async Task<IActionResult> Edit(int year, int month, int day, int? articleId, string articleTitle, CreateEditViewModel editViewModel)
        {
            var publishedDateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var article = (articleId != null)
                ? await _context.Articles.Where(a => a.PublishDateTime.Year == publishedDateTime.Year && a.PublishDateTime.Month == publishedDateTime.Month && a.PublishDateTime.Day == publishedDateTime.Day && a.ArticleId == articleId).FirstOrDefaultAsync()
                : await _context.Articles.Where(a => a.PublishDateTime.Year == publishedDateTime.Year && a.PublishDateTime.Month == publishedDateTime.Month && a.PublishDateTime.Day == publishedDateTime.Day && a.Title == articleTitle).FirstOrDefaultAsync();

            if (article == null)
                return NotFound();


            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Now;
                var dateTime = persianDateTime.ToDateTime();

                if (editViewModel.Thumbnail == null && editViewModel.Cover == null)
                {
                    try
                    {
                        if (editViewModel.Article.UrlTitle == null)
                            editViewModel.Article.UrlTitle = editViewModel.Article.Title;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            editViewModel.Article.UrlTitle = editViewModel.Article.UrlTitle.Replace(c, '-');


                        editViewModel.Article.LatestUpdateDateTime = dateTime;
                        _context.Update(editViewModel.Article);
                        await _context.SaveChangesAsync();
                        TempData["ArticleEditStatus"] = "OK";
                    }
                    catch (Exception e)
                    {
                        TempData["ArticleEditStatus"] = e.Message;
                        return View(editViewModel);
                    }
                }
                else
                {
                    try
                    {
                        var randomNumber = new Random();
                        var imagePath = Path.Combine("uploads/images/", persianDateTime.ToString("yyyy/MM/dd"));


                        if (editViewModel.Article.UrlTitle == null)
                            editViewModel.Article.UrlTitle = editViewModel.Article.Title;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            editViewModel.Article.UrlTitle = editViewModel.Article.UrlTitle.Replace(c, '-');


                        if (editViewModel.Thumbnail != null && editViewModel.Cover != null)
                        {
                            editViewModel.Thumbnail.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                            var thumbnailPath = Path.Combine(imagePath, persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999) + "-" + editViewModel.Thumbnail.FileName);
                            _ifileManager.SaveFile(editViewModel.Thumbnail, thumbnailPath);
                            editViewModel.Article.ThumbnailUrl = thumbnailPath;

                            editViewModel.Cover.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                            var coverPath = Path.Combine(imagePath, persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999) + "-" + editViewModel.Cover.FileName);
                            _ifileManager.SaveFile(editViewModel.Cover, coverPath);
                            editViewModel.Article.CoverUrl = coverPath;
                        }
                        else if (editViewModel.Thumbnail != null)
                        {
                            editViewModel.Thumbnail.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                            var thumbnailPath = Path.Combine(imagePath, persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999) + "-" + editViewModel.Thumbnail.FileName);
                            _ifileManager.SaveFile(editViewModel.Thumbnail, thumbnailPath);
                            editViewModel.Article.ThumbnailUrl = thumbnailPath;
                        }
                        else // if (editViewModel.Cover != null)
                        {
                            editViewModel.Cover.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                            var coverPath = Path.Combine(imagePath, persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999) + "-" + editViewModel.Cover.FileName);
                            _ifileManager.SaveFile(editViewModel.Cover, coverPath);
                            editViewModel.Article.CoverUrl = coverPath;
                        }

                        editViewModel.Article.LatestUpdateDateTime = dateTime;

                        _context.Update(editViewModel.Article);
                        await _context.SaveChangesAsync();
                        TempData["ArticleEditStatus"] = "OK";
                    }
                    catch (Exception e)
                    {
                        TempData["ArticleEditStatus"] = e.Message;
                        return View(editViewModel);
                    }
                }

                return RedirectToAction("Details",
                    new
                    {
                        year = dateTime.Year,
                        month = dateTime.Month,
                        day = dateTime.Day,
                        articleTitle = editViewModel.Article.Title
                    });
            }

            return View(editViewModel);
        }
    }
}