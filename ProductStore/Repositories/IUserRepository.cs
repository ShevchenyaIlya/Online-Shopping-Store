using ProductStore.Areas.Identity.Data;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetUsers();
        Task<ApplicationUser> GetUserById(string userId);
        Task<ApplicationUser> GetUserByName(string name);
        void RemoveUser(string userId);
        void Save();
    }
}
