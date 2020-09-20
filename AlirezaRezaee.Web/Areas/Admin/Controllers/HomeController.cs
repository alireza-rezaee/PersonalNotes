using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Rezaee.Alireza.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/home")]
    public class HomeController : Controller
    {
        [Route("/admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
