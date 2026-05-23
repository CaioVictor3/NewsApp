using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Domain.Interfaces.Repositorios
{
    public interface IRepositorioBase<T> : IDisposable
    {
        IUnitOfWorkBase UnitOfWork { get; }
        void SaveChanges();
        Task SaveChangesAsync();
        void DiscardChanges();
    }
}
