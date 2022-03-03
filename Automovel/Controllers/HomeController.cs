using Automovel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Automovel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _loggeer;

        public HomeController(ILogger<HomeController> logger)
        {
            this._loggeer = logger;
        }

        public IActionResult Index()
        {
            string datetime = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            ViewBag.DataHora = datetime;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
