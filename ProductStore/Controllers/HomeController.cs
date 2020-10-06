using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductStore.Data;
using ProductStore.Models;

namespace ProductStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AuthDbContext _context;

        public HomeController(AuthDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Users;
            // Logging levels
            //_logger.LogTrace("This is our first logged message Trace");
            //_logger.LogDebug("This is our first logged message Debug");
            //_logger.LogInformation("This is our first logged message Information");
            //_logger.LogWarning("This is our first logged message Warning");
            //_logger.LogError("This is our first logged message Error");
            //_logger.LogCritical("This is our first logged message Critical");


            return View();
        }

        [Authorize]
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
