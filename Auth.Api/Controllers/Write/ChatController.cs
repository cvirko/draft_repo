using Auth.Domain.Core.Common.Exceptions;
using Auth.Domain.Interface.Logic.Notification.Sockets;
using Auth.Domain.Interface.Logic.Notification.Sockets.RabbitMQ;
using Auth.Domain.Interface.Logic.Read.Validators;

namespace Auth.Api.Controllers.Write
{
    [Authorize(AuthConsts.AUTHENTICATION_SCHEME)]
    [ApiVersion(1.0, Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ChatController(IChatMessageService chat, IUnitOfWorkValidationRule validate,
        IRabbitMQRequest rabbitMQRequest) : ControllerBase
    {
        private readonly IChatMessageService _chat = chat;
        private readonly IUnitOfWorkValidationRule _validate = validate;
        private readonly IRabbitMQRequest _rabbitMQRequest = rabbitMQRequest;

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
            await _rabbitMQRequest.SendAsync(AppConsts.APP_NAME,
                string.Format( "{0} Join To Chat: {1}",User.GetFullName(), name));
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
            await _rabbitMQRequest.SendAsync(AppConsts.APP_NAME,
                string.Format("{0} Leave Chat: {1}", User.GetFullName(), name));
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
            await _rabbitMQRequest.SendAsync(AppConsts.APP_NAME,
                string.Format("{0}-{1} Send MSG: {2}", User.GetFullName(), name, message));
            return Ok();
        }
    }
}
