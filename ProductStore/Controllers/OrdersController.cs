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
    [Route("Admin/Orders")]
    [Route("Admin/Order")]
    public class OrdersController : Controller
    {
        private readonly AuthDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public OrdersController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        [Route("Index")]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.StatusMessage = StatusMessage;
            return View(await _context.Order.Include(m => m.Customer).ToListAsync());
        }

        // GET: Orders/Details/5
        [Route("Details/{id:long:min(1)?}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.Include(n => n.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(order);
        }

        // GET: Orders/Create
        [Route("Create")]
        public IActionResult Create()
        {
            List<SelectListItem> customers = new List<SelectListItem>();
            foreach (var customer in _context.Users)
            {
                customers.Add(new SelectListItem() { Value = customer.UserName, Text = customer.UserName });
            }
            ViewBag.Customer = customers;
            ViewBag.StatusMessage = StatusMessage;

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("OrderId,Addres,City,State,Country,ZipCode,AmountPaid,PaymentType,FormedDate")] Order order, string customer)
        {
            List<SelectListItem> customers = new List<SelectListItem>();
            foreach (var value in _context.Users)
            {
                customers.Add(new SelectListItem() { Value = value.UserName, Text = value.UserName });
            }
            ViewBag.CreatedPlace = customers;

            ApplicationUser temporalUser = _context.Users.Where(value => value.UserName == customer).First();

            if (temporalUser != null)
            {
                order.Customer = temporalUser;
            }
            else
            {
                StatusMessage = "Not all properties choosen";
                return RedirectToAction();
            }

            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                StatusMessage = "Order has been added";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Error. Invalid form.";
            return View(order);
        }

        // GET: Orders/Edit/5
        [Route("Edit/{id:long:min(1)?}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SelectListItem> customers = new List<SelectListItem>();
            foreach (var customer in _context.Users)
            {
                customers.Add(new SelectListItem() { Value = customer.UserName, Text = customer.UserName });
            }
            ViewBag.Customer = customers;

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:long:min(1)}")]
        public async Task<IActionResult> Edit(long id, [Bind("OrderId,Addres,City,State,Country,ZipCode,AmountPaid,PaymentType,FormedDate")] Order order, string customer)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            List<SelectListItem> customers = new List<SelectListItem>();
            foreach (var value in _context.Users)
            {
                customers.Add(new SelectListItem() { Value = value.UserName, Text = value.UserName });
            }
            ViewBag.Customer = customers;

            ApplicationUser temporalUser = _context.Users.Where(value => value.UserName == customer).First();
            if (temporalUser != null)
            {
                order.Customer = temporalUser;
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
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "Order has been updated";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Error. Invalid form.";
            return View(order);
        }

        // GET: Orders/Delete/5
        [Route("Delete/{id:long:min(1)?}")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.Include(n => n.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:long:min(1)}")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            StatusMessage = "Order deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(long id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
