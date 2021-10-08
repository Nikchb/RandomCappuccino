using Grpc.Core;
using Microsoft.AspNetCore.Components;
using RandomCappuccino.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using static RandomCappuccino.Shared.GroupService;

namespace RandomCappuccino.Client.Pages
{
    public partial class Groups
    {
        [Inject]
        public GroupServiceClient GroupService {  get; set; }

        private readonly List<GroupInfo> groups;

        private string NewGroupName { get; set; } = "";

        public Groups()
        {
            groups = new List<GroupInfo>();            
        }

        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            try
            {
                var response = await GroupService.GetGroupsAsync(new GetGroupsRequest());

                MessageManager.UpdateErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    groups.AddRange(response.Groups);
                }
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
            }
        }

        private async Task AddGroup()
        {
            try
            {
                var response = await GroupService.CreateGroupAsync(new CreateGroupRequest { Name = NewGroupName });

                MessageManager.UpdateErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    groups.Add(response.Group);
                    NewGroupName = "";
                    MessageManager.UpdateSuccessMessages("Group is successfully created");
                }
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
            }
        }    
        
        private void NavigateToGroup(string id)
        {
            NavigationManager.NavigateTo($"/group/{id}");
        }
    }
}
