using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RandomCappuccino.Server.Authentication;
using RandomCappuccino.Server.Data;
using RandomCappuccino.Server.Mapper;
using RandomCappuccino.Server.Middlewares.RPCAuthenticationHandler;
using RandomCappuccino.Server.RPC;
using RandomCappuccino.Server.Services.GroupManager;
using RandomCappuccino.Server.Services.IdentityManager;
using RandomCappuccino.Server.Services.ParticipantManager;
using RandomCappuccino.Server.Services.SignManager;
using RandomCappuccino.Server.Services.TourManager;
using RandomCappuccino.Server.Services.UserManager;
using System;

namespace RandomCappuccino.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataBaseContext();

            services.AddAuthorization();
            services.AddCustomAuthentication();

            services.AddAutoMapper();

            services.AddHttpContextAccessor();
            services.AddScoped<IIdentityManager, IdentityManager>();
            services.AddScoped<ISignManager, SignManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IParticipantManager, ParticipantManager>();
            services.AddScoped<IGroupManager, GroupManager>();
            services.AddScoped<ITourManager, TourManager>();

            services.AddScoped<RPCAuthenticationHandlerMiddleware>();
            
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseDeveloperExceptionPage();               
            }
            else
            {
                if(Environment.GetEnvironmentVariable("AUTH_KEY") == null)
                {
                    throw new ArgumentNullException("AUTH_KEY is not defined");
                }
            }

            app.UseStaticFiles();

            app.UseBlazorFrameworkFiles();            

            app.UseRouting();

            app.UseMiddleware<RPCAuthenticationHandlerMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseGrpcWeb();

            app.UseEndpoints(endpoints =>
            {                
                endpoints.MapGrpcService<UserServiceProvider>().EnableGrpcWeb();
                endpoints.MapGrpcService<SignServiceProvider>().EnableGrpcWeb();
                endpoints.MapGrpcService<GroupServiceProvider>().EnableGrpcWeb();
                endpoints.MapGrpcService<ParticipantServiceProvider>().EnableGrpcWeb();
                endpoints.MapGrpcService<TourServiceProvider>().EnableGrpcWeb();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
