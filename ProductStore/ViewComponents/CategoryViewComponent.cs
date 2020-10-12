using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly AuthDbContext _context;
        public CategoryViewComponent(AuthDbContext context)
        {
            this._context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var category = await _context.Category.ToListAsync();
            return View(category);
        }
    }
}
