using ProductStore.Areas.Identity.Data;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public interface IMarkRepository
    {
        Task<IEnumerable<Mark>> GetMarks();
        Task<Mark> GetMarkByUserAndProduct(ApplicationUser user, Product product);
        Task<IEnumerable<Mark>> GetMarkWhereId(long? id);
        void InsertMark(Mark mark);
        void Save();
    }
}
