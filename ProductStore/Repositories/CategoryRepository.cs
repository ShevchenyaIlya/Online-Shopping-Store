using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly AuthDbContext _dbContext;

        public CategoryRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _dbContext.Category.ToListAsync();
        }

        public bool CategoryExist(long id)
        {
            return _dbContext.Category.Any(e => e.CategoryId == id);
        }

        public void RemoveCategory(long categoryId)
        {
            var category = _dbContext.Category.Find(categoryId);
            _dbContext.Category.Remove(category);
            Save();
        }

        public void UpdateCategory(Category category)
        {
            _dbContext.Entry(category).State = EntityState.Modified;
            Save();
        }

        public void InsertCategory(Category category)
        {
            _dbContext.Add(category);
            Save();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task<Category> FindFirsByCategoryName(string name)
        {
            return await _dbContext.Category.Where(value => value.CategoryName == name).FirstAsync();
        }
        public async Task<Category> GetCategorytByID(long? category)
        {
            return await _dbContext.Category.FirstOrDefaultAsync(m => m.CategoryId == category);
        }
    }
}
