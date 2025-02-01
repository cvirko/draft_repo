using Auth.Domain.Core.Logic.Commands.User;
using Swashbuckle.AspNetCore.Filters;

namespace Auth.Api.Configuration.Swagger.SwaggerExamples.User
{
    public class UpdatePasswordRequestExample : IExamplesProvider<UpdatePasswordCommand>
    {
        public UpdatePasswordCommand GetExamples()
        {
            return new()
            {
                NewPassword = "Pa$$w0rd1",
                Password = "Pa$$w0rd"
            };
        }
    }
}
