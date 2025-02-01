using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Logic.Models.DTOs.User;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.AccountBuilders;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;

namespace Auth.Api.Controllers.Read
{
    [Authorize(AuthConsts.AUTHENTICATION_SCHEME)]
    [ApiVersion(1.0, Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController(IUserBuilder userB, IFileBuilder fileB, IOptionsSnapshot<FilesOptions> option) : ControllerBase
    {
        private readonly IUserBuilder _user = userB;
        private readonly IFileBuilder _file = fileB;
        private readonly FilesOptions _options = option.Value;

        [Authorize(AuthConsts.IS_USER)]
        [Route("Get")]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> Get()
        {
            var userId = User.GetUserId();
            var user = await _user.GetAsync(userId, Request.GetDomain());
            return Ok(user);
        }
        [Authorize(AuthConsts.IS_USER)]
        [Route("Avatar")]
        [HttpGet]
        public async Task<ActionResult> GetAvatar()
        {
            return await GetAvatar(User.GetUserId());
        }
        [Authorize(AuthConsts.IS_USER)]
        [Route("Avatar/{userId}")]
        [HttpGet]
        public async Task<ActionResult> GetAvatar([FromRoute] Guid userId)
        {
            var avatar = await _file.ReadFileAsync(_options.AvatarsStorePath, userId.ToAvatarName());
            if (avatar == null) return NoContent();
            return File(avatar, MIMEType.Png);
        }
    }
}
