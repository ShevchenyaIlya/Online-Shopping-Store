using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using Microsoft.AspNetCore.Authorization;
using ProductStore.Models;

namespace ProductStore.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Route("Admin/Ratings")]
    [Route("Admin/Rating")]
    public class RatingsController : Controller
    {
        private readonly AuthDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public RatingsController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: Ratings
        [Route("Index")]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.StatusMessage = StatusMessage;
            return View(await _context.Rating.Include(m => m.Product).Include(m => m.UserMarks).Include(m => m.UserMarks.User).Include(m => m.UserMarks.Product).ToListAsync());
        }

        // GET: Ratings/Details/5
        [Route("Details/{id:int:min(1)?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating.Include(m => m.Product).Include(m => m.Product).Include(m => m.UserMarks).Include(m => m.UserMarks.User).Include(m => m.UserMarks.Product)
                .FirstOrDefaultAsync(m => m.RatingId == id);
            if (rating == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(rating);
        }

        // GET: Ratings/Create
        [Route("Create")]
        public IActionResult Create()
        {
            List<SelectListItem> mark = new List<SelectListItem>();
            foreach (var markValue in _context.Mark)
            {
                mark.Add(new SelectListItem() { Value = markValue.TotalMark.ToString(), Text = markValue.TotalMark.ToString() });
            }
            ViewBag.Mark = mark;

            List<SelectListItem> product = new List<SelectListItem>();
            foreach (var productValue in _context.Products)
            {
                product.Add(new SelectListItem() { Value = productValue.ProductName, Text = productValue.ProductName });
            }
            ViewBag.Product = product;

            ViewBag.StatusMessage = StatusMessage;
            return View();
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("RatingId")] Rating rating, string product, float userMarks)
        {
            List<SelectListItem> loadMark = new List<SelectListItem>();
            foreach (var markValue in _context.Mark)
            {
                loadMark.Add(new SelectListItem() { Value = markValue.TotalMark.ToString(), Text = markValue.TotalMark.ToString() });
            }
            ViewBag.Mark = loadMark;

            List<SelectListItem> loadProduct = new List<SelectListItem>();
            foreach (var productValue in _context.Products)
            {
                loadProduct.Add(new SelectListItem() { Value = productValue.ProductName, Text = productValue.ProductName });
            }
            ViewBag.Product = loadProduct;

            Mark findUser = _context.Mark.Where(value => value.TotalMark == userMarks).First();
            Product findProduct = _context.Products.Where(value => value.ProductName == product).First();

            if (findUser != null && findProduct != null)
            {
                rating.UserMarks = findUser;
                rating.Product = findProduct;
            }
            else
            {
                StatusMessage = "Not all properties choosen";
                return RedirectToAction();
            }

            if (ModelState.IsValid)
            {
                _context.Add(rating);
                await _context.SaveChangesAsync();
                StatusMessage = "Rating has been added";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Error. Invalid form.";
            return View(rating);
        }

        // GET: Ratings/Edit/5
        [Route("Edit/{id:int:min(1)?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SelectListItem> mark = new List<SelectListItem>();
            foreach (var markValue in _context.Mark)
            {
                mark.Add(new SelectListItem() { Value = markValue.TotalMark.ToString(), Text = markValue.TotalMark.ToString() });
            }
            ViewBag.Mark = mark;

            List<SelectListItem> product = new List<SelectListItem>();
            foreach (var productValue in _context.Products)
            {
                product.Add(new SelectListItem() { Value = productValue.ProductName, Text = productValue.ProductName });
            }
            ViewBag.Product = product;

            var rating = await _context.Rating.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }
            ViewBag.StatusMessage = StatusMessage;
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:int:min(1)}")]
        public async Task<IActionResult> Edit(int id, [Bind("RatingId")] Rating rating, string product, float userMarks)
        {
            List<SelectListItem> loadMark = new List<SelectListItem>();
            foreach (var markValue in _context.Mark)
            {
                loadMark.Add(new SelectListItem() { Value = markValue.TotalMark.ToString(), Text = markValue.TotalMark.ToString() });
            }
            ViewBag.Mark = loadMark;

            List<SelectListItem> loadProduct = new List<SelectListItem>();
            foreach (var productValue in _context.Products)
            {
                loadProduct.Add(new SelectListItem() { Value = productValue.ProductName, Text = productValue.ProductName });
            }
            ViewBag.Product = loadProduct;

            if (id != rating.RatingId)
            {
                return NotFound();
            }
            Mark findUser = _context.Mark.Where(value => value.TotalMark == userMarks).First();
            Product findProduct = _context.Products.Where(value => value.ProductName == product).First();

            if (findUser != null && findProduct != null)
            {
                rating.UserMarks = findUser;
                rating.Product = findProduct;
            }
            else
            {
                StatusMessage = "Not all properties choosen";
                return RedirectToAction();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RatingExists(rating.RatingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "Rating has been updated";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Error. Invalid form.";
            return View(rating);
        }

        // GET: Ratings/Delete/5
        [Route("Delete/{id:int:min(1)?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating
                .FirstOrDefaultAsync(m => m.RatingId == id);
            if (rating == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:int:min(1)}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rating = await _context.Rating.FindAsync(id);
            _context.Rating.Remove(rating);
            await _context.SaveChangesAsync();
            StatusMessage = "Raiting deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool RatingExists(int id)
        {
            return _context.Rating.Any(e => e.RatingId == id);
        }
    }
}
