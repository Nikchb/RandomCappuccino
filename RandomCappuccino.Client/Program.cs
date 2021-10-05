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
                var channel = GrpcChannel.ForAddress(httpClient.BaseAddress, new GrpcChannelOptions { HttpClient = httpClient });
                return new SignService.SignServiceClient(channel);
            });

            await builder.Build().RunAsync();
        }
    }
}
