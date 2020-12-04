using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Product>> GetProductsDescendant();
        Task<Product> GetProductByID(long? productId);
        Task<Product> GetProductByName(string name);
        Task<IEnumerable<Product>> GetProductsWhereId(long? id);
        void InsertProduct(Product product);
        void DeleteProduct(long productId);
        void UpdateProduct(Product product);
        bool ProductExist(long id);
        void Save();
    }
}
