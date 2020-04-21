using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Models.ViewModels.HeaderFooter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.ViewComponents
{
    public class HeaderFooterViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public HeaderFooterViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string page = "HeadCover")
        {
            var options = _context.Options;
            if (page == "HeadCover")
                return View(page, new HeadCoverViewModel()
                {
                    //ToDo: What if null FirstOrDefaultAsync();
                    FullName = (await options.FirstOrDefaultAsync(i => i.OptionName == "FullName")).OptionValue,
                    AvatarPath_64px = (await options.FirstOrDefaultAsync(i => i.OptionName == "AvatarPath_64px")).OptionValue,
                    AvatarPath_100px = (await options.FirstOrDefaultAsync(i => i.OptionName == "AvatarPath_100px")).OptionValue,
                    AvatarPath_125px = (await options.FirstOrDefaultAsync(i => i.OptionName == "AvatarPath_125px")).OptionValue,
                    AvatarPath_150px = (await options.FirstOrDefaultAsync(i => i.OptionName == "AvatarPath_150px")).OptionValue,
                    IllustratedNamePath = (await options.FirstOrDefaultAsync(i => i.OptionName == "IllustratedNamePath")).OptionValue,
                    CoverPath = (await options.FirstOrDefaultAsync(i => i.OptionName == "CoverPath")).OptionValue
                });
            else if (page == "FootCover")
            {
                ViewData["FootNote"] = (await options.FirstOrDefaultAsync(i => i.OptionName == "SiteFootnote")).OptionValue;
                return View(page, new FootCoverViewModel() {
                    SiteFootnote = (await options.FirstOrDefaultAsync(i => i.OptionName == "SiteFootnote")).OptionValue
                });
            }

            throw new Exception();
        }
    }
}