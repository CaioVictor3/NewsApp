using ERP_Domain.Interfaces.Repositorios;
using ERP_Infra.DBContext;

namespace ERP_Infra.Data
{
    public class UnitOfWorkBase : IUnitOfWorkBase
    {
        protected readonly Context _context;

        public UnitOfWorkBase(Context context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}
