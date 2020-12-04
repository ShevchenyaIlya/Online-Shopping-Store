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
using ProductStore.Repositories;
using ProductStore.VIewModels;

namespace ProductStore.Controllers
{
    [Route("Home")]
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMarkRepository _markRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IProductBasketRepository _productBasketRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICategoryRepository categoryRepository,
            IUserRepository userRepository, IMarkRepository markRepository, ICommentRepository commentRepository, IOrderRepository orderRepository,
            ISaleRepository saleRepository, IProductBasketRepository productBasketRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _markRepository = markRepository;
            _commentRepository = commentRepository;
            _orderRepository = orderRepository;
            _saleRepository = saleRepository;
            _productBasketRepository = productBasketRepository;
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


            var products = await _productRepository.GetProductsDescendant();
            ViewData["categories"] = await _categoryRepository.GetCategories();
            return View(products);
        }

        [Authorize(Roles = "Admin")]
        [Route("Image")]
        public async Task<IActionResult> Image()
        {
            List<Product> value = (List<Product>)await _productRepository.GetProducts();
            ViewBag.image = value[0].ProductPicture;
            return View(value);
        }

        [Authorize]
        [Route("About")]
        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [Route("AddToProductBasketCookie/{value}")]
        public async Task<IActionResult> AddToProductBasketCookie(string value)
        {
            var product = await _productRepository.GetProductByName(value);
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
                
                foreach (var value in objCartListStringSplit)
                {
                    string[] result = value.Split("?");
                    var productList = (Product) await  _productRepository.GetProductByID(long.Parse(result[1]));
                    products.Add(productList);
                }
                products.Remove(await _productRepository.GetProductByName(product));
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
                
                foreach (var value in objCartListStringSplit)
                {
                    string[] result = value.Split("?");
                    var productList = (Product) await _productRepository.GetProductByID(long.Parse(result[1]));
                    if (productList.InStock)
                        products.Add(productList);
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
            Comments = (List<Comment>)await _commentRepository.GetProductsWhereName(productName);

            return PartialView("_DisplayComments", Comments);
        }

        [Route("OnUpdateRating")]
        public async Task<IActionResult> OnUpdateRating(string productName, string userId, string value)
        {
            var product = await _productRepository.GetProductByName(productName);

            if (userId != null)
            {
                //var user = await _context.Users.FirstOrDefaultAsync(mbox => mbox.Id == userId);
                var user = await _userRepository.GetUserById(userId);
                var findMark = await _markRepository.GetMarkByUserAndProduct(user, product);

                if (findMark == null)
                {
                    Mark mark = new Mark
                    {
                        User = user,
                        Product = product,
                        TotalMark = Int64.Parse(value)
                    };
                    _markRepository.InsertMark(mark);
                }
            }
            var rating = await _markRepository.GetMarkWhereId(product.ProductId);
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
                var product = await _productRepository.GetProductByName(productName);
                return RedirectToAction(nameof(ProductCard), new { id = product.ProductId });
            }

            var dataComment = new Comment();
            dataComment.CommentTitle = commentTitle;
            dataComment.CommentBody = commentBody;
            dataComment.PostDate = DateTime.UtcNow;
            dataComment.CommentUser = await _userRepository.GetUserByName(User.Identity.Name);
            dataComment.CommentProduct = await _productRepository.GetProductByName(productName);

            _commentRepository.InsertComment(dataComment);

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

            var product = await _productRepository.GetProductByID(id);

            ViewBag.Product = product;
            var sales = await _saleRepository.GetSales();
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

            var comments = await _commentRepository.GetProductsLimit(product.ProductId, 5);
            ViewBag.Comments = comments;

            if (product == null)
            {
                return NotFound();
            }

            var rating = await _markRepository.GetMarkWhereId(product.ProductId);
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

            var category = await _categoryRepository.GetCategorytByID(id);

            var products = await _productRepository.GetProductsWhereId(category.CategoryId);

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
            var products = await _productRepository.GetProducts();
            return View(products);
        }

        [AllowAnonymous]
        [Route("Sales")]
        public async Task<IActionResult> Sales()
        {
            var sales = await _saleRepository.GetSales();
            return View(sales);
        }

        [AllowAnonymous]
        [Route("Categories")]
        [Route("Category")]
        public async Task<IActionResult> Category()
        {
            var categories = await _categoryRepository.GetCategories();
            return View(categories);
        }

        [AllowAnonymous]
        [Route("CategoryItems/{id:int?}")]
        public async Task<IActionResult> CategoryItems(int? id)
        {
            var products = await _productRepository.GetProducts();
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
            ViewBag.Categories = await _categoryRepository.GetCategories();
            return View(await _productRepository.GetProducts());
        }

        [AllowAnonymous]
        [Route("SearchProduct")]
        public async Task<IActionResult> SearchProduct(string searching)
        {
            var products = await _productRepository.GetProducts();

            ViewBag.Categories = await _categoryRepository.GetCategories();
            if (!String.IsNullOrEmpty(searching))
            {
                products = products.Where(product => product.ProductName.Contains(searching) || product.CreatedPlace.CountryName.Contains(searching));
            }
            return PartialView("_DisplayProductPartial", products.ToList());
        }

        [AllowAnonymous]
        [Route("FindProductByCategory")]
        public async Task<IActionResult> FindProductByCategory(string category)
        {
            var products = await _productRepository.GetProducts();

            ViewBag.Categories = await _categoryRepository.GetCategories();
            products = products.Where(product => product.Category.CategoryName.Contains(category));
            return PartialView("_DisplayProductPartial", products.ToList());
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
            if (Request.Cookies["ProductBasket1"] != null)
            {
                return View();
            }
            return RedirectToAction(nameof(Basket));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder([Bind("OrderId,Addres,City,State,Country,ZipCode,AmountPaid,PaymentType,FormedDate")] Order order)
        {
            ApplicationUser temporalUser = await _userRepository.GetUserByName(User.Identity.Name);
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
            foreach (var value in objCartListStringSplit)
            {
                string[] result = value.Split("?");
                var productList = (Product)await _productRepository.GetProductByID(long.Parse(result[1]));
                if (productList.InStock)
                    products.Add(productList);
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
                _orderRepository.InsertOrder(order);
            }
            foreach(var product in products)
            {
                ProductBasket productBasket = new ProductBasket
                {
                    Customer = order.Customer,
                    Products = product,
                    Order = order,
                };
                _productBasketRepository.AddBasketToTransaction(productBasket);
            }
            _productBasketRepository.Save();
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
