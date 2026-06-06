using NewsApp.Domain.Interfaces.Repositorios;
using NewsApp.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Infrastructure.Base
{
    public abstract class RepositorioBase
    {
        private readonly ContextBase _context;

        public IUnitOfWorkBase UnitOfWork => _context;

        protected RepositorioBase(ContextBase contextBase)
        {
            _context = contextBase;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void DiscardChanges()
        {
            _context.DiscardChanges();
        }

    }
}
