using Grpc.Core;
using Microsoft.AspNetCore.Components;
using RandomCappuccino.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RandomCappuccino.Shared.TourService;
using static RandomCappuccino.Shared.GroupService;

namespace RandomCappuccino.Client.Pages
{
    public partial class Tour
    {
        [Inject]
        public GroupServiceClient GroupService { get; set; }

        [Inject]
        public TourServiceClient TourService { get; set; }

        private ExtendedTourInfo TourInfo { get; set; }

        private GroupInfo GroupInfo { get; set; }

        [Parameter]
        public string TourId {  get; set; }

        [Parameter]
        public string GroupId { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            try
            {
                var response = await TourService.GetTourAsync(new GetTourRequest { Id = TourId });

                MessageManager.UpdateErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    TourInfo = response.Tour;

                    var groupResponse = await GroupService.GetGroupAsync(new GetGroupRequest { Id = GroupId });

                    MessageManager.AddErrorMessages(response.Messages);
                    if (response.Succeed)
                    {
                        GroupInfo = groupResponse.Group;
                    }
                }
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
            }
        }

        private async Task DeleteTour()
        {
            try
            {
                var response = await TourService.DeleteTourAsync(new DeleteTourRequest { Id = TourId });

                MessageManager.UpdateErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    NavigateBackToGroup();
                }
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
            }
        }

        private void NavigateBackToGroup()
        {
            NavigationManager.NavigateTo($"/group/{GroupId}");
        }
    }
}
