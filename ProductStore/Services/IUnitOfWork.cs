using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Services
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Commit();
    }
}
