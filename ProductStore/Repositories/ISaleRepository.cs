using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public interface ISaleRepository
    {
        Task<IEnumerable<Sale>> GetSales();
    }
}
