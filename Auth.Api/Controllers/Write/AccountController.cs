using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Core.Logic.Models.DTOs.User;
using Auth.Domain.Core.Logic.Models.Tokens;
using Auth.Domain.Interface.Logic.Read.Mappers;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.AccountBuilders;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;

namespace Auth.Api.Controllers.Write
{
    //[Authorize(AuthConsts.AUTHENTICATION_SCHEME)]
    [ApiVersion(1.0, Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountController(ICommandDispatcher command,
        ITokenBuilder token, IUserMapper mapper, IAccountBuilder builder) : CommandController(command)
    {
        private readonly ITokenBuilder _token = token;
        private readonly IUserMapper _mapper = mapper;
        private readonly IAccountBuilder _account = builder;

        /// <summary>
        ///  stores user data for 30 minutes for mail confirmation
        ///  Otherwise it loses the data
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Return access token</returns>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     {
        ///        "UserName": "Kazuma",
        ///        "Email": "example@gmail.com",
        ///        "Password": "Pa$$w0rd",
        ///     }
        ///
        /// </remarks>
        [AllowAnonymous]
        [Route("SignUp")]
        [HttpPost]
        public async Task<ActionResult<TokenData>> SignUp([FromBody] SignUpInCacheCommand command)
        {
            await _command.ProcessAsync(command);
            var login = _mapper.Map(command);

            await _command.ProcessAsync(new SendConfirmEmailMessageCommand
            {
                Email = command.Email,
                UserName = login.UserName,
                UserId = login.UserId,
                TokenLoginId = login.TokenLoginId
            });
            return Ok(_token.CreateEmailConfirmationToken(login, command.Email));
        }
        [Authorize(AuthConsts.IS_CONFIRMATION_MAIL)]
        [Route("Email/Code/ReSend")]
        [HttpPost]
        public async Task<ActionResult> SendCode()
        {
            await ProcessAsync(new SendConfirmEmailMessageCommand
            {
                Email = User.GetUserEmail(),
                UserName = User.GetFullName(),
                TokenLoginId = User.GetTokenLoginId(),
            });
            return Ok();
        }

        [Authorize(AuthConsts.IS_CONFIRMATION_MAIL)]
        [Route("Email/Code/Confirm")]
        [HttpPost]
        public async Task<ActionResult<TokenData[]>> MailConfirm([FromBody] ConfirmTokenCommand command)
        {
            command.TokenType = TokenType.ConfirmMail;
            command.TokenLoginId = User.GetTokenLoginId();
            await ProcessAsync(command);
            var createCommand = new CreateAccountFromCacheCommand(User.GetUserEmail());
            await ProcessAsync(createCommand);
            var login = await _account.GetLoginAsync(User.GetUserEmail());
            return await GetRefreshTokenAsync(login);
        }

        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<TokenData[]>> Login([FromBody] SignInCommand command)
        {
            await ProcessAsync(command);
            var login = await _account.GetLoginAsync(command.Login);
            return await GetRefreshTokenAsync(login);
        }

        [Authorize(AuthConsts.IS_REFRESH_TOKEN)]
        [Route("Token/Refresh")]
        [HttpPost]
        public async Task<ActionResult<TokenData[]>> RefreshToken()
        {
            await ProcessAsync(new ConfirmTokenCommand(User.GetTokenId(), TokenType.Refresh, User.GetTokenLoginId()));
            var login = await _account.GetLoginAsync(User);
            return await GetRefreshTokenAsync(login);
        }

        [AllowAnonymous]
        [Route("Login/Google")]
        [HttpPost]
        public async Task<ActionResult<TokenData[]>> LoginGoogle([FromBody] string token)
        {
            var socialInfo = await _account.GetBySocialAsync(token, SocialType.Google);
            
            await ProcessAsync(new SignInSocialCommand { Info = socialInfo});

            var login = await _account.GetLoginAsync(socialInfo.Email);
            return await GetRefreshTokenAsync(login);
        }
        [AllowAnonymous]
        [Route("Login/Facebook")]
        [HttpPost]
        public async Task<ActionResult<TokenData[]>> LoginFacebook([FromBody] string token)
        {
            var socialInfo = await _account.GetBySocialAsync(token, SocialType.Facebook);

            await ProcessAsync(new SignInSocialCommand { Info = socialInfo });

            var login = await _account.GetLoginAsync(socialInfo.Email);
            return await GetRefreshTokenAsync(login);
        }
        [AllowAnonymous]
        [Route("Forgot/Password/Send")]
        [HttpPost]
        public async Task<ActionResult<TokenData>> ForgotPassword([FromBody] SendResetPassMessageCommand command)
        {
            await ProcessAsync(command);
            var login = await _account.GetLoginAsync(command.Email);
            login.TokenLoginId = login.UserId;
            var response = _token.CreateEmailConfirmationToken(login,
                command.Email, TokenType.ConfirmPassword);
            return Ok(response);
        }

        [Authorize(AuthConsts.IS_CONFIRMATION_PASSWORD)]
        [Route("Forgot/Password/ReSend")]
        [HttpPost]
        public async Task<ActionResult> ForgotPassword()
        {
            await ProcessAsync(new SendResetPassMessageCommand(User.GetUserEmail()));
            return Ok();
        }

        [Authorize(AuthConsts.IS_CONFIRMATION_PASSWORD)]
        [Route("Forgot/Password/Confirm")]
        [HttpPost]
        public async Task<ActionResult<TokenData>> ForgotPassword([FromBody] ConfirmTokenCommand command)
        {
            command.TokenType = TokenType.Reset;
            command.TokenLoginId = User.GetTokenLoginId();
            await ProcessAsync(command);
            var response = _token.CreateEmailConfirmationToken(_mapper.Map(User), 
                User.GetUserEmail(), TokenType.Reset);
            return Ok(response);
        }
        
        [Authorize(AuthConsts.IS_RESET)]
        [Route("Forgot/Password/Reset")]
        [HttpPost]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            await ProcessAsync(command);
            return Ok();
        }

        private async Task<ActionResult<TokenData[]>> GetRefreshTokenAsync(LoginDTO login)
        {
            var tokenId = Guid.NewGuid();
            if (login is null)
                login = _mapper.Map(User);
            await _command.ProcessAsync(new UpdateTokenCommand(login.TokenLoginId, login.UserId, tokenId.ToString(), TokenType.Refresh));
            var refresh = _token.CreateRefreshTokens(login, tokenId);
            return Ok(new TokenData[]
            {
                refresh,
               _token.CreateAccessTokens(login)
            });
        }
    }
}
