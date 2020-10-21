using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Migrations;
using ProductStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public class CommentRepositoryAsync : GenericRepositoryAsync<Comment>, ICustomerRepositoryAsync
    {
        private readonly DbSet<Comment> _customer;
        public CommentRepositoryAsync(AuthDbContext dbContext) : base(dbContext)
        {
            _customer = dbContext.Set<Comment>();
        }
    }
}
