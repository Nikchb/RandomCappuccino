using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using RandomCappuccino.Server.Services.ParticipantManager;
using RandomCappuccino.Server.Services.ParticipantManager.DTOs;
using RandomCappuccino.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.RPC
{
    [Authorize]
    public class ParticipantServiceProvider : ParticipantService.ParticipantServiceBase
    {
        private readonly IParticipantManager participantManager;
        private readonly IMapper mapper;

        public ParticipantServiceProvider(IParticipantManager participantManager, IMapper mapper)
        {
            this.participantManager = participantManager;
            this.mapper = mapper;
        }

        public async override Task<ParticipantsResponse> GetParticipants(GetParticipantsRequest request, ServerCallContext context)
        {
            var managerResponse = await participantManager.GetGroupParticipants(request.GroupId);

            var response = new ParticipantsResponse();           

            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);
            if (managerResponse.Content != null)
            {
                response.Participants.AddRange(managerResponse.Content.Select(v => mapper.Map<ParticipantInfo>(v)));
            }

            return response;
        }
        public async override Task<ParticipantResponse> CreateParticipant(CreateParticipantRequest request, ServerCallContext context)
        {
            var managerResponse = await participantManager.CreateParticipant(mapper.Map<CreateParticipantDTO>(request));

            var response = new ParticipantResponse();

            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);
            if (managerResponse.Content != null)
            {
                response.Participant = mapper.Map<ParticipantInfo>(managerResponse.Content);
            }

            return response;
        }

        public async override Task<ParticipantResponse> GetParticipant(GetParticipantRequest request, ServerCallContext context)
        {
            var managerResponse = await participantManager.GetParticipant(request.Id);

            var response = new ParticipantResponse();

            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);
            if (managerResponse.Content != null)
            {
                response.Participant = mapper.Map<ParticipantInfo>(managerResponse.Content);
            }

            return response;
        }

        public async override Task<ParticipantServiceResponse> UpdateParticipant(UpdateParticipantRequest request, ServerCallContext context)
        {
            var managerResponse = await participantManager.UpdateParticipant(mapper.Map<ParticipantDTO>(request));

            var response = new ParticipantServiceResponse();

            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);           

            return response;
        }

        public async override Task<ParticipantServiceResponse> DeleteParticipant(DeleteParticipantRequest request, ServerCallContext context)
        {
            var managerResponse = await participantManager.DeleteParticipant(request.Id);

            var response = new ParticipantServiceResponse();

            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);

            return response;
        }
    }
}
