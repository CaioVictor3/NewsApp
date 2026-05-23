namespace ERP_Domain.Handle
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }

    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message) { }
    }

}
