using System.Collections.Generic;

namespace RandomCappuccino.Server.Services
{
    public class ServiceBase<T>
    {
        public ServiceResponse<T> Accept()
        {
            return new ServiceResponse<T>(true);
        }

        public ServiceResponse<T> Accept(T response)
        {
            return new ServiceResponse<T>(response);
        }

        public ServiceResponse<T> Decline()
        {
            return new ServiceResponse<T>(false);
        }

        public ServiceResponse<T> Decline(params string[] messages)
        {
            return new ServiceResponse<T>(messages);
        }

        public ServiceResponse<T> Decline(IEnumerable<string> messages)
        {
            return new ServiceResponse<T>(messages);
        }
    }
}
