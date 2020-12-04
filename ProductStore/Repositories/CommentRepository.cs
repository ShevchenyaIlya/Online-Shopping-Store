using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public class CommentRepository: ICommentRepository
    {
        private readonly AuthDbContext _dbContext;

        public CommentRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Comment>> GetProductsWhereName(string name)
        {
            return await _dbContext.Comment
                .Include(m => m.CommentUser).Include(n => n.CommentProduct)
                .Where(m => m.CommentProduct.ProductName == name).OrderByDescending(m => m.PostDate).ToListAsync(); ;
        }

        public async Task<IEnumerable<Comment>> GetProductsLimit(long id, int limit)
        {
            return await _dbContext.Comment.Include(m => m.CommentUser).Include(n => n.CommentProduct)
                .Where(mbox => mbox.CommentProduct.ProductId == id).OrderByDescending(m => m.PostDate).Take(limit).ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetComments()
        {
            return await _dbContext.Comment.Include(c => c.CommentUser).Include(p => p.CommentProduct).ToListAsync();
        }

        public void InsertComment(Comment comment)
        {
            _dbContext.Add(comment);
            Save();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
