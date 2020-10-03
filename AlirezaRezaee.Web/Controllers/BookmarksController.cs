using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Rezaee.Alireza.Web.Controllers
{
    [Route("bookmarks")]
    public class BookmarksController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
