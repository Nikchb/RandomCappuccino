namespace RandomCappuccino.Client.Components
{
    public class ViewMessage
    {
        public string Message {  get; set; }
        public ViewMessageType Type {  get; set; }
    }

    public enum ViewMessageType : uint
    {
        Error = 0,
        Message = 1,
        Success = 2
    }
}
