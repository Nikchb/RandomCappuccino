using System.Collections.Generic;
using System.Linq;

namespace RandomCappuccino.Server.Services
{
    public class ServiceContentResponse<T> : ServiceResponse
    {
        public T Content { get; set; }

        public ServiceContentResponse(bool succeed) : base(succeed)
        {
        }

        public ServiceContentResponse(T response) : base(succeed: true)
        {
            Content = response;
        }

        public ServiceContentResponse(params string[] messages) : base(messages)
        {
        }

        public ServiceContentResponse(IEnumerable<string> messages) : base(messages.ToArray())
        {
        }
    }
}
