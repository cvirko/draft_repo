using Auth.Domain.Interface.Data.Read.Repository;

namespace Auth.Domain.Interface.Data.Read.UOW
{
    public interface IUnitOfWorkRead : IDisposable
    {
        IUserRepository Users();
    }
}
