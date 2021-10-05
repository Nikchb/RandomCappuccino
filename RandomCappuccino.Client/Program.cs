using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using RandomCappuccino.Shared;

namespace RandomCappuccino.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddSingleton(new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())));
            

            builder.Services.AddSingleton(services =>
            {                             
                var channel = GrpcChannel.ForAddress(builder.HostEnvironment.BaseAddress, new GrpcChannelOptions { HttpClient = services.GetRequiredService<HttpClient>() });
                return new SignService.SignServiceClient(channel);
            });

            await builder.Build().RunAsync();
        }
    }
}
