using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Models;
using ProductStore.Repositories;

namespace ProductStore.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Route("Admin/Users")]
    [Route("Admin/User")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMarkRepository _markRepository;

        [TempData]
        public string StatusMessage { get; set; }

        public UsersController(IUserRepository userRepository, ICommentRepository commentRepository, IOrderRepository orderRepository, IMarkRepository markRepository)
        {
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _orderRepository = orderRepository;
            _markRepository = markRepository;
        }

        [Route("Index")]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.StatusMessage = StatusMessage;
            return View(await _userRepository.GetUsers());
        }

        // GET: Categories/Details/5
        [Route("Details/{id:length(10, 40)}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            List<Comment> comments = new List<Comment>();
            foreach (var comm in await _commentRepository.GetComments())
            {
                if (comm.CommentUser.Id == user.Id)
                {
                    comments.Add(comm);
                }
            }
            ViewBag.UserComments = comments;

            List<Mark> marks = new List<Mark>();
            foreach (var mark in await _markRepository.GetMarks())
            {
                if (mark.User.Id == user.Id)
                {
                    marks.Add(mark);
                }
            }
            ViewBag.UserMarks = marks;

            List<Order> orders = new List<Order>();
            foreach (var order in await _orderRepository.GetOrders())
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

            var user = await _userRepository.GetUserById(id);
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
        public IActionResult DeleteConfirmed(string id)
        {
            _userRepository.RemoveUser(id);
            StatusMessage = "User has been deleted";
            return RedirectToAction(nameof(Index));
        }
    }
}
