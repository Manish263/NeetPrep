﻿using Microsoft.AspNetCore.Mvc;

namespace Pdf.API.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
