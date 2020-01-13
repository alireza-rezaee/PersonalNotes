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

namespace AlirezaRezaee.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly OptionViewModel _profileOptions;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger) : base (context)
        {
            _context = context;
            _logger = logger;

            var options = _context.Options;

            _profileOptions = GetProfileOptions();
            _profileOptions.QuranAyah = options.First(i => i.OptionName == "QuranAyah").OptionValue;
            _profileOptions.AboutAuthorSummary = options.First(i => i.OptionName == "AboutAuthorSummary").OptionValue;

            //_profileOptions = new OptionViewModel()
            //{
            //    //FirstName = options.First(i => i.OptionName == "FirstName").OptionValue,
            //    //LastName = options.First(i => i.OptionName == "LastName").OptionValue,
            //    //AvatarOrginalPath = options.First(i => i.OptionName == "AvatarOrginalPath").OptionValue,
            //    //AvatarPath_64px = options.First(i => i.OptionName == "AvatarPath_64px").OptionValue,
            //    //AvatarPath_100px = options.First(i => i.OptionName == "AvatarPath_100px").OptionValue,
            //    //AvatarPath_125px = options.First(i => i.OptionName == "AvatarPath_125px").OptionValue,
            //    //AvatarPath_150px = options.First(i => i.OptionName == "AvatarPath_150px").OptionValue,
            //    //IllustratedNamePath = options.First(i => i.OptionName == "IllustratedNamePath").OptionValue,
            //    //CoverPath = options.First(i => i.OptionName == "CoverPath").OptionValue,
            //    //SiteFootnote = options.First(i => i.OptionName == "SiteFootnote").OptionValue,
            //    QuranAyah = options.First(i => i.OptionName == "QuranAyah").OptionValue,
            //    AboutAuthorSummary = options.First(i => i.OptionName == "AboutAuthorSummary").OptionValue
            //};
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.ProfileOptions = _profileOptions;
        }

        public IActionResult Index()
        {
            return View(new HomeIndexViewModel() { Notes = _context.Notes.OrderByDescending(n => n.DateTime).Take(2).ToList() });
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