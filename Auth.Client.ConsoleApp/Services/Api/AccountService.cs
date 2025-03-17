using Auth.Client.ConsoleApp.Consts;
using Auth.Client.ConsoleApp.Interfaces;
using Auth.Domain.Core.Common.Exceptions;
using Auth.Domain.Core.Common.Extensions;
using Auth.Domain.Core.Logic.Commands.Account;
using Auth.Domain.Core.Logic.Models.Tokens;

namespace Auth.Client.ConsoleApp.Services.Api
{
    public class AccountService(IServerClientService client) : IAccountService
    {
        private IServerClientService _client = client;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private Dictionary<Requests, string> requestPath = new()
        {
            {Requests.Refresh, "api/v1/Account/Token/Refresh" },
            {Requests.Login,"api/v1/Account/Login" },
            {Requests.SignUp,"api/v1/Account/SignUp" },
            {Requests.ConfirmCode,"api/v1/Account/Email/Code/Confirm" },
        };
        private DateTime expiresDate;
        private string refreshToken;
        private string accessToken;
        public string Token => accessToken ?? throw new Exception(" Token not found");

        public void Dispose()
        {
            cts.Dispose();
        }
        public async Task TryLoginAsync()
        {
            Console.WriteLine("Input email: ");
            var email = Console.ReadLine();
            Console.WriteLine("Input password (default 'Pa$$w0rd': ");
            var password = Console.ReadLine();

            var request = new SignInCommand()
            {
                Login = email,
                Password = string.IsNullOrEmpty(password) ? "Pa$$w0rd" : password,
            };
            try
            {
                await LoginAsync(request);
            }
            catch
            {
                await CreateUserAsync(request);
            }
            finally
            {
                _ = Task.Run(UpdateTokenWorkerAsync, cts.Token);
            }
        }
        private async Task LoginAsync(SignInCommand request)
        {
            var tokens = await _client.PostAsync<TokenData[], SignInCommand>(request, requestPath[Requests.Login]);
            accessToken = tokens[1].Token;
            expiresDate = tokens[1].Expires;
            refreshToken = tokens[0].Token;
        }
        
        private async Task UpdateTokenWorkerAsync()
        {
            while (!cts.Token.IsCancellationRequested)
            {
                int expires = Convert.ToInt32( (expiresDate - DateTimeExtension.Get())
                    .TotalMinutes - 0.5);
                await Task.Delay(1000 * 60 * expires, cts.Token);
                Console.WriteLine("{0} Update token {1}", 
                    ConsoleConst.YELLOW, ConsoleConst.NORMAL);
                await RefreshTokenAsync();
            }
        }
        private async Task CreateUserAsync(SignInCommand command)
        {
            var request = new SignUpInCacheCommand()
            {
                Email = command.Login,
                UserName = "BotTest",
                Password = command.Password,
            };
            TokenData token = await _client.PostAsync<TokenData, SignUpInCacheCommand>(request, requestPath[Requests.SignUp]);
            accessToken = token.Token;
            expiresDate = token.Expires;
            await ConfirmEmailUserAsync();
        }
        private async Task ConfirmEmailUserAsync()
        {
            var request = new ConfirmTokenCommand()
            {
                Token = GetUserConfirmationCodeAsync()
            };
            var tokens = await _client.PostAsync<TokenData[], ConfirmTokenCommand>(request, requestPath[Requests.ConfirmCode], accessToken);
            accessToken = tokens[1].Token;
            expiresDate = tokens[1].Expires;
            refreshToken = tokens[0].Token;
        }
        private string GetUserConfirmationCodeAsync()
        {
            Console.WriteLine("Input email confirmation code: ");
            var code = Console.ReadLine();
            return code;
        }
        private async Task RefreshTokenAsync()
        {
            try
            {
                var tokens = await _client.PostAsync<TokenData[],string>(string.Empty, requestPath[Requests.Refresh], refreshToken);
                if (tokens == null)
                {
                    Console.WriteLine("{0} No Refresh token{1}", ConsoleConst.RED,ConsoleConst.NORMAL);
                }
                accessToken = tokens[1].Token;
                expiresDate = tokens[1].Expires;
                refreshToken = tokens[0].Token;
            }
            catch (ResponseException ex)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                if (ex.Message != null)
                    Console.WriteLine(ex.Message);
                foreach (var error in ex.Errors)
                    Console.WriteLine("{0}: {1}", error.Field, error.Title);
                Console.BackgroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private enum Requests
        {
            Login,
            SignUp,
            Refresh,
            ConfirmCode,
            GetConfirmCode,
        }
    }
}
