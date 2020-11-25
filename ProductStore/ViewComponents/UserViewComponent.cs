using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.ViewComponents
{
    public class UserViewComponent : ViewComponent
    {
        private readonly AuthDbContext _context;
        public UserViewComponent(AuthDbContext context)
        {
            this._context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
    }
}
