using AlirezaRezaee.Web.Data;
using AlirezaRezaee.Web.Models.ViewModels.Profile;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.ViewComponents
{
    public class ProfileViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ProfileViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string page = "HeadCover")
        {
            var options = _context.Options;
            if (page == "HeadCover")
                return View(page, new HeadCoverViewModel()
                {
                    FullName = options.First(i => i.OptionName == "FullName").OptionValue,
                    AvatarPath_64px = options.First(i => i.OptionName == "AvatarPath_64px").OptionValue,
                    AvatarPath_100px = options.First(i => i.OptionName == "AvatarPath_100px").OptionValue,
                    AvatarPath_125px = options.First(i => i.OptionName == "AvatarPath_125px").OptionValue,
                    AvatarPath_150px = options.First(i => i.OptionName == "AvatarPath_150px").OptionValue,
                    IllustratedNamePath = options.First(i => i.OptionName == "IllustratedNamePath").OptionValue,
                    CoverPath = options.First(i => i.OptionName == "CoverPath").OptionValue
                });
            else if (page == "FootCover")
            {
                ViewData["FootNote"] = options.First(i => i.OptionName == "SiteFootnote").OptionValue;
                return View(page, new FootCoverViewModel() {
                    SiteFootnote = options.First(i => i.OptionName == "SiteFootnote").OptionValue
                });
            }

            throw new Exception();
        }
    }
}