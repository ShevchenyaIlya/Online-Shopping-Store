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
    public class UserRepository: IUserRepository
    {
        private readonly AuthDbContext _dbContext;

        public UserRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(m => m.Id == userId);
        }

        public async Task<ApplicationUser> GetUserByName(string name)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(m => m.UserName == name);
        }

        public void RemoveUser(string userId)
        {
            var user = _dbContext.Users.Find(userId);
            _dbContext.Users.Remove(user);
            Save();
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
