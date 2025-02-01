using Auth.Domain.Core.Logic.Commands.User;
using Swashbuckle.AspNetCore.Filters;

namespace Auth.Api.Configuration.Swagger.SwaggerExamples.User
{
    public class AddEmailRequestExample : IExamplesProvider<AddLoginInCacheCommand>
    {
        public AddLoginInCacheCommand GetExamples()
        {
            return new()
            {
                Email = "one.example@gmail.com",
                Password = "Pa$$w0rd"
            };
        }
    }
}
