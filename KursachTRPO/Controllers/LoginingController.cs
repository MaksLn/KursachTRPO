using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KursachTRPO.Controllers
{
    public class LoginingController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public string Login(string Login, string Password)
        {
            return Login+" "+Password;
        }

    }
}