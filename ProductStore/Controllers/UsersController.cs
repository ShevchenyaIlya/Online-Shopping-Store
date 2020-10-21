using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Models;

namespace ProductStore.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Route("Admin/Users")]
    [Route("Admin/User")]
    public class UsersController : Controller
    {
        private readonly AuthDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public UsersController(AuthDbContext context)
        {
            _context = context;
        }

        [Route("Index")]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.StatusMessage = StatusMessage;
            return View(await _context.Users.ToListAsync());
        }

        // GET: Categories/Details/5
        [Route("Details/{id:length(10, 40)}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            List<Comment> comments = new List<Comment>();
            foreach (var comm in _context.Comment.Include(m => m.CommentUser).Include(m => m.CommentProduct))
            {
                if (comm.CommentUser.Id == user.Id)
                {
                    comments.Add(comm);
                }
            }
            ViewBag.UserComments = comments;

            List<Mark> marks = new List<Mark>();
            foreach (var mark in _context.Mark.Include(m => m.User).Include(m => m.Product))
            {
                if (mark.User.Id == user.Id)
                {
                    marks.Add(mark);
                }
            }
            ViewBag.UserMarks = marks;

            List<Order> orders = new List<Order>();
            foreach (var order in _context.Order.Include(m => m.Customer))
            {
                if (order.Customer.Id == user.Id)
                {
                    orders.Add(order);
                }
            }
            ViewBag.UserOrders = orders;

            ViewBag.StatusMessage = StatusMessage;
            return View(user);
        }

        // GET: Categories/Delete/5
        [Route("Delete/{id:length(10, 40)}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(user);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:length(10, 40)}")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            StatusMessage = "User has been deleted";
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Category.Any(e => e.CategoryId == id);
        }
    }
}
