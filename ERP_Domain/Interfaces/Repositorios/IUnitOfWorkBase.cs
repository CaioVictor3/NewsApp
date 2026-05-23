using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Domain.Interfaces.Repositorios
{
    public interface IUnitOfWorkBase : IDisposable
    {
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
