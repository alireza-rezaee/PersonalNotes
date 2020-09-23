using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Extensions;
using Rezaee.Alireza.Web.Helpers;
using Rezaee.Alireza.Web.Models;

namespace Rezaee.Alireza.Web.Controllers
{
    [Route("links")]
    public class LinksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _ifileManager;

        public LinksController(ApplicationDbContext context, IFileManager ifileManager)
        {
            _context = context;
            _ifileManager = ifileManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index() => View(await _context.Links.OrderBy(link => link.IsExpanded).ThenBy(link => link.Id).ToListAsync());

        [HttpGet("create")]
        [Authorize(Roles = Roles.LinkCreate)]
        public IActionResult Create() => View();

        // POST: Links/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.LinkCreate)]
        public async Task<IActionResult> Create(Link link, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (image != null)
                    {
                        image.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                        var imagePath = $"uploads/links/{PersianDateTime.Now.ToString("yyyy/MM/dd/yyyyMMddhhmmss")}{DateTime.Now.ToString("ffff") + new Random().Next(1000000, 9999999)}_{Helpers.File.ValidateName(image.FileName)}";
                        await _ifileManager.SaveFile(image, imagePath);
                        link.ImagePath = $"/{imagePath}";
                    }

                    _context.Add(link);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    TempData["LinkCreateStatus"] = e.Message;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(link);
        }

        [HttpGet("edit/{id}")]
        [Authorize(Roles = Roles.LinkEdit)]
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var link = await _context.Links.FindAsync(id);
            if (link == null)
            {
                return NotFound();
            }
            return View(link);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.LinkEdit)]
        public async Task<IActionResult> Edit(short id, IFormFile image, Link linksModel)
        {
            if (id != linksModel.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var link = await _context.Links.FindAsync(id);
                    if (link == null)
                        return NotFound();

                    if (image != null)
                    {
                        image.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                        var imagePath = $"uploads/links/{PersianDateTime.Now.ToString("yyyy/MM/dd/yyyyMMddhhmmss")}{DateTime.Now.ToString("ffff") + new Random().Next(1000000, 9999999)}-{Helpers.File.ValidateName(image.FileName)}";

                        //Upload new image
                        await _ifileManager.SaveFile(image, imagePath);
                        //Remove old image
                        _ifileManager.DeleteFile(link.ImagePath);
                        //Set new image
                        link.ImagePath = $"/{imagePath}";
                    }

                    link.IsExpanded = linksModel.IsExpanded;
                    link.Title = linksModel.Title;
                    link.Url = linksModel.Url;

                    _context.Update(link);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    TempData["LinkEditStatus"] = e.Message;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(linksModel);
        }

        // POST: Links/Delete/5
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.LinkDelete)]
        public async Task<IActionResult> Delete(short id)
        {
            try
            {
                var link = await _context.Links.FindAsync(id);
                if (link == null)
                    return NotFound();

                _ifileManager.DeleteFile(link.ImagePath);
                _context.Links.Remove(link);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                TempData["LinkDeleteStatus"] = e.Message;
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LinksModelExists(short id)
        {
            return _context.Links.Any(e => e.Id == id);
        }
    }
}
