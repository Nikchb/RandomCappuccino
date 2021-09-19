using System.Collections.Generic;

namespace RandomCappuccino.Server.Services
{
    public class ServiceBase<T>
    {
        public ServiceContentResponse<T> Accept()
        {
            return new ServiceContentResponse<T>(true);
        }

        public ServiceContentResponse<T> Accept(T response)
        {
            return new ServiceContentResponse<T>(response);
        }

        public ServiceContentResponse<T> Decline()
        {
            return new ServiceContentResponse<T>(false);
        }

        public ServiceContentResponse<T> Decline(params string[] messages)
        {
            return new ServiceContentResponse<T>(messages);
        }

        public ServiceContentResponse<T> Decline(IEnumerable<string> messages)
        {
            return new ServiceContentResponse<T>(messages);
        }
    }
}
