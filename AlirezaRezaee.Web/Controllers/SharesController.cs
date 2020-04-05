using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AlirezaRezaee.Web.Data;
using AlirezaRezaee.Web.Extensions;
using AlirezaRezaee.Web.Helpers;
using AlirezaRezaee.Web.Models.ViewModels.Shares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlirezaRezaee.Web.Controllers
{
    public class SharesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _ifileManager;

        public SharesController(ApplicationDbContext context, IFileManager ifileManager)
        {
            _context = context;
            _ifileManager = ifileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shares.Select(share =>
                new ShareSummary
                {
                    ShareId = share.ShareId,
                    Title = share.Title,
                    Summary = share.Summary,
                    ThumbnailUrl = share.ThumbnailUrl,
                    PublishDateTime = share.PublishDateTime,
                    LatestUpdateDateTime = share.LatestUpdateDateTime,
                    Url = share.Url
                }).ToListAsync());
        }

        [Route("shares/details/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{shareUrlTitle}")]
        [Route("details/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{shareId:regex(^[[0-9]]+[[13579]]$)}/{shareUrlTitle?}")]
        public async Task<IActionResult> Details(int year, int month, int day, int? shareId, string shareUrlTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var share = (shareId != null)
                ? await _context.Shares.Where(s => s.PublishDateTime.Year == dateTime.Year && s.PublishDateTime.Month == dateTime.Month && s.PublishDateTime.Day == dateTime.Day && s.ShareId == shareId).FirstOrDefaultAsync()
                : await _context.Shares.Where(s => s.PublishDateTime.Year == dateTime.Year && s.PublishDateTime.Month == dateTime.Month && s.PublishDateTime.Day == dateTime.Day && s.UrlTitle == shareUrlTitle)
                .FirstOrDefaultAsync();

            if (share == null)
                return NotFound();

            return View(share);
        }

        [HttpGet]
        [Route("shares/delete/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{shareUrlTitle}")]
        [Route("delete/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{shareId:regex(^[[0-9]]+[[13579]]$)}/{shareUrlTitle?}")]
        public async Task<IActionResult> Delete(int year, int month, int day, int? shareId, string shareUrlTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var share = (shareId != null)
                ? await _context.Shares.Where(s => s.PublishDateTime.Year == dateTime.Year && s.PublishDateTime.Month == dateTime.Month && s.PublishDateTime.Day == dateTime.Day && s.ShareId == shareId).FirstOrDefaultAsync()
                : await _context.Shares.Where(s => s.PublishDateTime.Year == dateTime.Year && s.PublishDateTime.Month == dateTime.Month && s.PublishDateTime.Day == dateTime.Day && s.UrlTitle == shareUrlTitle)
                .FirstOrDefaultAsync();

            if (share == null)
                return NotFound();

            return View(share);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("shares/delete/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{shareUrlTitle}")]
        [Route("delete/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{shareId:regex(^[[0-9]]+[[13579]]$)}/{shareUrlTitle?}")]
        public async Task<IActionResult> DeleteDone(int year, int month, int day, int? shareId, string shareUrlTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var share = (shareId != null)
                ? await _context.Shares.Where(s => s.PublishDateTime.Year == dateTime.Year && s.PublishDateTime.Month == dateTime.Month && s.PublishDateTime.Day == dateTime.Day && s.ShareId == shareId).FirstOrDefaultAsync()
                : await _context.Shares.Where(s => s.PublishDateTime.Year == dateTime.Year && s.PublishDateTime.Month == dateTime.Month && s.PublishDateTime.Day == dateTime.Day && s.UrlTitle == shareUrlTitle)
                .FirstOrDefaultAsync();

            if (share == null)
                return NotFound();

            _context.Remove(share);
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
                    if (createViewModel.Share.UrlTitle == null)
                        createViewModel.Share.UrlTitle = createViewModel.Share.Title;
                    foreach (char c in Path.GetInvalidFileNameChars())
                        createViewModel.Share.UrlTitle = createViewModel.Share.UrlTitle.Replace(c, '-');


                    createViewModel.Thumbnail.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });

                    var randomNumber = new Random();
                    var thumbnailPath = Path.Combine(
                        "uploads/images/",
                        persianDateTime.ToString("yyyy/MM/dd"),
                        persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999) + "-" + createViewModel.Thumbnail.FileName);

                    _ifileManager.SaveFile(createViewModel.Thumbnail, thumbnailPath);

                    createViewModel.Share.ThumbnailUrl = thumbnailPath;

                    createViewModel.Share.PublishDateTime = dateTime;

                    _context.Add(createViewModel.Share);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    TempData["shareCreateStatus"] = e.Message;
                }

                return RedirectToAction("Details",
                    new
                    {
                        year = dateTime.Year,
                        month = dateTime.Month,
                        day = dateTime.Day,
                        shareTitle = createViewModel.Share.Title
                    });
            }

            return View(createViewModel);
        }

        [HttpGet]
        [Route("shares/edit/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{shareUrlTitle}")]
        [Route("edit/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{shareId:regex(^[[0-9]]+[[13579]]$)}/{shareUrlTitle?}")]
        public async Task<IActionResult> Edit(int year, int month, int day, int? shareId, string shareUrlTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var share = (shareId != null)
                ? await _context.Shares.Where(s => s.PublishDateTime.Year == dateTime.Year && s.PublishDateTime.Month == dateTime.Month && s.PublishDateTime.Day == dateTime.Day && s.ShareId == shareId).FirstOrDefaultAsync()
                : await _context.Shares.Where(s => s.PublishDateTime.Year == dateTime.Year && s.PublishDateTime.Month == dateTime.Month && s.PublishDateTime.Day == dateTime.Day && s.UrlTitle == shareUrlTitle)
                .FirstOrDefaultAsync();

            if (share == null)
                return NotFound();

            return View(share);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("shares/edit/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{shareUrlTitle}")]
        [Route("edit/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{shareId:regex(^[[0-9]]+[[13579]]$)}/{shareUrlTitle?}")]
        public async Task<IActionResult> Edit(int year, int month, int day, int? shareId, string shareUrlTitle, CreateEditViewModel editViewModel)
        {
            var publishedDateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var share = (shareId != null)
                ? await _context.Shares.Where(s => s.PublishDateTime.Year == publishedDateTime.Year && s.PublishDateTime.Month == publishedDateTime.Month && s.PublishDateTime.Day == publishedDateTime.Day && s.ShareId == shareId).FirstOrDefaultAsync()
                : await _context.Shares.Where(s => s.PublishDateTime.Year == publishedDateTime.Year && s.PublishDateTime.Month == publishedDateTime.Month && s.PublishDateTime.Day == publishedDateTime.Day && s.UrlTitle == shareUrlTitle)
                .FirstOrDefaultAsync();

            if (share == null)
                return NotFound();


            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Now;
                var dateTime = persianDateTime.ToDateTime();

                if (editViewModel.Thumbnail == null)
                {
                    try
                    {
                        if (editViewModel.Share.UrlTitle == null)
                            editViewModel.Share.UrlTitle = editViewModel.Share.Title;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            editViewModel.Share.UrlTitle = editViewModel.Share.UrlTitle.Replace(c, '-');


                        editViewModel.Share.LatestUpdateDateTime = dateTime;
                        _context.Update(editViewModel.Share);
                        await _context.SaveChangesAsync();
                        TempData["shareEditStatus"] = "OK";
                    }
                    catch (Exception e)
                    {
                        TempData["shareEditStatus"] = e.Message;
                        return View(editViewModel);
                    }
                }
                else
                {
                    try
                    {
                        if (editViewModel.Share.UrlTitle == null)
                            editViewModel.Share.UrlTitle = editViewModel.Share.Title;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            editViewModel.Share.UrlTitle = editViewModel.Share.UrlTitle.Replace(c, '-');


                        var randomNumber = new Random();
                        var imagePath = Path.Combine("uploads/images/", persianDateTime.ToString("yyyy/MM/dd"));

                        editViewModel.Thumbnail.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                        var thumbnailPath = Path.Combine(imagePath, persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999) + "-" + editViewModel.Thumbnail.FileName);
                        _ifileManager.SaveFile(editViewModel.Thumbnail, thumbnailPath);
                        editViewModel.Share.ThumbnailUrl = thumbnailPath;

                        editViewModel.Share.LatestUpdateDateTime = dateTime;

                        _context.Update(editViewModel.Share);
                        await _context.SaveChangesAsync();
                        TempData["shareEditStatus"] = "OK";
                    }
                    catch (Exception e)
                    {
                        TempData["shareEditStatus"] = e.Message;
                        return View(editViewModel);
                    }
                }

                return RedirectToAction("Details",
                    new
                    {
                        year = dateTime.Year,
                        month = dateTime.Month,
                        day = dateTime.Day,
                        shareTitle = editViewModel.Share.Title
                    });
            }

            return View(editViewModel);
        }

        [Route("shares/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{shareUrlTitle}")]
        [Route("{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{shareId:regex(^[[0-9]]+[[13579]]$)}/{shareUrlTitle?}")]
        public async Task<IActionResult> GoToLink(int year, int month, int day, int? shareId, string shareUrlTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var sharedLink = (shareId != null)
                ? await _context.Shares.Where(s => s.PublishDateTime.Year == dateTime.Year && s.PublishDateTime.Month == dateTime.Month && s.PublishDateTime.Day == dateTime.Day && s.ShareId == shareId)
                .Select(s => s.Url).FirstOrDefaultAsync()
                : await _context.Shares.Where(s => s.PublishDateTime.Year == dateTime.Year && s.PublishDateTime.Month == dateTime.Month && s.PublishDateTime.Day == dateTime.Day && s.UrlTitle == shareUrlTitle)
                .Select(s => s.Url).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(sharedLink))
                return NotFound();

            return Redirect(sharedLink);
        }
    }
}