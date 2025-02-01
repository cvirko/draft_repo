using Auth.Domain.Core.Logic.Models.DTOs.User;

namespace Auth.Domain.Interface.Logic.Read.ModelBuilder.AccountBuilders
{
    public interface IUserBuilder : IBuilder
    {
        public Task<UserDTO> GetAsync(Guid userId, string baseURL);
    }
}
