using ProductStore.Data;
using ProductStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _dbContext;
        private bool disposed;
        public UnitOfWork(AuthDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<int> Commit()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }
    }
}
