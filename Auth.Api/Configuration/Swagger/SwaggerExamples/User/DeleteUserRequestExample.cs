using Auth.Domain.Core.Logic.Commands.User;
using Swashbuckle.AspNetCore.Filters;

namespace Auth.Api.Configuration.Swagger.SwaggerExamples.User
{
    public class DeleteUserRequestExample : IExamplesProvider<DeleteUserCommand>
    {
        public DeleteUserCommand GetExamples()
        {
            return new()
            {
                Password = "Pa$$w0rd"
            };
        }
    }
}
