namespace RandomCappuccino.Server.Services
{
    public class ServiceResponse
    {
        public bool Succeed { get; }

        public string[] Messages { get; }

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
