using System.Collections.Generic;

namespace RandomCappuccino.Server.Services
{
    public class ServiceBase<T>
    {
        public ServiceResponse Accept()
        {
            return new ServiceResponse(true);
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

        public ServiceContentResponse<TT> Accept<TT>(TT response)
        {
            return new ServiceContentResponse<TT>(response);
        }

        public ServiceContentResponse<TT> Decline<TT>()
        {
            return new ServiceContentResponse<TT>(false);
        }

        public ServiceContentResponse<TT> Decline<TT>(params string[] messages)
        {
            return new ServiceContentResponse<TT>(messages);
        }
    }
}
