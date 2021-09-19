using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RandomCappuccino.Server.Data
{
    public static partial class ServiceProviderExtensions
    {
        public static void AddDataBaseContext(this IServiceCollection services)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5442";
            var username = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres";

            var connectionString = $"Host={host};Port={port};Database=RandomCappuccinodb;Username={username};Password={password}";
            
            services.AddDbContext<DataBaseContext>(options => options.UseNpgsql(connectionString));
        }            
    }
}
