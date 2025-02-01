namespace Auth.Api.Configuration
{
    public static class CorsConfig
    {
        public static void AddCorsCustom(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AppConsts.ALLOWED_SPECIFIC_ORIGIN,
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            //.AllowCredentials()
                            ;
                    });
            });
        }
        public static void UseCorsCustom(this WebApplication app)
        {
            app.UseCors(AppConsts.ALLOWED_SPECIFIC_ORIGIN);
        }
    }
}
