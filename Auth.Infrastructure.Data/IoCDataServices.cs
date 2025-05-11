using Auth.Domain.Core.Common.Extensions;
using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Data.Queues;
using Auth.Domain.Interface.Data.Read.Cache;
using Auth.Domain.Interface.Data.Read.Locks;
using Auth.Domain.Interface.Data.Read.Queues;
using Auth.Infrastructure.Data.Cache;
using Auth.Infrastructure.Data.Locks;
using Auth.Infrastructure.Data.Queues;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Auth.Infrastructure.Data
{
    public static class IoCDataServices
    {
        public static bool IsMemoryDB = false;
        public static IServiceCollection RegistrationDataServices(this IServiceCollection services, IHostEnvironment environment, ConnectionOptions connectionString)
        {
            services.AddServices();
            services.AddSqlOrInMemory<AuthDBContext>(environment.IsDevelopment(), connectionString?.Database);
            services.AddSqlOrInMemory<AuthReadDBContext>(environment.IsDevelopment(), connectionString?.DatabaseRead ?? connectionString?.Database);
            services.AddDistributedCache(connectionString?.Redis);
            return services;
        }
        public static void UseDataServices(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AuthDBContext>();
                context.UseScripts();
            }
        }
        private static void UseScripts(this AuthDBContext context)
        {
            if (IsMemoryDB) return;
            var solutionPath = Directory.GetParent(@".");

            var fullPath = solutionPath.GetDirectories("Scripts", 
                SearchOption.AllDirectories).FirstOrDefault().FullName;
            
            foreach (var file in Directory.EnumerateFiles(fullPath).Order())
            {
                using (var reader = new StreamReader(Path.Combine(fullPath, file)))
                {
                    context.Database.ExecuteSqlRaw(reader.ReadToEnd());
                }
            }
        }
        private static IServiceCollection AddDistributedCache(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                ConsoleExtension.Errors("No cache connection string!!!",
                    "Switching to the memory cache");

                return services.AddMemoryCache();
            }
            if (!IsRedisConnectionAvailable(connectionString))
            {
                ConsoleExtension.Errors("Invalid cache connection string!!!",
                    "Switching to the memory cache");

                return services.AddMemoryCache();
            }
            return services.AddRedisCache(connectionString);
        }
        private static IServiceCollection AddRedisCache(this IServiceCollection services, string connectionString)
        {
            var cacheName = Assembly.GetCallingAssembly().GetName().Name + "DistributedCache";
            return services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = cacheName;
                options.ConfigurationOptions = new()
                {
                    ReconnectRetryPolicy = new LinearRetry(5000),
                    ConnectRetry = 3,
                    AbortOnConnectFail = false
                };
            });
        }
        private static IServiceCollection AddMemoryCache(this IServiceCollection services)
        {
            return services.AddDistributedMemoryCache();
        }
        private static bool IsRedisConnectionAvailable(string connectionString)
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(connectionString))
                {
                    var db = redis.GetDatabase();
                    db.Ping();
                } 
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static IServiceCollection AddSqlOrInMemory<T>(this IServiceCollection services, bool isDevelopment, string database) where T : BaseDBContext
        {
            if (string.IsNullOrEmpty(database))
            {
                ConsoleExtension.Errors("No database connection string!!!");
                if (!isDevelopment)
                    throw new ArgumentNullException("No database connection string");
                ConsoleExtension.Errors("Switching to the memory database");
                return AddSqlInMemory<T>(services, isDevelopment, typeof(T).Name);
            }
            if (IsDatabaseConnectionAvailable(database))
                return AddSqlServer<T>(services, isDevelopment, database);

            ConsoleExtension.Errors("Invalid database connection string!!!");
            if (!isDevelopment)
                throw new InvalidDataException("Invalid database connection string!");
            ConsoleExtension.Errors("Switching to the memory database");
            return AddSqlInMemory<T>(services, isDevelopment, typeof(T).Name);
        }
        private static IServiceCollection AddSqlInMemory<T>(this IServiceCollection services, bool isDevelopment, string database) where T : BaseDBContext
        {
            var migratioName = Assembly.GetCallingAssembly().GetName().Name;
            IsMemoryDB = true;
            return services.AddDbContext<T>(options =>
            {
                options.UseInMemoryDatabase(database);
                if (isDevelopment)
                    options.EnableSensitiveDataLogging();
                options.LogTo(Console.Error.WriteLine, LogLevel.Error, DbContextLoggerOptions.DefaultWithUtcTime).EnableDetailedErrors();

            });
        }
        private static IServiceCollection AddSqlServer<T>(this IServiceCollection services, bool isDevelopment, string database) where T : BaseDBContext
        {
            if (string.IsNullOrEmpty(database))
                throw new ArgumentNullException(nameof(database));
            var migratioName = Assembly.GetCallingAssembly().GetName().Name;
            return services.AddDbContext<T>(options =>
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
        private static bool IsDatabaseConnectionAvailable(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Close();
                }
                return true;
            }
            catch
            {
                return TryCreateDB(connectionString);
            }
        }
        private static bool TryCreateDB(string connectionString)
        {
            try
            {
                if (connectionString.Contains("ApplicationIntent=ReadOnly"))
                    return false;

                string databaseName = connectionString.GetBetween("Database=", ";");
                string serverConnection = connectionString.Replace($"Database={databaseName};", "");
                if ((!Regex.IsMatch(databaseName, @"^[\w-]+$")))
                {
                    ConsoleExtension.Errors("Invalid database name");
                    return false;
                }
                    
                using (var connection = new SqlConnection(serverConnection))
                {
                    connection.Open();
                    string commandText = $@"
                        IF NOT EXISTS (SELECT name FROM master.sys.databases
                        WHERE name = '{databaseName}')
                        BEGIN
                            CREATE DATABASE {databaseName}
                        END";

                    using (var command = new SqlCommand(commandText, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex) 
            {
                Console.Error.WriteLine(nameof(TryCreateDB), ex);
                return false;
            }
        }
        private static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IAsyncSynchronization, AsyncSynchronization>();
            services.AddSingleton<IQueueRepository<WaitingPaymentApproval>, QueueRepository<WaitingPaymentApproval>>();
            services.AddScoped<ICacheRepository, CacheRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<IUnitOfWorkRead, UnitOfWork.UnitOfWorkRead>();
        }
    }
}
