using System.Collections.Generic;
using System.Linq;

namespace RandomCappuccino.Client.Services
{
    public class MessageManager
    {
        private readonly List<ViewMessage> messages;

        public MessageManager()
        {
            messages = new List<ViewMessage>();            
        }

        public delegate void MessageListHandler();

        public event MessageListHandler MessageListChanged;

        public IReadOnlyCollection<ViewMessage> Messages => messages.AsReadOnly();

        public void AddErrorMessages(params string[] messages)
        {
            AddErrorMessages(messages.AsEnumerable());
        }

        public void AddErrorMessages(IEnumerable<string> messages)
        {
            this.messages.AddRange(messages.Select(v => new ViewMessage { Message = v, Type = ViewMessageType.Error }));
            if(MessageListChanged != null)
            {
                MessageListChanged.Invoke();
            }            
        }

        public void UpdateErrorMessages(params string[] messages)
        {
            UpdateErrorMessages(messages.AsEnumerable());
        }

        public void UpdateErrorMessages(IEnumerable<string> messages)
        {
            this.messages.Clear();
            AddErrorMessages(messages);
        }

        public void AddMessages(params string[] messages)
        {
            AddMessages(messages.AsEnumerable());
        }

        public void AddMessages(IEnumerable<string> messages)
        {
            this.messages.AddRange(messages.Select(v => new ViewMessage { Message = v, Type = ViewMessageType.Message }));
            if (MessageListChanged != null)
            {
                MessageListChanged.Invoke();
            }
        }

        public void UpdateMessages(params string[] messages)
        {
            UpdateMessages(messages.AsEnumerable());
        }

        public void UpdateMessages(IEnumerable<string> messages)
        {
            this.messages.Clear();
            AddMessages(messages);
        }

        public void AddSuccessMessages(params string[] messages)
        {
            AddSuccessMessages(messages.AsEnumerable());
        }

        public void AddSuccessMessages(IEnumerable<string> messages)
        {
            this.messages.AddRange(messages.Select(v => new ViewMessage { Message = v, Type = ViewMessageType.Success }));
            if (MessageListChanged != null)
            {
                MessageListChanged.Invoke();
            }
        }

        public void UpdateSuccessMessages(params string[] messages)
        {
            UpdateSuccessMessages(messages.AsEnumerable());
        }

        public void UpdateSuccessMessages(IEnumerable<string> messages)
        {
            this.messages.Clear();
            AddSuccessMessages(messages);
        }
    }
}
