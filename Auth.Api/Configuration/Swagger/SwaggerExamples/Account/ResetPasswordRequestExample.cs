using Auth.Domain.Core.Logic.Commands.Account;
using Swashbuckle.AspNetCore.Filters;

namespace Auth.Api.Configuration.Swagger.SwaggerExamples.Account
{
    public class ResetPasswordRequestExample : IExamplesProvider<ResetPasswordCommand>
    {
        public ResetPasswordCommand GetExamples()
        {
            return new()
            {
                Password = "Pa$$w0rd",
            };
        }
    }
}
