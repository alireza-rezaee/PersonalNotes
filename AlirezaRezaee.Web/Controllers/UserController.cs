﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AlirezaRezaee.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index(string username)
        {
            ViewData["Title"] = username;
            return View();
        }
    }
}