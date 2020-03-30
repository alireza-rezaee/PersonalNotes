using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlirezaRezaee.Web.Data;
using AlirezaRezaee.Web.Models;
using AlirezaRezaee.Web.Models.ViewModels.Links;

namespace AlirezaRezaee.Web.Controllers
{
    public class LinksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LinksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Links
        public IActionResult Index()
        {
            var wholeLinks = _context.Links.OrderBy(link => link.Rank)
                .Select(list => new IllustratedLinkViewModel { Title = list.Title, ImagePath = list.ImagePath, Url = list.Url })
                .ToList();

            var numberOfPrimaryLinks = int.Parse(_context.Options.First(i => i.OptionName == "NumberOfPrimaryLinks").OptionValue);
            var illustratedLinks = wholeLinks.Where(link => !string.IsNullOrEmpty(link.ImagePath)).Take(numberOfPrimaryLinks).ToList();
            var plainLinks = wholeLinks.Where(link => !illustratedLinks.Contains(link)).Select(link => new PlainLinkViewModel { Title = link.Title, Url = link.Url }).ToList();

            return View(new IndexViewModel
            {
                IllustratedLinks = illustratedLinks,
                PlainLinks = plainLinks
            });
        }

        // GET: Links/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var linksModel = await _context.Links
                .FirstOrDefaultAsync(m => m.Rank == id);
            if (linksModel == null)
            {
                return NotFound();
            }

            return View(linksModel);
        }

        // GET: Links/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Links/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Rank,Title,Url,ImagePath")] Link linksModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(linksModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(linksModel);
        }

        // GET: Links/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var linksModel = await _context.Links.FindAsync(id);
            if (linksModel == null)
            {
                return NotFound();
            }
            return View(linksModel);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("Rank,Title,Url,ImagePath")] Link linksModel)
        {
            if (id != linksModel.Rank)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(linksModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LinksModelExists(linksModel.Rank))
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
            return View(linksModel);
        }

        // GET: Links/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var linksModel = await _context.Links
                .FirstOrDefaultAsync(m => m.Rank == id);
            if (linksModel == null)
            {
                return NotFound();
            }

            return View(linksModel);
        }

        // POST: Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var linksModel = await _context.Links.FindAsync(id);
            _context.Links.Remove(linksModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LinksModelExists(short id)
        {
            return _context.Links.Any(e => e.Rank == id);
        }
    }
}
