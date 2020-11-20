using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Extensions;

namespace Rezaee.Alireza.Web.Controllers
{
    [Route("bookmarks")]
    public class BookmarksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookmarksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("")]
        public async Task <IActionResult> Index()
        {
            ViewData["Image"] = new Uri(
                baseUri: new Uri($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}"),
                relativeUri: (await _context.Personalizations.FirstOrDefaultAsync(item => item.Title == "SiteCoverSrc")).Value).ToString();
            ViewData["Url"] = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action(nameof(Index), nameof(BookmarksController).ControllerName())}";

            return View();
        }
    }
}
