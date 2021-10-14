using Grpc.Core;
using Microsoft.AspNetCore.Components;
using RandomCappuccino.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RandomCappuccino.Shared.GroupService;
using static RandomCappuccino.Shared.ParticipantService;
using static RandomCappuccino.Shared.TourService;

namespace RandomCappuccino.Client.Pages
{
    public partial class Group
    {
        [Inject]
        public GroupServiceClient GroupService { get; set; }

        [Inject]
        public ParticipantServiceClient ParticipantService { get; set; }

        [Inject]
        public TourServiceClient TourService { get; set; }

        [Parameter]
        public string GroupId { get; set; }

        private readonly List<ParticipantInfo> participants;

        private readonly List<TourInfo> tours;

        private string NewParticipantName { get; set; } = "";       

        private string GroupName { get; set; } = "";

        private GroupInfo GroupInfo { get; set; }        

        public Group()
        {
            participants = new List<ParticipantInfo>();
            tours = new List<TourInfo>();
        }

        private void ResetChanges()
        {
            GroupName = GroupInfo.Name;
            NewParticipantName = "";
        }

        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            try
            {
                var response = await GroupService.GetGroupAsync(new GetGroupRequest { Id = GroupId });

                MessageManager.UpdateErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    GroupInfo = response.Group;
                    ResetChanges();
                }
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
            }

            try
            {
                var response = await TourService.GetToursAsync(new GetToursRequest { GroupId = GroupId });

                MessageManager.AddErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    tours.AddRange(response.Tours);
                }
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
            }

            try
            {
                var response = await ParticipantService.GetParticipantsAsync(new GetParticipantsRequest { GroupId = GroupId });

                MessageManager.AddErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    participants.AddRange(response.Participants);
                }
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
            }

        }

        private async Task UpdateGroup()
        {
            try
            {
                var response = await GroupService.UpdateGroupAsync(new UpdateGroupRequest { Id = GroupId, Name = GroupName });

                MessageManager.UpdateErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    GroupInfo.Name = GroupName;
                    ResetChanges();
                    MessageManager.UpdateSuccessMessages("Group information is successfully updated");
                }
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
            }
        }

        private async Task DeleteGroup()
        {
            try
            {
                var response = await GroupService.DeleteGroupAsync(new DeleteGroupRequest { Id = GroupId });

                MessageManager.UpdateErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    NavigationManager.NavigateTo("/groups");
                }
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
            }
        }

        private async Task AddParticiapant()
        {
            try
            {
                var response = await ParticipantService.CreateParticipantAsync(new CreateParticipantRequest { GroupId = GroupId, Name = NewParticipantName });

                MessageManager.UpdateErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    participants.Add(response.Participant);
                    ResetChanges();
                    MessageManager.UpdateSuccessMessages("Participant is successfully created");
                }
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
            }
        }

        private async Task UpdateParticipant(ParticipantInfo participant)
        {
            try
            {                
                var response = await ParticipantService.UpdateParticipantAsync(new UpdateParticipantRequest { Id = participant.Id, Name = participant.Name });

                MessageManager.UpdateErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    if (participant != null)
                    {
                        MessageManager.UpdateSuccessMessages("Participant information is successfully updated");
                    }
                }
                else
                {
                    var getResponse = await ParticipantService.GetParticipantAsync(new GetParticipantRequest { Id = participant.Id });

                    MessageManager.AddErrorMessages(getResponse.Messages);
                    if (getResponse.Succeed)
                    {                                              
                        participant.Name = getResponse.Participant.Name;                        
                    }
                }
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
            }
        }

        private async Task DeleteParticipant(string id)
        {
            try
            {
                var response = await ParticipantService.DeleteParticipantAsync(new DeleteParticipantRequest { Id = id });

                MessageManager.UpdateErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    participants.RemoveAll(v => v.Id == id);
                    MessageManager.UpdateSuccessMessages("Participant is successfully removed");
                }
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
            }
        }

        private async Task AddTour()
        {
            try
            {
                var response = await TourService.CreateTourAsync(new CreateTourRequest { GroupId = GroupId });

                MessageManager.UpdateErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    tours.Insert(0, new TourInfo { Id = response.Tour.Id, CreationTime = response.Tour.CreationTime });
                    MessageManager.UpdateSuccessMessages("A new tour is successfully created");
                }
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
            }
        }
    }
}
