using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

namespace Auth.Api.Configuration.Swagger
{
    public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider = provider;
        public void Configure(SwaggerGenOptions options)
        {

            var jwtSecurityScheme = GetjwtSecurityScheme();
            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            options.AddSecurityRequirement(new() { { jwtSecurityScheme, Array.Empty<string>() } });
            GeneratedXMLDescription(options);
            GeneratedExampleFilters(options);

            foreach (var description in provider.ApiVersionDescriptions)
            {

                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private void GeneratedXMLDescription(SwaggerGenOptions options)
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            try
            {
                options.IncludeXmlComments(xmlPath);
            }
            catch
            {
                string msg = "Add \"<GenerateDocumentationFile>true</GenerateDocumentationFile>\" to the \"PropertyGroup\" project";
                ConsoleExtension.Errors(msg);
                throw new Exception(msg);
            }
            
        }
        private void GeneratedExampleFilters(SwaggerGenOptions options)
        {
            options.ExampleFilters();
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        }
        private static OpenApiSecurityScheme GetjwtSecurityScheme()
        {
            return new OpenApiSecurityScheme
            {
                BearerFormat = AuthConsts.BEARER_FORMAT,
                Name = AuthConsts.AUTHENTICATION_SCHEME,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = AuthConsts.AUTHENTICATION_SCHEME,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = AuthConsts.AUTHENTICATION_SCHEME,
                    Type = ReferenceType.SecurityScheme
                }
            };
        }
        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var text = new StringBuilder(AppConsts.SWAGGER_DESCRIPTION);
            var info = new OpenApiInfo()
            {
                Title = AppConsts.SWAGGER_TITLE,
                Version = description.ApiVersion.ToString(),
            };

            if (description.IsDeprecated)
            {
                text.Append(" This API version has been deprecated.");
            }

            text.Append("<h4>Additional Information</h4>");
            info.Description = text.ToString();

            return info;
        }
    }
}
