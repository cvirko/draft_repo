using Auth.Domain.Core.Common.Extensions;
using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Interface.Data.Read.Cache;
using Auth.Infrastructure.Data.Cache;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Auth.Infrastructure.Data
{
    public static class IoCDataServices
    {
        public static void RegistrationDBService(this IServiceCollection services, IHostEnvironment environment, ConnectionOptions connectionString)
        {
            RegistrationAccountingService(services);
            AddSqlOrInMemory<AuthDBContext>(services, environment.IsDevelopment(), connectionString?.Database);
            AddSqlOrInMemory<AuthReadDBContext>(services, environment.IsDevelopment(), connectionString?.DatabaseRead ?? connectionString?.Database);
            AddDistributedCache(services, connectionString?.Redis);
        }
        private static void AddDistributedCache(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                ConsoleExtension.Errors("No cache connection string!!!", 
                    "Switching to the memory database");

                services.AddMemoryCache();
            }
            else
            {
                try
                {
                    services.AddRedisCache(connectionString);
                }
                catch 
                {
                    ConsoleExtension.Errors("Invalid cache connection string!!!",
                        "Switching to the memory database");

                    services.AddMemoryCache();
                }
            }
                
        }
        private static void AddRedisCache(this IServiceCollection services, string connectionString)
        {
            var cacheName = Assembly.GetCallingAssembly().GetName().Name + "DistributedCache";
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = cacheName;
            });
        }
        private static void AddMemoryCache(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
        }

        private static void AddSqlOrInMemory<T>(IServiceCollection services, bool isDevelopment, string database) where T : BaseDBContext
        {
            try
            {
                AddSqlServer<T>(services, isDevelopment, database);
            }
            catch 
            {
                ConsoleExtension.Errors("Invalid database connection string!!!");
                if (!isDevelopment) 
                    throw;
                ConsoleExtension.Errors("Switching to the temporary database");
                AddSqlInMemory<T>(services, isDevelopment, typeof(T).Name);
            }

        }
        private static void AddSqlInMemory<T>(IServiceCollection services, bool isDevelopment, string database) where T : BaseDBContext
        {
            var migratioName = Assembly.GetCallingAssembly().GetName().Name;
            services.AddDbContext<T>(options =>
            {
                options.UseInMemoryDatabase(database);
                if (isDevelopment)
                    options.EnableSensitiveDataLogging();
                options.LogTo(Console.Error.WriteLine, LogLevel.Error, DbContextLoggerOptions.DefaultWithUtcTime).EnableDetailedErrors();

            });
        }
        private static void AddSqlServer<T>(IServiceCollection services, bool isDevelopment, string database) where T : BaseDBContext
        {
            if (string.IsNullOrEmpty(database))
                throw new ArgumentNullException(nameof(database));
            var migratioName = Assembly.GetCallingAssembly().GetName().Name;
            services.AddDbContext<T>(options =>
            {
                options.UseSqlServer(database,
                    b =>
                    {
                        b.MigrationsAssembly(migratioName);
                        b.CommandTimeout(60);
                        b.EnableRetryOnFailure(
                            maxRetryCount: 2,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
                if (isDevelopment)
                    options.EnableSensitiveDataLogging();
                options.LogTo(Console.Error.WriteLine, LogLevel.Error, DbContextLoggerOptions.DefaultWithUtcTime).EnableDetailedErrors();

            });
        }
        private static void RegistrationAccountingService(this IServiceCollection services)
        {
            services.AddScoped<ICacheRepository, CacheRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<IUnitOfWorkRead, UnitOfWork.UnitOfWorkRead>();
        }
    }
}
