using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Core.Logic.Commands.User;
using Auth.Domain.Core.Logic.Models.Tokens;
using Auth.Domain.Interface.Logic.Read.Mappers;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;

namespace Auth.Api.Controllers.Write
{
    [Authorize(AuthConsts.AUTHENTICATION_SCHEME)]
    [ApiVersion(1.0, Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController(ICommandDispatcher commandDispatcher, ITokenBuilder token,
        IUserMapper mapper) : CommandController(commandDispatcher)
    {
        private readonly ITokenBuilder _token = token;
        private readonly IUserMapper _mapper = mapper;

        [Authorize(AuthConsts.IS_USER)]
        [Route("Update/Avatar")]
        [HttpPost]
        public async Task<ActionResult> UpdateAvatar(IFormFile file)
        {
            using (var img = file.OpenReadStream())
                await ProcessAsync(new UpdateAvatarCommand(img, file.ContentType));

            return Ok();
        }
        [Authorize(AuthConsts.IS_USER)]
        [Route("Update/UserName")]
        [HttpPost]
        public async Task<ActionResult> UpdateUserName([FromBody] UpdateUserNameCommand command)
        {
            await ProcessAsync(command);
            return Ok();
        }
        [Authorize(AuthConsts.IS_USER)]
        [Route("Update/Password")]
        [HttpPost]
        public async Task<ActionResult> UpdatePassword([FromBody] UpdatePasswordCommand command)
        {
            await ProcessAsync(command);
            return Ok();
        }

        [Authorize(AuthConsts.IS_USER)]
        [Route("Add/Email")]
        [HttpPost]
        public async Task<ActionResult<TokenData>> UpdateEmailCodeSend([FromBody] AddLoginInCacheCommand command)
        {
            await ProcessAsync(command);
            await ProcessAsync(new SendConfirmEmailMessageCommand
            {
                Email = command.Email,
                UserName = User.GetFullName(),
                UserInfo = Request.GetUserInfo()
            });
            return Ok(_token.CreateEmailConfirmationToken(_mapper.Map(User, Request), command.Email));
        }

        [Authorize(AuthConsts.IS_USER)]
        [Route("Delete")]
        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] DeleteUserCommand command)
        {
            await ProcessAsync(command);
            return Ok();
        }
        [Authorize(AuthConsts.IS_USER)]
        [Route("Update/Video")]
        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        public async Task<ActionResult> UpdateVideo(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
                await ProcessAsync(new UpdateVideoCommand(stream, file.ContentType));
            return Ok();
        }
    }
}
