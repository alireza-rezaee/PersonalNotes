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
            var personalizations = await _context.Personalizations
                .Where(item => (new string[] { "IndexTitle", "SiteFootnote", "SiteCoverSrc", "SiteLogoSrc", "TextLogoSrc" })
                .Contains(item.Title)).ToListAsync();

            if (page == "HeadCover")
                return View(page, new HeadCoverViewModel()
                {
                    SiteName = personalizations.FirstOrDefault(item => item.Title == "IndexTitle").Value,
                    SiteCoverSrc = personalizations.FirstOrDefault(item => item.Title == "SiteCoverSrc").Value,
                    SiteLogoSrc = personalizations.FirstOrDefault(item => item.Title == "SiteLogoSrc").Value,
                    TextLogoSrc = personalizations.FirstOrDefault(item => item.Title == "TextLogoSrc").Value
                });
            else if (page == "FootCover")
            {
                return View(page, new FootCoverViewModel()
                {
                    SiteFootnote = personalizations.FirstOrDefault(item => item.Title == "SiteFootnote").Value
                });
            }

            throw new Exception();
        }
    }
}