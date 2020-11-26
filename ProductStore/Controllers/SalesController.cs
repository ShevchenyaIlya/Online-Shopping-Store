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
    [Route("Admin/Sales")]
    [Route("Admin/Sale")]
    public class SalesController : Controller
    {
        private readonly AuthDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public SalesController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: Sales
        [Route("Index")]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.StatusMessage = StatusMessage;
            return View(await _context.Sale.Include(n => n.Product).ToListAsync());
        }

        // GET: Sales/Details/5
        [Route("Details/{id:int?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sale.Include(n => n.Product)
                .FirstOrDefaultAsync(m => m.SaleId == id);
            if (sale == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(sale);
        }

        // GET: Sales/Create
        [Route("Create")]
        public IActionResult Create()
        {
            ViewBag.StatusMessage = StatusMessage;
            List<SelectListItem> products = new List<SelectListItem>();
            foreach (var productValue in _context.Products)
            {
                products.Add(new SelectListItem() { Value = productValue.ProductName, Text = productValue.ProductName });
            }
            ViewBag.Products = products; 
            ViewBag.StatusMessage = StatusMessage;
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("SaleId,SaleValue,AddedDate,FromDate,ToDate")] Sale sale, string product)
        {
            List<SelectListItem> products = new List<SelectListItem>();
            foreach (var productValue in _context.Products)
            {
                products.Add(new SelectListItem() { Value = productValue.ProductName, Text = productValue.ProductName });
            }
            ViewBag.Products = products;

            if (sale.SaleValue > 1 || sale.SaleValue < 0)
            {
                StatusMessage = "Error. Invalid range for sale value.";
                return RedirectToAction();
            }

            Product findProduct = _context.Products.Where(value => value.ProductName == product).First();
            if (findProduct != null)
            {
                sale.Product = findProduct;
            }
            else
            {
                StatusMessage = "Not all properties choosen";
                return RedirectToAction();
            }

            if (ModelState.IsValid)
            {
                _context.Add(sale);
                await _context.SaveChangesAsync();
                StatusMessage = "Sale has been added";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Error. Invalid form.";
            return View(sale);
        }

        // GET: Sales/Edit/5
        [Route("Edit/{id:int?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SelectListItem> products = new List<SelectListItem>();
            foreach (var productValue in _context.Products)
            {
                products.Add(new SelectListItem() { Value = productValue.ProductName, Text = productValue.ProductName });
            }
            ViewBag.Products = products;

            var sale = await _context.Sale.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, [Bind("SaleId,SaleValue,AddedDate,FromDate,ToDate")] Sale sale, string product)
        {
            if (id != sale.SaleId)
            {
                return NotFound();
            }

            List<SelectListItem> products = new List<SelectListItem>();
            foreach (var productValue in _context.Products)
            {
                products.Add(new SelectListItem() { Value = productValue.ProductName, Text = productValue.ProductName });
            }
            ViewBag.Products = products;

            if (sale.SaleValue > 1 || sale.SaleValue < 0)
            {
                StatusMessage = "Error. Invalid range for sale value.";
                return RedirectToAction();
            }

            Product findProduct = _context.Products.Where(value => value.ProductName == product).First();
            if (findProduct != null)
            {
                sale.Product = findProduct;
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
                    _context.Update(sale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.SaleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "Sale has been updated";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Error. Invalid form.";
            return View(sale);
        }

        // GET: Sales/Delete/5
        [Route("Delete/{id:int?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sale.Include(m => m.Product)
                .FirstOrDefaultAsync(m => m.SaleId == id);
            if (sale == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sale = await _context.Sale.FindAsync(id);
            _context.Sale.Remove(sale);
            await _context.SaveChangesAsync();
            StatusMessage = "Sale deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
            return _context.Sale.Any(e => e.SaleId == id);
        }
    }
}
