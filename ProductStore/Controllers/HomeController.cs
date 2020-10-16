using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductStore.Data;
using ProductStore.Models;
using ProductStore.VIewModels;

namespace ProductStore.Controllers
{
    [Route("Home")]
    [Route("")]
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
        [Route("Index")]
        [Route("")]
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
        [Route("Image")]
        public async Task<IActionResult> Image()
        {
            var value = await _context.Products.ToListAsync();
            ViewBag.image = value[0].ProductPicture;
            return View(_context.Products.ToList());
        }

        [Authorize]
        [Route("About")]
        public async Task<IActionResult> About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Comment(CommentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //Mapping code - alternatively try AutoMapper
                var dataComment = new Comment();
                dataComment.CommentTitle = viewModel.Title;
                dataComment.CommentBody = viewModel.Body;
                dataComment.PostDate = DateTime.Now;
                dataComment.CommentUser = _context.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
                // Create comment and save changes
                _context.Add(dataComment);
                await _context.SaveChangesAsync();
                
            }

            return RedirectToAction(nameof(ProductCard));
        }

        [AllowAnonymous]
        [Route("ProductCard/{id:int?}")]
        public async Task<ActionResult> ProductCard(int? id, string text, string text1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(n => n.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            var sales = await _context.Sale.Include(m => m.Product).ToListAsync();
            Sale productSale = null;
            foreach(var sale in sales)
            {
                if (sale.Product == product)
                {
                    productSale = sale;
                    break;
                }
            }
            ViewBag.Sale = productSale;

            var comments = await _context.Comment.Include(m => m.CommentUser).Include(n => n.CommentProduct).Where(mbox => mbox.CommentProduct.ProductId == product.ProductId).ToListAsync();
            ViewBag.Comments = comments;
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [AllowAnonymous]
        [Route("CategoryCard/{id:int?}")]
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
        [Route("Products")]
        public async Task<IActionResult> Products()
        {
            var products = await _context.Products.ToListAsync<Product>();
            return View(products);
        }

        [AllowAnonymous]
        [Route("Sales")]
        public async Task<IActionResult> Sales()
        {
            var sales = await _context.Sale.Include(m => m.Product).ToListAsync<Sale>();
            return View(sales);
        }

        [AllowAnonymous]
        [Route("Categories")]
        [Route("Category")]
        public async Task<IActionResult> Category()
        {
            var categories = await _context.Category.ToListAsync<Category>();
            return View(categories);
        }

        [AllowAnonymous]
        [Route("CategoryItems/{id:int?}")]
        public async Task<IActionResult> CategoryItems(int? id)
        {
            var products = await _context.Products.ToListAsync<Product>();
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
        [Route("FindProduct")]
        public async Task<IActionResult> FindProduct(string searching)
        {
            var products = from product in _context.Products
                           select product;

            ViewBag.Categories = await _context.Category.ToListAsync();
            if (!String.IsNullOrEmpty(searching))
            {
                products = products.Where(product => product.ProductName.Contains(searching));
            }

            return View(products.ToList());
        }

        [Authorize]
        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
