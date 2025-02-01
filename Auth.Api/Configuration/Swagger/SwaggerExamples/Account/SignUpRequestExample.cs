using Auth.Domain.Core.Logic.Commands.Account;
using Swashbuckle.AspNetCore.Filters;

namespace Auth.Api.Configuration.Swagger.SwaggerExamples.Account
{
    public class SignUpRequestExample : IExamplesProvider<SignUpInCacheCommand>
    {
        public SignUpInCacheCommand GetExamples()
        {
            return new()
            {
                UserName = "PonPushka",
                Email = "example@gmail.com",
                Password = "Pa$$w0rd",
            };
        }
    }
}
