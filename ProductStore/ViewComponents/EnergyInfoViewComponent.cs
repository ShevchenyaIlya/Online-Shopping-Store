using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.ViewComponents
{
    public class EnergyInfoViewComponent : ViewComponent
    {
        private readonly AuthDbContext _context;
        public EnergyInfoViewComponent(AuthDbContext context)
        {
            this._context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(Product product)
        {
            var products = await _context.Products.ToListAsync();
            return View(product);
        }
    }
}
