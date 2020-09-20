using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Markdig.Syntax;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Extensions;
using Rezaee.Alireza.Web.Helpers;
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
        /// صفحه افزودن بلاک
        /// </summary>
        /// <returns></returns>
        [HttpGet("create")]
        [Authorize(Roles = Roles.BlockCreate)]
        public IActionResult Create() => View();

        /// <summary>
        /// افزودن بلاک
        /// </summary>
        /// <param name="createVM">مدلی از یک بلاک</param>
        /// <param name="returnUrl">نشانی بازگشت</param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = Roles.BlockCreate)]
        public async Task<IActionResult> Create(CreateEditViewModel createVM, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(createVM.Html) && string.IsNullOrEmpty(createVM.Styles) && string.IsNullOrEmpty(createVM.Scripts))
                    return BadRequest();

                try
                {
                    await _context.AddAsync(new Block
                    {
                        Html = createVM.Html,
                        IsEnable = createVM.IsEnable,
                        Position = (BlockPosition)createVM.PositionNo,
                        Rank = createVM.Rank,
                        Styles = createVM.Styles,
                        Scripts = createVM.Scripts
                    });
                    await _context.SaveChangesAsync();

                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).ControllerName());
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return RedirectToAction(nameof(Create), nameof(BlocksController).ControllerName(), createVM);
        }

        /// <summary>
        /// اصلاح بلاک
        /// </summary>
        /// <returns></returns>
        [HttpGet("edit/{id}")]
        [Authorize(Roles = Roles.BlockEdit)]
        public async Task<IActionResult> Edit(int id)
        {
            var block = await _context.Blocks.FindAsync(id);
            if (block == null)
                return NotFound();

            return View(new CreateEditViewModel
            {
                Id = id,
                Html = block.Html,
                Styles = block.Styles,
                Scripts = block.Scripts,
                PositionNo = (byte)block.Position,
                Rank = block.Rank,
                IsEnable = block.IsEnable
            });
        }

        /// <summary>
        /// اصلاح بلاک
        /// </summary>
        /// <param name="id">شناسه بلاک</param>
        /// <param name="editVM">مدلی از بلاک اصلاح شده</param>
        /// <param name="returnUrl">نشانی بازگشت</param>
        [HttpPost("edit/{id}")]
        [Authorize(Roles = Roles.BlockEdit)]
        public async Task<IActionResult> Edit(int id, CreateEditViewModel editVM, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var block = await _context.Blocks.FindAsync(id);
                if (block == null)
                    return NotFound();

                if (string.IsNullOrEmpty(editVM.Html) && string.IsNullOrEmpty(editVM.Styles) && string.IsNullOrEmpty(editVM.Scripts))
                    return BadRequest();

                try
                {
                    block.Html = editVM.Html;
                    block.Styles = editVM.Styles;
                    block.Scripts = editVM.Scripts;
                    block.IsEnable = editVM.IsEnable;
                    block.Rank = editVM.Rank;
                    block.Position = (BlockPosition)editVM.PositionNo;

                    _context.Update(block);
                    await _context.SaveChangesAsync();

                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).ControllerName());
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return RedirectToAction(nameof(Edit), nameof(BlocksController).ControllerName(), editVM);
        }

        /// <summary>
        /// حذف بلاک
        /// </summary>
        /// <param name="id">شناسه بلاک</param>
        [Route("remove/{id}")]
        [Authorize(Roles = Roles.BlockDelete)]
        public async Task<IActionResult> Delete(int id, string returnUrl)
        {
            try
            {
                var block = await _context.Blocks.FindAsync(id);
                if (block == null) return NotFound();

                _context.Remove(block);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).ControllerName());
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// فعال کردن بلاک
        /// </summary>
        /// <param name="id">شناسه بلاک</param>
        /// <returns></returns>
        [Route("enable/{id}")]
        [Authorize(Roles = Roles.BlockVisibility)]
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
        [Authorize(Roles = Roles.BlockVisibility)]
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
