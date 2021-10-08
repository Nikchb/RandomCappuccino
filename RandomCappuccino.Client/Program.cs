using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using RandomCappuccino.Shared;
using Blazored.LocalStorage;
using RandomCappuccino.Client.Services;

namespace RandomCappuccino.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped<MessageManager>();

            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped(services =>
            {
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                return httpClient;
            });

            builder.Services.AddScoped(services =>
            {
                return new AuthenticationService(
                    services.GetRequiredService<ISyncLocalStorageService>(),
                    services.GetRequiredService<HttpClient>());
            });

            builder.Services.AddScoped(services =>
            {
                var httpClient = services.GetRequiredService<HttpClient>();
                return GrpcChannel.ForAddress(httpClient.BaseAddress, new GrpcChannelOptions { HttpClient = httpClient });
            });
            
            builder.Services.AddScoped(services =>
            {                
                return new SignService.SignServiceClient(services.GetRequiredService<GrpcChannel>());
            });

            builder.Services.AddScoped(services =>
            {
                return new UserService.UserServiceClient(services.GetRequiredService<GrpcChannel>());
            });

            builder.Services.AddScoped(services =>
            {
                return new GroupService.GroupServiceClient(services.GetRequiredService<GrpcChannel>());
            });

            builder.Services.AddScoped(services =>
            {
                return new ParticipantService.ParticipantServiceClient(services.GetRequiredService<GrpcChannel>());
            });

            await builder.Build().RunAsync();
        }
    }
}
