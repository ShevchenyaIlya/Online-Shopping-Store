using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public class SaleRepository: ISaleRepository
    {
        private readonly AuthDbContext _dbContext;

        public SaleRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Sale>> GetSales()
        {
            return await _dbContext.Sale.Include(m => m.Product).ToListAsync();
        }
    }
}
