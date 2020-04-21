using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Models;
using Rezaee.Alireza.Web.Models.ViewModels.Recommendeds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Rezaee.Alireza.Web.Controllers
{
    public class RecommendationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecommendationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var recommendations = await _context.Recommendeds.ToListAsync();

                var recommendation = recommendations.Where(r => r.PostId == id).FirstOrDefault();
                if (recommendation == null)
                    return NotFound();

                _context.Remove(recommendation);
                await _context.SaveChangesAsync();

                return StatusCode(200, "Deletion Successfull");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<RecommendedPostsViewModel>> List(int counts = 10)
        {
            if (counts < 1)
                return null;

            try
            {
                return await _context.Recommendeds.Include(p => p.Post).OrderBy(p => p.Rank).Include(p => p.Post).ThenInclude(p => p.Article).Select(p => new RecommendedPostsViewModel
                {
                    Id = p.PostId,
                    Rank = p.Rank,
                    Title = p.Post.Title,
                    Url = "/" + p.Post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{p.PostId}/{p.Post.Article.UrlTitle}"
                }).Take(10).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> ChangeRank(int id, int currentRank, bool isUpgrade)
        {
            try
            {
                var recommendationSource = await _context.Recommendeds.FirstOrDefaultAsync(r => r.PostId == id && r.Rank == currentRank);
                if (recommendationSource == null)
                    return NotFound();

                if (!isUpgrade || recommendationSource.Rank > 1)
                {
                    var recommendationTarget = await _context.Recommendeds.FirstOrDefaultAsync(r => r.Rank == recommendationSource.Rank + (isUpgrade ? -1 : +1));
                    if (recommendationTarget == null)
                    {
                        recommendationSource.Rank += (isUpgrade ? -1 : +1);
                        _context.Update(recommendationSource);
                    }
                        
                    else
                    {
                        (recommendationSource.Rank, recommendationTarget.Rank) = (recommendationTarget.Rank, recommendationSource.Rank);
                        _context.UpdateRange(new List<Recommendeds> { recommendationSource, recommendationTarget });
                    }
                }
                else
                    return BadRequest();

                await _context.SaveChangesAsync();
                return StatusCode(200);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}