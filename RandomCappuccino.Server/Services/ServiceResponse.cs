using System.Collections.Generic;
using System.Linq;

namespace RandomCappuccino.Server.Services
{
    public class ServiceResponse<T>
    {
        public bool Succeed { get; set; }

        public string[] Messages { get; set; }

        public T Response { get; set; }

        public ServiceResponse(params string[] messages)
        {
            Succeed = false;
            Messages = messages;
        }

        public ServiceResponse(IEnumerable<string> messages)
        {
            Succeed = false;
            Messages = messages.ToArray();
        }

        public ServiceResponse(T response)
        {
            Succeed = true;
            Response = response;
        }

        public ServiceResponse(bool succeed)
        {
            Succeed = succeed;           
        }
    }
}
