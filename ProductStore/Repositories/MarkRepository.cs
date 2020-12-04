using Microsoft.EntityFrameworkCore;
using ProductStore.Areas.Identity.Data;
using ProductStore.Data;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public class MarkRepository: IMarkRepository
    {
        private readonly AuthDbContext _dbContext;

        public MarkRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Mark>> GetMarks()
        {
            return await _dbContext.Mark.Include(m => m.User).Include(m => m.Product).ToListAsync();
        }
        public async Task<Mark> GetMarkByUserAndProduct(ApplicationUser user, Product product)
        {
            return await _dbContext.Mark.Include(m => m.User).Include(m => m.Product).FirstOrDefaultAsync(mbox => mbox.User == user && mbox.Product == product);
        }
        public async Task<IEnumerable<Mark>> GetMarkWhereId(long? id)
        {
            return await _dbContext.Mark.Include(m => m.User).Include(m => m.Product).Where(m => m.Product.ProductId == id).ToListAsync();
        }
        public void InsertMark(Mark mark)
        {
            _dbContext.Add(mark);
            Save();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
