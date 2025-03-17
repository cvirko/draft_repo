using Auth.Domain.Core.Common.Exceptions;
using Auth.Domain.Interface.Logic.Notification.Sockets;
using Auth.Domain.Interface.Logic.Read.Validators;

namespace Auth.Api.Controllers.Write
{
    [Authorize(AuthConsts.AUTHENTICATION_SCHEME)]
    [ApiVersion(1.0, Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ChatController(IChatMessageService chat, IUnitOfWorkValidationRule validate) : ControllerBase
    {
        private readonly IChatMessageService _chat = chat;
        private readonly IUnitOfWorkValidationRule _validate = validate;

        [Authorize(AuthConsts.IS_USER)]
        [Route("Join/{name}")]
        [HttpPost]
        public async Task<ActionResult> JoinToChat([FromRoute] string name)
        {
            if (!_validate.Name().IsLengthFormatValid(name))
            {
                throw new ForbiddenException(_validate.GetErrors());
            }
            await _chat.AddUserToGroupAsync(name, User.GetUserId());
            return Ok();
        }
        [Authorize(AuthConsts.IS_USER)]
        [Route("Leave/{name}")]
        [HttpPost]
        public async Task<ActionResult> LeaveChat([FromRoute] string name)
        {
            if (!_validate.Name().IsLengthFormatValid(name))
            {
                throw new ForbiddenException(_validate.GetErrors());
            }
            await _chat.RemoveUserFromGroupAsync(name, User.GetUserId());
            return Ok();
        }
        [Authorize(AuthConsts.IS_USER)]
        [Route("Send/Message/{name}")]
        [HttpPost]
        public async Task<ActionResult> Send([FromRoute] string name, [FromBody] string message)
        {
            _validate.Message().IsLengthFormatValid(message);
            _validate.Name().IsLengthFormatValid(name);
            if (!_validate.IsValid())
            {
                throw new ForbiddenException(_validate.GetErrors());
            }
            await _chat.SendChatMessageAsync(new()
            {
                GroupName = name,
                Text = message,
                UserId = User.GetUserId(),
                UserName = User.GetFullName()
            });
            return Ok();
        }
    }
}
