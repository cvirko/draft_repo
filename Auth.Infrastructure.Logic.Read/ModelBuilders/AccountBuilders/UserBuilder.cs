using Auth.Domain.Core.Common.Consts;
using Auth.Domain.Core.Logic.Models.DTOs.User;

namespace Auth.Infrastructure.Logic.Read.ModelBuilders.AccountBuilders
{
    internal class UserBuilder(IUnitOfWorkRead uow, IUserMapper mapper) : IUserBuilder
    {
        private readonly IUnitOfWorkRead _uow = uow;
        private readonly IUserMapper _mapper = mapper;

        public async Task<UserDTO> GetAsync(Guid userId, string baseURL)
        {
            var user = await _uow.Users().GetUserAsync(userId);
            return _mapper.Map(user, Path.Combine(baseURL, AppConsts.AVATARS_PATH));
        }
    }
}
