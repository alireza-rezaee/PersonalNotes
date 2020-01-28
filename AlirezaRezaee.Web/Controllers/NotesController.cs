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
using Microsoft.AspNetCore.Authorization;
using AlirezaRezaee.Web.Models.ViewModels.Notes;
using AlirezaRezaee.Web.Areas.Identity.Data;

namespace AlirezaRezaee.Web.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public NotesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewData["AvatarPath_64px"] = _context.Options.First(i => i.OptionName == "AvatarPath_64px").OptionValue;
            ViewData["FullName"] = _context.Options.First(i => i.OptionName == "FullName").OptionValue;
        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            return _signInManager.IsSignedIn(User) ?
                View("IndexAdmin", new IndexAuthorNoteViewModel() { AuthorNotes = await _context.Notes.OrderByDescending(n=>n.DateTime).ToListAsync() }) : View("Index", await _context.Notes.OrderByDescending(n => n.DateTime).ToListAsync());
        }
        
        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //return 404 for unauthenticated users
            if (!_signInManager.IsSignedIn(User)) return NotFound();

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

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(IndexAuthorNoteViewModel newNote)
        {
            newNote.SingleNote.DateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(newNote.SingleNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newNote);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, IndexAuthorNoteViewModel note)
        {
            var Note = await _context.Notes.FirstOrDefaultAsync(n=>n.Id == id);

            if (Note == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    Note.Content = note.SingleNote.Content;
                    _context.Update(Note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorNoteModelExists(note.SingleNote.Id))
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
            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
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
