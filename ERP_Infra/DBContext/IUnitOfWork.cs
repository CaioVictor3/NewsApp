namespace ERP_Infra.DBContext
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}