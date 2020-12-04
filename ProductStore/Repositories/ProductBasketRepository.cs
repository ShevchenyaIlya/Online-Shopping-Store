using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public class ProductBasketRepository: IProductBasketRepository
    {
        private readonly AuthDbContext _dbContext;

        public ProductBasketRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void AddBasketToTransaction(ProductBasket basket)
        {
            _dbContext.Add(basket);
        }
    }
}
