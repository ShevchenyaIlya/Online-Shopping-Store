using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly AuthDbContext _context;
        public ProductViewComponent(AuthDbContext context)
        {
            this._context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }
    }
}
