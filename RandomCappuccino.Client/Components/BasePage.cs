using Grpc.Core;
using Microsoft.AspNetCore.Components;
using RandomCappuccino.Client.Services;
using System.Collections.Generic;
using System.Linq;

namespace RandomCappuccino.Client.Components
{
    public class BasePage : ComponentBase
    {
        [Inject]
        public MessageManager MessageManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public AuthenticationService AuthenticationService { get; set; }    
       
        protected void HandleRPCExection(RpcException ex)
        {
            if(ex.StatusCode == StatusCode.Unauthenticated)
            {
                AuthenticationService.RemoveToken();
                NavigationManager.NavigateTo("/sign-in");
            }
            if (ex.StatusCode == StatusCode.PermissionDenied)
            {
                MessageManager.UpdateErrorMessages("Permission denied");
            }
        }
    }    
}
