using Auth.Domain.Core.Logic.Commands.Account;
using Swashbuckle.AspNetCore.Filters;

namespace Auth.Api.Configuration.Swagger.SwaggerExamples.Account
{
    public class LoginRequestExample : IExamplesProvider<SignInCommand>
    {
        public SignInCommand GetExamples()
        {
            return new()
            {
                Login = "example@gmail.com",
                Password = "Pa$$w0rd"
            };
        }
    }
}
