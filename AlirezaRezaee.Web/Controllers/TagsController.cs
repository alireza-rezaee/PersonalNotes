using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Markdig.Parsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Extensions;
using Rezaee.Alireza.Web.Helpers;
using Rezaee.Alireza.Web.Models;
using Rezaee.Alireza.Web.Models.ViewModels.Tags;

namespace Rezaee.Alireza.Web.Controllers
{
    [Route("tags")]
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static readonly string persianChars = "اآبپتثجچحخدذرزژسشصضطظعغفقکگلمنوهی"; //persian in-order
        private static readonly string englishChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //english in-order

        public TagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _context.Tags.OrderBy(tag => tag.Title).ToListAsync();

            var tagCatgegories = persianChars.ToCharArray().Concat(englishChars)
                .Select(c => new TagSortByAlphabet { Alphabet = c.ToString(), Tags = new List<Tag>() }).ToList();
            tagCatgegories.Add(new TagSortByAlphabet { Alphabet = "سایر", Tags = new List<Tag>() });

            foreach (var tag in tags)
            {
                var firstChar = char.Parse(tag.Title.Substring(0, 1));
                if (IsPersianChar(firstChar) || IsEnglishChar(firstChar))
                    tagCatgegories.Where(t => t.Alphabet == tag.Title.Substring(0, 1).ToUpper()).FirstOrDefault().Tags.Add(tag);
                else
                    tagCatgegories.Where(t => t.Alphabet == "سایر").FirstOrDefault().Tags.Add(tag);
            }

            tagCatgegories.RemoveAll(tc => tc.Tags.Count == 0);

            return View(tagCatgegories);
        }

        private static bool IsPersianChar(char character) => persianChars.IndexOf(character) != -1;

        private static bool IsEnglishChar(char character) => englishChars.IndexOf(Char.ToUpper(character)) != -1;

        [Route("add-tags")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.TagCreate)]
        public async Task<IActionResult> Create(string newTags)
        {
            try
            {
                //CheckNewTagsViolations
                if (string.IsNullOrEmpty(newTags) || string.IsNullOrEmpty(newTags.Trim())) throw new ArgumentException("برچسبی برای افزودن وارد نشد.");

                var tagsArray = newTags.Split(",").Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                if (tagsArray.Length == 0) throw new ArgumentException("برچسبی برای افزودن وارد نشد.");
                //CheckNewTagsViolations



                //check tag existence in database
                var toCreateTags = tagsArray.Except(await _context.Tags.Select(tag => tag.Title).Where(tagTitle => tagsArray.Contains(tagTitle)).ToArrayAsync())
                    .Distinct().Select(tag => new Tag { Title = tag }).ToList();
                //check tag existence in database



                //add new tags to database
                await _context.Tags.AddRangeAsync(toCreateTags);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        [Route("{id}/delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.TagDelete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var tag = await _context.Tags.Where(tag => tag.Id == id).FirstOrDefaultAsync();
                if (tag == null) return NotFound();

                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        [Route("{id}/edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.TagEdit)]
        public async Task<IActionResult> Edit(int id, string newName)
        {
            try
            {
                var tag = await _context.Tags.Where(tag => tag.Id == id).FirstOrDefaultAsync();
                if (tag == null) return NotFound();

                var oldName = tag.Title;
                tag.Title = newName;
                _context.Tags.Update(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(RelatedPosts), new { id });
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        
        [Route("{id}/related-posts")]
        public async Task<IActionResult> RelatedPosts(int id)
        {
            try
            {
                var tag = await _context.Tags.Where(tag => tag.Id == id).FirstOrDefaultAsync();
                if (tag == null) return NotFound();



                //return related posts
                return View(new RelatedPostsViewModel
                {
                    Posts = await _context.PostTags
                        .Where(pt => pt.TagId == tag.Id)
                        .Include(pt => pt.Post).ThenInclude(p => p.Article)
                        .Include(pt => pt.Post).ThenInclude(p => p.Share)
                        .Include(pt => pt.Post).ThenInclude(p => p.Markdown)
                        .Select(pt => new PostSummaryViewModel
                        {
                            Id = pt.Post.Id,
                            Title = pt.Post.Title,
                            Summary = pt.Post.Summary,
                            PostUrl = PostsController.GetPostUrl(pt.Post),
                            ThumbnailUrl = pt.Post.ThumbnailUrl,
                            PublishDateTime = pt.Post.PublishDateTime,
                            LatestUpdateDateTime = pt.Post.LatestUpdateDateTime,
                            Type = PostsController.DetectPostType(pt.Post),
                        })
                        .ToListAsync(),
                    Tag = tag
                });
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        [Route("suggestions")]
        public async Task<List<Tag>> Suggestion(string q, int count = 3)
        {
            if (string.IsNullOrEmpty(q)) return null;
            return await _context.Tags
                .Where(tag => tag.Title.Contains(q))
                .OrderByDescending(tag => tag.Title == q)
                .ThenByDescending(tag => tag.Title.Contains($"{q} "))
                .ThenByDescending(tag => tag.Title.Contains($" {q} "))
                .ThenByDescending(tag => tag.Title.Contains($" {q}"))
                .ThenBy(tag => tag.Title.Substring(0, q.Length)).Take(count).ToListAsync();
            
        }
    }
}
