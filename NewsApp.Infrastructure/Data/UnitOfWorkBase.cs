using NewsApp.Domain.Interfaces.Repositorios;
using NewsApp.Infrastructure.DBContext;

namespace NewsApp.Infrastructure.Data
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
