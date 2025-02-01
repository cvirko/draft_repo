using Auth.Domain.Core.Logic.Commands.Account;
using Swashbuckle.AspNetCore.Filters;

namespace Auth.Api.Configuration.Swagger.SwaggerExamples.Account
{
    public class ForgotPasswordRequestExample : IExamplesProvider<SendResetPassMessageCommand>
    {
        public SendResetPassMessageCommand GetExamples()
        {
            return new("example@gmail.com");
        }
    }
}
