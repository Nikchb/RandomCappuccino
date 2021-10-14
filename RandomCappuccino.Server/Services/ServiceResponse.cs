using System;

namespace RandomCappuccino.Server.Services
{
    public class ServiceResponse
    {
        public bool Succeed { get; } 

        public string[] Messages { get; } = Array.Empty<string>();

        public ServiceResponse(bool succeed)
        {
            Succeed = succeed;
        }

        public ServiceResponse(params string[] messages)
        {
            Succeed = false;
            Messages = messages;
        }
    }
}
