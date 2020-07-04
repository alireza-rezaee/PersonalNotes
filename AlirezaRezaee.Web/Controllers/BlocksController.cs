using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Markdig.Syntax;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Models;
using Rezaee.Alireza.Web.Models.ViewModels.Blocks;
using Block = Rezaee.Alireza.Web.Models.Block;

namespace Rezaee.Alireza.Web.Controllers
{
    [Route("blocks")]
    public class BlocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlocksController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// افزودن بلاک
        /// </summary>
        /// <param name="blockVM">مدلی از یک بلاک</param>
        /// <returns></returns>
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] CreateEditViewModel createVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ///ERROR
                    await _context.AddAsync(new Block
                    {
                        Html = createVM.Html,
                        IsEnable = createVM.IsEnable,
                        Position = (BlockPosition)createVM.PositionNo,
                        Rank = createVM.Rank
                    });
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return BadRequest(createVM);
        }

        /// <summary>
        /// حذف بلاک
        /// </summary>
        /// <param name="id">شناسه بلاک</param>
        [Route("remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var block = await _context.Blocks.FindAsync(id);
            if (block == null) return NotFound();

            _context.Remove(block);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// اصلاح بلاک
        /// </summary>
        /// <param name="id">شناسه بلاک</param>
        /// <param name="editVM">مدلی از بلاک اصلاح شده</param>
        /// <returns></returns>
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id, CreateEditViewModel editVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var block = await _context.Blocks.FindAsync(id);
                    if (block == null) return NotFound();

                    _context.Update(new Block
                    {
                        Id = id,
                        Html = editVM.Html,
                        IsEnable = editVM.IsEnable,
                        Position = (BlockPosition)editVM.PositionNo
                    });
                    await _context.SaveChangesAsync();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return BadRequest(editVM);
        }

        /// <summary>
        /// فعال کردن بلاک
        /// </summary>
        /// <param name="id">شناسه بلاک</param>
        /// <returns></returns>
         [Route("enable/{id}")]
       public async Task<IActionResult> Enable(int id)
        {
            try
            {
                var block = await _context.Blocks.FindAsync(id);
                if (block == null) return NotFound();

                if (block.IsEnable)
                    return new ContentResult { StatusCode = 208, Content = $"این بلاک پیش از این نیز فعال بود. آیا آنرا غیر فعال می کنید؟" };

                _context.Update(new Block
                {
                    Id = id,
                    Html = block.Html,
                    IsEnable = true,
                    Position = block.Position
                });
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// غیرفعال کردن بلاک
        /// </summary>
        /// <param name="id">شناسه بلاک</param>
        /// <returns></returns>
        [Route("disable/{id}")]
        public async Task<IActionResult> Disable(int id)
        {
            try
            {
                var block = await _context.Blocks.FindAsync(id);
                if (block == null) return NotFound();

                if (!block.IsEnable)
                    return new ContentResult { StatusCode = 208, Content = $"این بلاک پیش از این نیز غیرفعال بود. آیا آنرا فعال می کنید؟" };

                _context.Update(new Block
                {
                    Id = id,
                    Html = block.Html,
                    IsEnable = false,
                    Position = block.Position
                });
                await _context.SaveChangesAsync();
                return Ok();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// دریافت فهرست بلاک ها
        /// </summary>
        /// <returns>فهرست بلاک ها</returns>
        [NonAction]
        public static async Task<List<Block>> GetBlocks(ApplicationDbContext context) => await context.Blocks.OrderBy(block => block.Position).ToListAsync();
    }
}
