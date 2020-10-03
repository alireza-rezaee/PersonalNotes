using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Extensions;
using Rezaee.Alireza.Web.Helpers;
using Rezaee.Alireza.Web.Models;

namespace Rezaee.Alireza.Web.Controllers
{
    [Route("messages")]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly IFileManager _fileManager;

        public MessagesController(ApplicationDbContext context/*, IFileManager fileManager*/)
        {
            _context = context;
            //_fileManager = fileManager;
        }

        [Route("")]
        [Authorize(Roles = Roles.MessagesList)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Messages.OrderBy(message => message.CreateDateTime).ToListAsync());
        }

        [HttpGet("/contact-us")]
        public IActionResult ContactUs() => View();

        [HttpPost("/contact-us")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactingUs(Message model)
        {
            if (!ModelState.IsValid)
            {
                //TODO: Notify the user (Sender). Please send the reason!
                return View(nameof(ContactUs), model);
            }

            try
            {
                model.CreateDateTime = DateTime.Now;
                await _context.Messages.AddAsync(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //TODO: Notify the user. Please send the reason!
                throw e;
            }

            //TODO: Let the user know that this process has been completed successfully
            //TODO: Notify the user (Receiver) that a new message is in the inbox!
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).ControllerName());
        }

        [Route("{id}/show")]
        [Authorize(Roles = Roles.MessagesList)]
        public async Task<IActionResult> Details(int id)
        {
            Message message;
            try
            {
                message = await _context.Messages.FindAsync(id);
                if (message == null) return NotFound();

                message.HaveRead = true;
                _context.Update(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //TODO: Notify the user. Please send the reason!
                throw e;
            }

            return View(message);
        }

        [HttpPost("{id}/delete")]
        [Authorize(Roles = Roles.MessageDelete)]
        public async Task<IActionResult> Delete(int id)
        {
            Message message;
            try
            {
                message = await _context.Messages.FindAsync(id);
                if (message == null) return NotFound();

                _context.Remove(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //TODO: Notify the user. Please send the reason!
                throw e;
            }

            return RedirectToAction(nameof(Index), nameof(MessagesController).ControllerName());
        }

        [HttpPost("{id}/read/{mode:length(3,5)}")]
        public async Task<IActionResult> HaveRead(int id, string mode)
        {
            bool setAsRead;
            Message message;
            //Find mission (user wants to message as read or unread)
            if (mode == "set") setAsRead = true;
            else if (mode == "unset") setAsRead = false;
            else return NotFound();

            try
            {
                message = await _context.Messages.FindAsync(id);
                if (message == null) return NotFound();

                message.HaveRead = setAsRead;
                _context.Update(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction(nameof(Index), nameof(MessagesController).ControllerName());
        }
    }
}
