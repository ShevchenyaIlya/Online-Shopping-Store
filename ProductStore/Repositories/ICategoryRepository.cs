using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();

        Task<Category> FindFirsByCategoryName(string name);
        Task<Category> GetCategorytByID(long? category);
        bool CategoryExist(long id);
        void RemoveCategory(long categoryId);
        void UpdateCategory(Category category);
        void InsertCategory(Category category);
        void Save();
    }
}
