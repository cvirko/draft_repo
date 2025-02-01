using Auth.Api.Configuration.Swagger;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Auth.Api.Configuration
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerGenConfig(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(options => { options.OperationFilter<SwaggerDefaultValues>(); });
            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

        }
        public static void UseSwaggerCustom(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
               options =>
               {
                   var descriptions = app.DescribeApiVersions();

                   foreach (var description in descriptions)
                   {
                       var url = $"/swagger/{description.GroupName}/swagger.json";
                       var name = description.GroupName.ToUpperInvariant();
                       options.SwaggerEndpoint(url, name);
                   }
               });
        }
    }
}
