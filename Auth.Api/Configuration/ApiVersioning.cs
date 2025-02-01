namespace Auth.Api.Configuration
{
    public static class ApiVersioning
    {
        public static void AddApiVersioningCustom(this IServiceCollection services)
        {
            services.AddApiVersioning(options => { options.ReportApiVersions = true; })
            .AddMvc()
            .AddApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
        }
    }
}
