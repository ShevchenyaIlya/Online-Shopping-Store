using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProductStore.Areas.Identity.Data;
using ProductStore.Data;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Hubs
{
    public class CommentHub : Hub
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentHub(UserManager<ApplicationUser> userManager, AuthDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task JoinPostGroup(string productName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, productName);
            //await Clients.Group(productName).SendAsync("Notify", $"User вошел в чат");
        }


        [Authorize]
        public async Task SendMessage(string title, string body, string postDate, string image, string username, string productName)
        {
            if (!(productName == null || title == null || body == null))
            {
                var dataComment = new Comment();
                dataComment.CommentTitle = title;
                dataComment.CommentBody = body;
                dataComment.PostDate = DateTime.Parse(postDate);
                dataComment.CommentUser = await _context.Users.FirstOrDefaultAsync(m => m.UserName == username);
                dataComment.CommentProduct = await _context.Products.FirstOrDefaultAsync(mbox => mbox.ProductName == productName);
                _context.Add(dataComment);
                await _context.SaveChangesAsync();
                await Clients.Group(productName).SendAsync("ReceiveMessage", title, body, postDate, image, username, productName);
            }
        }
    }
}
