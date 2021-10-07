using Grpc.Core;
using Microsoft.AspNetCore.Components;
using RandomCappuccino.Client.Services;
using System.Collections.Generic;
using System.Linq;

namespace RandomCappuccino.Client.Components
{
    public class BasePage : ComponentBase
    {
        protected readonly List<ViewMessage> messages = new List<ViewMessage>();

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public AuthenticationService AuthenticationService { get; set; }    
        
        protected void AddErrorMessages(params string[] messages)
        {
            AddErrorMessages(messages.AsEnumerable());
        }

        protected void AddErrorMessages(IEnumerable<string> messages)
        {
            this.messages.AddRange(messages.Select(v=> new ViewMessage { Message = v, Type = ViewMessageType.Error }));
        }

        protected void UpdateErrorMessages(params string[] messages)
        {
            UpdateErrorMessages(messages.AsEnumerable());
        }

        protected void UpdateErrorMessages(IEnumerable<string> messages)
        {
            this.messages.Clear();
            AddErrorMessages(messages);
        }

        protected void AddMessages(params string[] messages)
        {
            AddMessages(messages.AsEnumerable());
        }

        protected void AddMessages(IEnumerable<string> messages)
        {
            this.messages.AddRange(messages.Select(v => new ViewMessage { Message = v, Type = ViewMessageType.Message }));
        }

        protected void UpdateMessages(params string[] messages)
        {
            UpdateMessages(messages.AsEnumerable());
        }

        protected void UpdateMessages(IEnumerable<string> messages)
        {
            this.messages.Clear();
            AddMessages(messages);
        }

        protected void AddSuccessMessages(params string[] messages)
        {
            AddSuccessMessages(messages.AsEnumerable());
        }

        protected void AddSuccessMessages(IEnumerable<string> messages)
        {
            this.messages.AddRange(messages.Select(v => new ViewMessage { Message = v, Type = ViewMessageType.Success }));
        }

        protected void UpdateSuccessMessages(params string[] messages)
        {
            UpdateSuccessMessages(messages.AsEnumerable());
        }

        protected void UpdateSuccessMessages(IEnumerable<string> messages)
        {
            this.messages.Clear();
            AddSuccessMessages(messages);
        }

        protected void HandleRPCExection(RpcException ex)
        {
            if(ex.StatusCode == StatusCode.Unauthenticated)
            {
                AuthenticationService.RemoveToken();
                NavigationManager.NavigateTo("/sign-in");
            }
            if (ex.StatusCode == StatusCode.PermissionDenied)
            {
                UpdateErrorMessages("Permission denied");
            }

        }
    }    
}
