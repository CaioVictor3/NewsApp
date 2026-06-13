namespace NewsApp.Domain.Interfaces.Repositorios
{
    public interface IUnitOfWorkBase : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
