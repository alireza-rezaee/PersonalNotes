using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlirezaRezaee.PersonalNotes.WeblogApp.Data;
using AlirezaRezaee.PersonalNotes.WeblogApp.Helpers;
using AlirezaRezaee.PersonalNotes.WeblogApp.Models;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Controllers
{
    [Route("pins")]
    public class PinsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PinsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// پین کردن مطالب سایت
        /// </summary>
        /// <param name="id">شناسه مطلب</param>
        [Route("pin/{id}")]
        [Authorize(Roles = Roles.Pin)]
        public async Task<IActionResult> Pin(int id)
        {
            var post = await _context.Posts.Where(post => post.Id == id).Include(post => post.Pin).FirstOrDefaultAsync();
            if (post == null)
                return NotFound();

            if (post.Pin != null)
                return new ContentResult { StatusCode = 208, Content = $"مطلب «{post.Title}» با شناسه {post.Id}، پیش از این پین شده بود. آیا پین را میخواهید بردارید؟" };

            post.Pin = new Pin { PostId = post.Id };
            _context.Update(post);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// برداشتن پین مطلب
        /// </summary>
        /// <param name="id">شناسه مطلب</param>
        [Route("un-pin/{id}")]
        [Authorize(Roles = Roles.Pin)]
        public async Task<IActionResult> UnPin(int id)
        {
            var post = await _context.Posts.Where(post => post.Id == id).Include(post => post.Pin).FirstOrDefaultAsync();
            if (post == null)
                return NotFound();

            if (post.Pin == null)
                return new ContentResult { StatusCode = 208, Content = $"مطلب «{post.Title}» با شناسه {post.Id}، پیش از این فاقد پین بود. آیا برای آن، پین میخواهید قرار دهید؟", ContentType = "text/plain" };

            _context.Remove(post.Pin);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
