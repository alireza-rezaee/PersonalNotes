using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using Rezaee.Alireza.Web.Areas.Admin.Models.ViewModels.Personalization;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Extensions;
using Rezaee.Alireza.Web.Helpers;
using Rezaee.Alireza.Web.Models;

namespace Rezaee.Alireza.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/personalization")]
    [Authorize(Roles = Roles.Personalize)]
    public class PersonalizationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public PersonalizationController(ApplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> Personalize()
        {
            var personalizationTitles = new string[]
            {
                "IndexTitle", "IndexDescription", "SiteFootnote", "AdditionalTitle",
                "SiteCoverSrc", "SiteLogoSrc", "TextLogoSrc",
            };
            var personalizations = await _context.Personalizations.Where(item => personalizationTitles.Contains(item.Title)).ToListAsync();
            ViewData["SiteCoverSrc"] = personalizations.FirstOrDefault(item => item.Title == "SiteCoverSrc").Value;
            ViewData["SiteLogoSrc"] = personalizations.FirstOrDefault(item => item.Title == "SiteLogoSrc").Value;
            ViewData["TextLogoSrc"] = personalizations.FirstOrDefault(item => item.Title == "TextLogoSrc").Value;
            return View(new PersonalizingVM
            {
                IndexTitle = personalizations.FirstOrDefault(item => item.Title == "IndexTitle").Value,
                IndexDescription = personalizations.FirstOrDefault(item => item.Title == "IndexDescription").Value,
                SiteFootnote = personalizations.FirstOrDefault(item => item.Title == "SiteFootnote").Value,
                AdditionalTitle = personalizations.FirstOrDefault(item => item.Title == "AdditionalTitle").Value
            });
        }

        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Personalizing(PersonalizingVM options)
        {
            if (ModelState.IsValid)
            {
                //try
                //{
                    var personalizationTitles = new string[]
                    {
                        "IndexTitle", "IndexDescription", "SiteFootnote", "AdditionalTitle",
                        "SiteCoverSrc", "SiteLogoSrc", "TextLogoSrc",
                    };

                    //Retrieve previous personalizations
                    var personalizations = await _context.Personalizations.Where(item => personalizationTitles.Contains(item.Title)).ToListAsync();

                    personalizations.FirstOrDefault(item => item.Title == "IndexTitle").Value = options.IndexTitle;
                    personalizations.FirstOrDefault(item => item.Title == "IndexDescription").Value = options.IndexDescription;
                    personalizations.FirstOrDefault(item => item.Title == "SiteFootnote").Value = options.SiteFootnote;
                    personalizations.FirstOrDefault(item => item.Title == "AdditionalTitle").Value = options.AdditionalTitle;

                    //Upload and change Site-cover-image
                    if (options.SiteCover is object)
                    {
                        options.SiteCover.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                        var dateTime = DateTime.Now;
                        var imageSrc = personalizations.FirstOrDefault(item => item.Title == "SiteCoverSrc");
                        _fileManager.DeleteFile(imageSrc.Value);
                        var filePath = $"uploads/theme/{ Helpers.File.ValidateName(options.SiteCover.FileName) }";
                        await _fileManager.SaveFile(file: options.SiteCover, path: filePath);
                        imageSrc.Value = $"/{filePath}";
                    }

                    //Upload and change Site-logo-image
                    if (options.SiteLogo is object)
                    {
                        options.SiteLogo.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                        var dateTime = DateTime.Now;
                        var imageSrc = personalizations.FirstOrDefault(item => item.Title == "SiteLogoSrc");
                        _fileManager.DeleteFile(imageSrc.Value);
                        var filePath = $"uploads/theme/{ Helpers.File.ValidateName(options.SiteLogo.FileName) }";
                        await _fileManager.SaveFile(file: options.SiteLogo, path: filePath);
                        imageSrc.Value = $"/{ filePath }";
                    }

                    //Upload and change Text-logo-image
                    if (options.TextLogo is object)
                    {
                        options.TextLogo.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                        var dateTime = DateTime.Now;
                        var imageSrc = personalizations.FirstOrDefault(item => item.Title == "TextLogoSrc");
                        _fileManager.DeleteFile(imageSrc.Value);
                        var filePath = $"uploads/theme/{ Helpers.File.ValidateName(options.TextLogo.FileName)}";
                        await _fileManager.SaveFile(file: options.TextLogo, path: filePath);
                        imageSrc.Value = $"/{filePath}";
                    }

                    _context.UpdateRange(personalizations);
                    await _context.SaveChangesAsync();
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}
            }

            return RedirectToAction(actionName: nameof(Personalize), controllerName: nameof(PersonalizationController).ControllerName());
        }
    }
}
