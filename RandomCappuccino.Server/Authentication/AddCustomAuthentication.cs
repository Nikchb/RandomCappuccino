using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RandomCappuccino.Server.Authentication
{
    public static partial class ServiceProviderExtensions
    {
        public static void AddCustomAuthentication(this IServiceCollection services)
        {           
            var tokenManager = new TokenManager(
                issuerSigningKey: Environment.GetEnvironmentVariable("AUTH_KEY") ?? "DefaultKey",
                validIssuer: Environment.GetEnvironmentVariable("AUTH_ISSUER") ?? "DefaultIssuer",
                validAudience: Environment.GetEnvironmentVariable("AUTH_AUDIENCE") ?? "DefaultAudience",
                clockSkewMinutes: int.Parse(Environment.GetEnvironmentVariable("AUTH_MINUTES") ?? "4320"));

            services.AddSingleton(tokenManager);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = tokenManager.GetTokenValidationParameters();
            });
        }
    }
}
