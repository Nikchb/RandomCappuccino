using AutoMapper;

using RandomCappuccino.Server.Data;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.GroupManager;
using RandomCappuccino.Server.Services.ParticipantManager.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.ParticipantManager
{
    public class ParticipantManager : ServiceBase, IParticipantManager
    {
        private readonly IMapper mapper;
        private readonly DataBaseContext context;
        private readonly IGroupManager groupManager;

        public ParticipantManager(IMapper mapper, DataBaseContext context, IGroupManager groupManager)
        {
            this.mapper = mapper;
            this.context = context;
            this.groupManager = groupManager;
        }

        public async Task<ServiceContentResponse<ParticipantDTO>> CreateParticipant(string userId, CreateParticipantDTO model)
        {
            

            var participant = mapper.Map<Participant>(model);

            try
            {
                await context.Participants.AddAsync(participant);
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline<ParticipantDTO>("Participant creation failed");
            }

            return Accept(mapper.Map<ParticipantDTO>(participant));
        }

        public async Task<ServiceResponse> DeleteParticipant(string userId, string participantId)
        {


            throw new System.NotImplementedException();


        }

        public Task<ServiceContentResponse<IEnumerable<ParticipantDTO>>> GetGroupParticipants(string userId, string groupId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceContentResponse<ParticipantDTO>> GetParticipant(string userId, string partcipantId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceContentResponse<ParticipantDTO>> UpdateParticipant(string userId, ParticipantDTO model)
        {
            throw new System.NotImplementedException();
        }
    }
}
