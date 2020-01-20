using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlirezaRezaee.Web.Data;
using AlirezaRezaee.Web.Models;
using AlirezaRezaee.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;

namespace AlirezaRezaee.Web.Controllers
{
    public class NotesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly OptionViewModel _profileOptions;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public NotesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;

            //var options = _context.Options;

            _profileOptions = GetProfileOptions();
            //_profileOptions.QuranAyah = options.First(i => i.OptionName == "QuranAyah").OptionValue;
            //_profileOptions.AboutAuthorSummary = options.First(i => i.OptionName == "AboutAuthorSummary").OptionValue;

            ////_profileOptions = new OptionViewModel()
            ////{
            ////    //FirstName = options.First(i => i.OptionName == "FirstName").OptionValue,
            ////    //LastName = options.First(i => i.OptionName == "LastName").OptionValue,
            ////    //AvatarOrginalPath = options.First(i => i.OptionName == "AvatarOrginalPath").OptionValue,
            ////    //AvatarPath_64px = options.First(i => i.OptionName == "AvatarPath_64px").OptionValue,
            ////    //AvatarPath_100px = options.First(i => i.OptionName == "AvatarPath_100px").OptionValue,
            ////    //AvatarPath_125px = options.First(i => i.OptionName == "AvatarPath_125px").OptionValue,
            ////    //AvatarPath_150px = options.First(i => i.OptionName == "AvatarPath_150px").OptionValue,
            ////    //IllustratedNamePath = options.First(i => i.OptionName == "IllustratedNamePath").OptionValue,
            ////    //CoverPath = options.First(i => i.OptionName == "CoverPath").OptionValue,
            ////    //SiteFootnote = options.First(i => i.OptionName == "SiteFootnote").OptionValue,
            ////    QuranAyah = options.First(i => i.OptionName == "QuranAyah").OptionValue,
            ////    AboutAuthorSummary = options.First(i => i.OptionName == "AboutAuthorSummary").OptionValue
            ////};
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.ProfileOptions = _profileOptions;
        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            return _signInManager.IsSignedIn(User) ? View("IndexAdmin", await _context.Notes.ToListAsync()) : View("Index", await _context.Notes.ToListAsync());
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorNoteModel = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorNoteModel == null)
            {
                return NotFound();
            }

            return View(authorNoteModel);
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateTime,Content")] AuthorNoteModel authorNoteModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(authorNoteModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(authorNoteModel);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorNoteModel = await _context.Notes.FindAsync(id);
            if (authorNoteModel == null)
            {
                return NotFound();
            }
            return View(authorNoteModel);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateTime,Content")] AuthorNoteModel authorNoteModel)
        {
            if (id != authorNoteModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(authorNoteModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorNoteModelExists(authorNoteModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(authorNoteModel);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorNoteModel = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorNoteModel == null)
            {
                return NotFound();
            }

            return View(authorNoteModel);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var authorNoteModel = await _context.Notes.FindAsync(id);
            _context.Notes.Remove(authorNoteModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorNoteModelExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
