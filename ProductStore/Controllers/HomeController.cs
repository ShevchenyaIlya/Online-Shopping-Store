using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductStore.Areas.Identity.Data;
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

        [Route("AddToProductBasketCookie/{value}")]
        public async Task<IActionResult> AddToProductBasketCookie(string value)
        {
            var product = await _context.Products.Include(m => m.Category).Include(m => m.CreatedPlace).FirstOrDefaultAsync(mbox => mbox.ProductName == value);
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(30);
            if (Request.Cookies["ProductBasket1"] == null)
                Response.Cookies.Append("ProductBasket1", product.ProductName + "?" + product.ProductId, option);
            else
                Response.Cookies.Append("ProductBasket1", Request.Cookies["ProductBasket1"].ToString() + "|" + product.ProductName + "?" + product.ProductId, option);

            return RedirectToAction(nameof(Products));
        }

        [Route("RemoveFromProductBasketCookie/{product}")]
        public async Task<IActionResult> RemoveFromProductBasketCookie(string product)
        {
            if (Request.Cookies["ProductBasket1"] != null)
            {
                string objCartListString = Request.Cookies["ProductBasket1"];
                string[] objCartListStringSplit = objCartListString.Split('|');
                List<Product> products = new List<Product>();
                var productList = await _context.Products.ToListAsync();
                foreach (var value in objCartListStringSplit)
                {
                    string[] result = value.Split("?");
                    products.Add(productList.Find(mbox => mbox.ProductId == long.Parse(result[1])));
                }
                products.Remove(await _context.Products.FirstOrDefaultAsync(mbox => mbox.ProductName == product));
                Response.Cookies.Delete("ProductBasket1");
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddMinutes(30);
                string cookiesString = "";
                foreach (var value in products)
                {
                    cookiesString += value.ProductName + "?" + value.ProductId + "|";
                }
                cookiesString = cookiesString.Remove(cookiesString.Length - 1);
                Response.Cookies.Append("ProductBasket1", cookiesString, option);
            }
            return RedirectToAction(nameof(Basket));
        }

        [Route("Basket")]
        public async Task<IActionResult> Basket()
        {
            if (Request.Cookies["ProductBasket1"] != null)
            {
                string objCartListString = Request.Cookies["ProductBasket1"];
                string[] objCartListStringSplit = objCartListString.Split('|');
                List<Product> products = new List<Product>();
                var productList = await _context.Products.Include(m => m.Category).Include(m => m.CreatedPlace).ToListAsync();
                foreach (var value in objCartListStringSplit)
                {
                    string[] result = value.Split("?");
                    products.Add(productList.Find(mbox => mbox.ProductId == long.Parse(result[1])));
                }
                return View(products);
            }
            return View();
        }
        
        [BindProperty]
        public List<Comment> Comments { get; set; }
        
        [Route("OnGetAllCommentAsync")]
        public async Task<IActionResult> OnGetAllCommentAsync(string productName)
        {
            Comments = await _context.Comment
                .Include(m => m.CommentUser).Include(n => n.CommentProduct)
                .Where(m => m.CommentProduct.ProductName == productName).OrderByDescending(m => m.PostDate).ToListAsync();

            return PartialView("_DisplayComments", Comments);
        }

        [Route("OnUpdateRating")]
        public async Task<IActionResult> OnUpdateRating(string productName, string userId, string value)
        {
            var product = await _context.Products.FirstOrDefaultAsync(mbox => mbox.ProductName == productName);

            if (userId != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(mbox => mbox.Id == userId);
                var findMark = await _context.Mark.Include(m => m.User).Include(m => m.Product).FirstOrDefaultAsync(mbox => mbox.User == user && mbox.Product == product);

                if (findMark == null)
                {
                    Mark mark = new Mark
                    {
                        User = user,
                        Product = product,
                        TotalMark = Int64.Parse(value)
                    };
                    _context.Add(mark);
                    await _context.SaveChangesAsync();
                }
            }
            var rating = await _context.Mark.Where(m => m.Product.ProductId == product.ProductId).ToListAsync();
            double ratingValue = 0;
            foreach (var variable in rating)
            {
                ratingValue += variable.TotalMark;
            }
            if (rating.Count() != 0)
            {
                ratingValue /= rating.Count();
            }

            int fullStars = (int)Math.Truncate(ratingValue);
            RatingViewModel ratingViewModel = new RatingViewModel
            {
                MarkCount = rating.Count(),
                FullStars = fullStars,
                HalfStar = (ratingValue - fullStars) >= 0.5,
                EmptyStart = (int)Math.Round((5 - ratingValue)),
                TotalRating = ratingValue
            };

            return PartialView("_DisplayRatingPartial", ratingViewModel);
        }

        [HttpPost]
        [Route("Comment")]
        public async Task<ActionResult> Comment(string commentTitle, string commentBody, string productName)
        {
            if (productName == null)
            {
                return RedirectToAction(nameof(Products));
            }

            if (commentTitle == null || commentBody == null)
            {
                var product = await _context.Products.FirstOrDefaultAsync(mbox => mbox.ProductName == productName);
                return RedirectToAction(nameof(ProductCard), new { id = product.ProductId });
            }

            var dataComment = new Comment();
            dataComment.CommentTitle = commentTitle;
            dataComment.CommentBody = commentBody;
            dataComment.PostDate = DateTime.Now;
            dataComment.CommentUser = await _context.Users.FirstOrDefaultAsync(m => m.UserName == User.Identity.Name);
            dataComment.CommentProduct = await _context.Products.FirstOrDefaultAsync(mbox => mbox.ProductName == productName);

            _context.Add(dataComment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ProductCard), new { id = dataComment.CommentProduct.ProductId });
        }

        [AllowAnonymous]
        [Route("ProductCard/{id:long?}")]
        public async Task<ActionResult> ProductCard(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(n => n.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            ViewBag.Product = product;
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

            var comments = await _context.Comment.Include(m => m.CommentUser).Include(n => n.CommentProduct)
                .Where(mbox => mbox.CommentProduct.ProductId == product.ProductId).OrderByDescending(m => m.PostDate).Take(5).ToListAsync();
            ViewBag.Comments = comments;

            if (product == null)
            {
                return NotFound();
            }

            var rating = await _context.Mark.Where(m => m.Product.ProductId == product.ProductId).ToListAsync();
            double ratingValue = 0;
            foreach (var value in rating)
            {
                ratingValue += value.TotalMark;
            }
            if (rating.Count() != 0)
            {
                ratingValue /= rating.Count();
            }

            int fullStars = (int)Math.Truncate(ratingValue);
            RatingViewModel ratingViewModel = new RatingViewModel
            {
                MarkCount = rating.Count(),
                FullStars = fullStars,
                HalfStar = (ratingValue - fullStars) >= 0.5,
                EmptyStart = (int)Math.Round((5 - ratingValue)),
                TotalRating = ratingValue
            };
            ViewBag.Rating = ratingViewModel;

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

        [Authorize]
        [Route("CreateOrder")]
        public IActionResult CreateOrder()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder([Bind("OrderId,Addres,City,State,Country,ZipCode,AmountPaid,PaymentType,FormedDate")] Order order)
        {
            ApplicationUser temporalUser = _context.Users.Where(value => value.UserName == User.Identity.Name).First();
            if (temporalUser != null)
            {
                order.Customer = temporalUser;
            }
            else
            {
                return RedirectToAction();
            }

            string objCartListString = Request.Cookies["ProductBasket1"];
            string[] objCartListStringSplit = objCartListString.Split('|');
            List<Product> products = new List<Product>();
            var productList = await _context.Products.Include(m => m.Category).Include(m => m.CreatedPlace).ToListAsync();
            foreach (var value in objCartListStringSplit)
            {
                string[] result = value.Split("?");
                products.Add(productList.Find(mbox => mbox.ProductId == long.Parse(result[1])));
            }
            double amountPaid = 0;

            foreach(var value in products)
            {
                amountPaid += value.Price;
            }

            order.FormedDate = DateTime.Now;
            order.AmountPaid = amountPaid;
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
            }
            foreach(var product in products)
            {
                ProductBasket productBasket = new ProductBasket
                {
                    Customer = order.Customer,
                    Products = product,
                    Order = order,
                };
                _context.Add(productBasket);
            }
            await _context.SaveChangesAsync();
            Response.Cookies.Delete("ProductBasket1");
            return RedirectToAction(nameof(Products));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
