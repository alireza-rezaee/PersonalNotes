using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rezaee.Alireza.Web.Areas.Admin.Models;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Extensions;
using Rezaee.Alireza.Web.Helpers;

namespace Rezaee.Alireza.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/files")]
    public class FilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IFileManager _fileManager;

        public FilesController(ApplicationDbContext context, IWebHostEnvironment env, IFileManager fileManager)
        {
            _context = context;
            _env = env;
            _fileManager = fileManager;
        }

        [Route("")]
        [Authorize(Roles = Roles.FilesList)]
        public async Task<IActionResult> Index()
        {
            //TODO: Pagination
            return View(await _context.Files.ToListAsync());
        }

        [HttpGet("upload")]
        [Authorize(Roles = Roles.FileUpload)]
        public IActionResult Upload() => View();

        [HttpPost("upload")]
        [DisableRequestSizeLimit]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.FileUpload)]
        public async Task<IActionResult> Uploading(List<IFormFile> files)
        {
            var filesRow = new List<Models.File>();
            try
            {
                foreach (var file in files)
                {
                    var dateTime = DateTime.Now;
                    var filePath = $"uploads/files/{dateTime.ToPersianDateTime().ToString("yyyy/MM/dd/yyyyMMddhhmmss") + dateTime.ToString("ffff") + new Random().Next(1000000, 9999999)}_{Helpers.File.ValidateName(file.FileName)}";
                    await _fileManager.SaveFile(
                        file: file,
                        path: filePath);

                    filesRow.Add(new Models.File { FilePath = $"/{filePath}", CreateDateTime = dateTime, ContentType = file.ContentType, Length = file.Length });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                await _context.AddRangeAsync(filesRow);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(actionName: nameof(Index), controllerName: nameof(FilesController).ControllerName());
        }

        [HttpPost("remove")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.FileRemove)]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();
            try
            {
                var file = await _context.Files.FindAsync(id);
                if (file is null)
                    return NotFound();

                _fileManager.DeleteFile(file.FilePath);
                _context.Remove(file);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction(actionName: nameof(Index), controllerName: nameof(FilesController).ControllerName());
        }
    }
}
