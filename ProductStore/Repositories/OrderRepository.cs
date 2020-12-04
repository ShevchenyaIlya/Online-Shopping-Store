using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly AuthDbContext _dbContext;

        public OrderRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _dbContext.Order.Include(c => c.Customer).ToListAsync();
        }
        public void InsertOrder(Order order)
        {
            _dbContext.Add(order);
            Save();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void AddOrderToTransaction(Order order)
        {
            _dbContext.Add(order);
        }
    }
}
