using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Models;

namespace Rezaee.Alireza.Web.Controllers
{

    [Route("posters")]
    public class PosterpinsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PosterpinsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// پوستر-پین کردن مطالب سایت
        /// </summary>
        /// <param name="id">شناسه مطلب</param>
        /// <returns></returns>
        [Route("pin/{id}")]
        public async Task<IActionResult> Pin(int id)
        {
            var post = await _context.Posts.Where(post => post.Id == id).Include(post => post.Posterpins).FirstOrDefaultAsync();
            if (post == null)
                return NotFound();

            if (post.Posterpins != null)
                return new ContentResult
                {
                    StatusCode = 208,
                    Content = $"مطلب «{post.Title}» با شناسه {post.Id}، پیش از این به عنوان پوستر پین شده بود. آیا پین را میخواهید بردارید؟",
                    ContentType = "text/plain"
                };

            post.Posterpins = new Posterpins { PostId = post.Id };
            //need to completely fill Posterpins

            _context.Update(post);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// برداشتن پوستر-پین مطلب
        /// </summary>
        /// <param name="id">شناسه مطلب</param>
        [Route("un-pin/{id}")]
        public async Task<IActionResult> UnPin(int id)
        {
            var post = await _context.Posts.Where(post => post.Id == id).Include(post => post.Posterpins).FirstOrDefaultAsync();
            if (post == null)
                return NotFound();

            if (post.Posterpins == null)
                return new ContentResult
                {
                    StatusCode = 208,
                    Content = $"مطلب «{post.Title}» با شناسه {post.Id}، پیش از این فاقد پوستر پین بود. آیا برای آن، پین میخواهید قرار دهید؟",
                    ContentType = "text/plain"
                };

            _context.Remove(post.Posterpins);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
