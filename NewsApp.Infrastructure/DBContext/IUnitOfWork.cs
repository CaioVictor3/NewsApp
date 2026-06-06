namespace NewsApp.Infrastructure.DBContext
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}