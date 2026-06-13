using NewsApp.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace NewsApp.Infrastructure.Base
{
    public abstract class ContextBase : DbContext, IUnitOfWorkBase
    {
        protected ContextBase(DbContextOptions options) : base(options)
        {
        }

        public virtual Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
