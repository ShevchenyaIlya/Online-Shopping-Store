using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductStore.Areas.Identity.Data;
using ProductStore.Data;
using ProductStore.Models;

namespace ProductStore.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class CommentsController : Controller
    {
        private readonly AuthDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public CommentsController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            ViewBag.StatusMessage = StatusMessage;
            return View(await _context.Comment.Include(n => n.CommentProduct).Include(n => n.CommentUser).ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.Include(m => m.CommentProduct).Include(m => m.CommentUser)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(comment);
        }

        // GET: Comments/Create
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

            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,CommentTitle,CommentBody,PostDate")] Comment comment, string commentUser, string commentProduct)
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

            ApplicationUser findUser = _context.Users.Where(value => value.UserName == commentUser).First();
            Product findProduct = _context.Products.Where(value => value.ProductName == commentProduct).First();

            if (findUser != null && findProduct != null)
            {
                comment.CommentUser = findUser;
                comment.CommentProduct = findProduct;
            }
            else
            {
                StatusMessage = "Not all properties choosen";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.ErrorCount == 2)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                StatusMessage = "Product has been added";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Error. Invalid form.";
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(long? id)
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

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CommentId,CommentTitle,CommentBody,PostDate")] Comment comment, string commentUser, string commentProduct)
        {
            if (id != comment.CommentId)
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

            ApplicationUser findUser = _context.Users.Where(value => value.UserName == commentUser).First();
            Product findProduct = _context.Products.Where(value => value.ProductName == commentProduct).First();

            if (findUser != null && findProduct != null)
            {
                comment.CommentUser = findUser;
                comment.CommentProduct = findProduct;
            }
            else
            {
                StatusMessage = "Not all properties choosen";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.ErrorCount == 2)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "Product has been updated";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Error. Invalid form.";
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.Include(m => m.CommentProduct).Include(m => m.CommentUser)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var comment = await _context.Comment.FindAsync(id);
            _context.Comment.Remove(comment);
            StatusMessage = "Product deleted successfully!";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(long id)
        {
            return _context.Comment.Any(e => e.CommentId == id);
        }
    }
}
