using Microsoft.AspNetCore.Http;
using System.Linq;

namespace RandomCappuccino.Server.Services.IdentityManager
{
    public class IdentityManager : IIdentityManager
    {
        public string UserId { get; private set; }

        public IdentityManager(IHttpContextAccessor contextAccessor)
        {
            UserId = contextAccessor.HttpContext.User.Claims.FirstOrDefault(v => v.Type == "UserId")?.Value;
        }
    }
}
