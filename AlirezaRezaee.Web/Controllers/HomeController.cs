using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AlirezaRezaee.Web.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using AlirezaRezaee.Web.Data;
using AlirezaRezaee.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AlirezaRezaee.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["FullName"] = _context.Options.First(i => i.OptionName == "FullName").OptionValue;
            ViewData["Title"] = _context.Options.First(i => i.OptionName == "IndexTitle").OptionValue;

            return View(
                new HomeIndexViewModel() {
                    QuranAyah = _context.Options.First(i => i.OptionName == "QuranAyah").OptionValue,
                    AboutAuthorSummary = _context.Options.First(i => i.OptionName == "AboutAuthorSummary").OptionValue
                });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}