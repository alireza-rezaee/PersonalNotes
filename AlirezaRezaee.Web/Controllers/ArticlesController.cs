using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AlirezaRezaee.Web.Data;
using AlirezaRezaee.Web.Extensions;
using AlirezaRezaee.Web.Helpers;
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
            //ToDo: Select Filter
            return View(await _context.Articles.ToListAsync());
        }

        [Route("{year}/(month}/{day}/{articleTitle}")]
        public async Task<IActionResult> Details(int year, int month, int day, string articleTitle)
        {
            var article = await _context.Articles
                .Where(a => a.PublishDateTime.Year == year && a.PublishDateTime.Month == month && a.PublishDateTime.Day == day && a.Title == articleTitle)
                .FirstOrDefaultAsync();

            if (article == null)
                return NotFound();

            return View(article);
        }

        [HttpGet]
        [Route("delete/{year}/(month}/{day}/{articleTitle}")]
        public async Task<IActionResult> Delete(int year, int month, int day, string articleTitle)
        {
            var article = await _context.Articles
                .Where(a => a.PublishDateTime.Year == year && a.PublishDateTime.Month == month && a.PublishDateTime.Day == day && a.Title == articleTitle)
                .FirstOrDefaultAsync();

            if (article == null)
                return NotFound();

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete/{year}/(month}/{day}/{articleTitle}")]
        public async Task<IActionResult> DeleteDone(int year, int month, int day, string articleTitle)
        {
            var article = await _context.Articles
                .Where(a => a.PublishDateTime.Year == year && a.PublishDateTime.Month == month && a.PublishDateTime.Day == day && a.Title == articleTitle)
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
        [Route("edit/{year}/(month}/{day}/{articleTitle}")]
        public async Task<IActionResult> Edit(int year, int month, int day, string articleTitle)
        {
            var article = await _context.Articles
                .Where(a => a.PublishDateTime.Year == year && a.PublishDateTime.Month == month && a.PublishDateTime.Day == day && a.Title == articleTitle)
                .FirstOrDefaultAsync();

            if (article == null)
                return NotFound();

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/{year}/(month}/{day}/{articleTitle}")]
        public async Task<IActionResult> Edit(int year, int month, int day, string articleTitle, CreateEditViewModel editViewModel)
        {
            var article = await _context.Articles
                .Where(a => a.PublishDateTime.Year == year && a.PublishDateTime.Month == month && a.PublishDateTime.Day == day && a.Title == articleTitle)
                .FirstOrDefaultAsync();

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