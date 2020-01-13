using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlirezaRezaee.Web.Data;
using AlirezaRezaee.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlirezaRezaee.Web.Controllers
{
    public class ControllerBase : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OptionViewModel _profileOptions;

        public ControllerBase(ApplicationDbContext context)
        {
            _context = context;

            var options = _context.Options;
            _profileOptions = new OptionViewModel()
            {
                FirstName = options.First(i => i.OptionName == "FirstName").OptionValue,
                LastName = options.First(i => i.OptionName == "LastName").OptionValue,
                AvatarOrginalPath = options.First(i => i.OptionName == "AvatarOrginalPath").OptionValue,
                AvatarPath_64px = options.First(i => i.OptionName == "AvatarPath_64px").OptionValue,
                AvatarPath_100px = options.First(i => i.OptionName == "AvatarPath_100px").OptionValue,
                AvatarPath_125px = options.First(i => i.OptionName == "AvatarPath_125px").OptionValue,
                AvatarPath_150px = options.First(i => i.OptionName == "AvatarPath_150px").OptionValue,
                IllustratedNamePath = options.First(i => i.OptionName == "IllustratedNamePath").OptionValue,
                CoverPath = options.First(i => i.OptionName == "CoverPath").OptionValue,
                SiteFootnote = options.First(i => i.OptionName == "SiteFootnote").OptionValue,
            };
        }

        protected OptionViewModel GetProfileOptions()
        {
            return _profileOptions;
        }
    }
}