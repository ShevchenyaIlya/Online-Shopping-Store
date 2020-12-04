using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly AuthDbContext _dbContext;

        public ProductRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteProduct(long productId)
        {
            var product = _dbContext.Products.Find(productId);
            _dbContext.Products.Remove(product);
            Save();
        }

        public async Task<Product> GetProductByID(long? productId)
        {
            return await _dbContext.Products.Include(n => n.Category).Include(c => c.CreatedPlace).FirstOrDefaultAsync(m => m.ProductId == productId);
        }

        public async Task<Product> GetProductByName(string name)
        {
            return await _dbContext.Products.Include(n => n.Category).Include(c => c.CreatedPlace).FirstOrDefaultAsync(m => m.ProductName == name);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _dbContext.Products.Include(n => n.Category).Include(c => c.CreatedPlace).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsDescendant()
        {
            return await _dbContext.Products.Include(n => n.Category).Include(c => c.CreatedPlace).OrderByDescending(m => m.AddedDate).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsWhereId(long? id)
        {
            return await _dbContext.Products.Include(n => n.Category).Include(c => c.CreatedPlace).Where(m => m.Category.CategoryId == id).ToListAsync();
        }

        public void InsertProduct(Product product)
        {
            _dbContext.Add(product);
            Save();
        }

        public bool ProductExist(long id)
        {
            return _dbContext.Products.Any(e => e.ProductId == id);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _dbContext.Entry(product).State = EntityState.Modified;
            Save();
        }
    }
}
