using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetProductsWhereName(string name);
        Task<IEnumerable<Comment>> GetProductsLimit(long id, int limit);
        Task<IEnumerable<Comment>> GetComments();
        void InsertComment(Comment comment);
        void Save();
    }
}
