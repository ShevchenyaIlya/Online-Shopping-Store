using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ProductStore.Data;
using ProductStore.Models;
using ProductStore.Areas.Identity.Data;

namespace ProductStore.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class MarksController : Controller
    {
        private readonly AuthDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public MarksController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: Marks
        public async Task<IActionResult> Index()
        {
            ViewBag.StatusMessage = StatusMessage;
            return View(await _context.Mark.Include(n => n.Product).Include(n => n.User).ToListAsync());
        }

        // GET: Marks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Mark.Include(n => n.Product).Include(n => n.User)
                .FirstOrDefaultAsync(m => m.MarkId == id);
            if (mark == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(mark);
        }

        // GET: Marks/Create
        public IActionResult Create()
        {
            List<SelectListItem> users = new List<SelectListItem>();
            foreach (var user in _context.Users)
            {
                users.Add(new SelectListItem() { Value = user.UserName, Text = user.UserName });
            }
            ViewBag.Users = users;

            List<SelectListItem> products = new List<SelectListItem>();
            foreach (var product in _context.Products)
            {
                products.Add(new SelectListItem() { Value = product.ProductName, Text = product.ProductName });
            }
            ViewBag.Products = products;

            ViewBag.StatusMessage = StatusMessage;
            return View();
        }

        // POST: Marks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MarkId,TotalMark")] Mark mark, string product, string user)
        {

            List<SelectListItem> users = new List<SelectListItem>();
            foreach (var userValue in _context.Users)
            {
                users.Add(new SelectListItem() { Value = userValue.UserName, Text = userValue.UserName });
            }
            ViewBag.Users = users;

            List<SelectListItem> products = new List<SelectListItem>();
            foreach (var productValue in _context.Products)
            {
                products.Add(new SelectListItem() { Value = productValue.ProductName, Text = productValue.ProductName });
            }
            ViewBag.Products = products;

            if (mark.TotalMark > 5 || mark.TotalMark < 0)
            {
                StatusMessage = "Error. Invalid range for total mark.";
                return RedirectToAction();
            }
            ApplicationUser findUser = _context.Users.Where(value => value.UserName == user).First();
            Product findProduct = _context.Products.Where(value => value.ProductName == product).First();

            if (findUser != null && findProduct != null)
            {
                mark.User = findUser;
                mark.Product = findProduct;
            }
            else
            {
                StatusMessage = "Not all properties choosen";
                return RedirectToAction();
            }

            if (ModelState.IsValid)
            {
                _context.Add(mark);
                await _context.SaveChangesAsync();
                StatusMessage = "Mark has been added";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Error. Invalid form.";
            return View(mark);
        }

        // GET: Marks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SelectListItem> users = new List<SelectListItem>();
            foreach (var userValue in _context.Users)
            {
                users.Add(new SelectListItem() { Value = userValue.UserName, Text = userValue.UserName });
            }
            ViewBag.Users = users;

            List<SelectListItem> products = new List<SelectListItem>();
            foreach (var productValue in _context.Products)
            {
                products.Add(new SelectListItem() { Value = productValue.ProductName, Text = productValue.ProductName });
            }
            ViewBag.Products = products;

            var mark = await _context.Mark.FindAsync(id);
            if (mark == null)
            {
                return NotFound();
            }
            ViewBag.StatusMessage = StatusMessage;
            return View(mark);
        }

        // POST: Marks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MarkId,TotalMark")] Mark mark, string product, string user)
        {
            if (id != mark.MarkId)
            {
                return NotFound();
            }

            List<SelectListItem> users = new List<SelectListItem>();
            foreach (var userValue in _context.Users)
            {
                users.Add(new SelectListItem() { Value = userValue.UserName, Text = userValue.UserName });
            }
            ViewBag.Users = users;

            List<SelectListItem> products = new List<SelectListItem>();
            foreach (var productValue in _context.Products)
            {
                products.Add(new SelectListItem() { Value = productValue.ProductName, Text = productValue.ProductName });
            }
            ViewBag.Products = products;

            if (mark.TotalMark > 5 || mark.TotalMark < 0)
            {
                StatusMessage = "Error. Invalid range for total mark.";
                return RedirectToAction();
            }

            ApplicationUser findUser = _context.Users.Where(value => value.UserName == user).First();
            Product findProduct = _context.Products.Where(value => value.ProductName == product).First();

            if (findUser != null && findProduct != null)
            {
                mark.User = findUser;
                mark.Product = findProduct;
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
                    _context.Update(mark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarkExists(mark.MarkId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "Mark has been updated";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Error. Invalid form.";
            return View(mark);
        }

        // GET: Marks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Mark.Include(m => m.User).Include(m => m.Product)
                .FirstOrDefaultAsync(m => m.MarkId == id);
            if (mark == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(mark);
        }

        // POST: Marks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mark = await _context.Mark.FindAsync(id);
            _context.Mark.Remove(mark);
            StatusMessage = "Mark deleted successfully!";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarkExists(int id)
        {
            return _context.Mark.Any(e => e.MarkId == id);
        }
    }
}
