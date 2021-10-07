using Grpc.AspNetCore.Server;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Middlewares.RPCAuthenticationHandler
{
    public class RPCAuthenticationHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            await next(httpContext);

            if (httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized ||
               httpContext.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                if (httpContext.GetEndpoint()?.Metadata.GetMetadata<GrpcMethodMetadata>() != null)
                {
                    var statusCode = httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized ? StatusCode.Unauthenticated : StatusCode.PermissionDenied;
                    httpContext.Response.Headers.Add("grpc-status", statusCode.ToString("D"));
                    httpContext.Response.StatusCode = StatusCodes.Status200OK;
                }
            }
        }
    }
}
