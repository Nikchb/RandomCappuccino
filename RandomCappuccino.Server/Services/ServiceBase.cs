using System.Collections.Generic;

namespace RandomCappuccino.Server.Services
{
    public class ServiceBase
    {
        public ServiceResponse Accept()
        {
            return new ServiceResponse(true);
        }

        public ServiceResponse Decline()
        {
            return new ServiceResponse(false);
        }

        public ServiceResponse Decline(params string[] messages)
        {
            return new ServiceResponse(messages);
        }

        public ServiceContentResponse<T> Accept<T>(T response)
        {
            return new ServiceContentResponse<T>(response);
        }

        public ServiceContentResponse<T> Decline<T>()
        {
            return new ServiceContentResponse<T>(false);
        }

        public ServiceContentResponse<T> Decline<T>(params string[] messages)
        {
            return new ServiceContentResponse<T>(messages);
        }
    }
}
