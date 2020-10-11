using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            // Logging levels
            //_logger.LogTrace("This is our first logged message Trace");
            //_logger.LogDebug("This is our first logged message Debug");
            //_logger.LogInformation("This is our first logged message Information");
            //_logger.LogWarning("This is our first logged message Warning");
            //_logger.LogError("This is our first logged message Error");
            //_logger.LogCritical("This is our first logged message Critical");


            var products = await _context.Products.OrderByDescending(m => m.AddedDate).ToListAsync();
            ViewData["categories"] = _context.Category;
            return View(products);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Image()
        {
            ViewBag.image = _context.Products.ToList()[0].ProductPicture;
            return View(_context.Products.ToList());
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public async Task<ActionResult> ProductCard(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(n => n.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<ActionResult> CategoryCard(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            var products = await _context.Products.Where(m => m.Category.CategoryId == category.CategoryId).ToListAsync();
            ViewBag.Products = products;
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [AllowAnonymous]
        public ActionResult Products()
        {
            var products = _context.Products.ToList<Product>();
            return View(products);
        }
        [AllowAnonymous]
        public ActionResult Category()
        {
            var categories = _context.Category.ToList<Category>();
            return View(categories);
        }

        [AllowAnonymous]
        public ActionResult CategoryItems(int? id)
        {
            var products = _context.Products.ToList<Product>();
            List<Product> categoryProducts = new List<Product>();
            foreach (var product in products)
            {
                if (product.Category.CategoryId == id)
                {
                    categoryProducts.Add(product);
                }
            }

            ViewBag.categoryProducts = categoryProducts;

            return View(products);
        }

        [AllowAnonymous]
        public ActionResult FindProduct(string searching)
        {
            var products = from product in _context.Products
                           select product;

            if (!String.IsNullOrEmpty(searching))
            {
                products = products.Where(product => product.ProductName.Contains(searching));
            }

            return View(products.ToList());
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
