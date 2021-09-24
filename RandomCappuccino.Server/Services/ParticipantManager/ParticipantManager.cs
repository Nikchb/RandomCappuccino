using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RandomCappuccino.Server.Data;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.GroupManager;
using RandomCappuccino.Server.Services.IdentityManager;
using RandomCappuccino.Server.Services.ParticipantManager.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.ParticipantManager
{
    public class ParticipantManager : ServiceBase, IParticipantManager
    {
        private readonly IMapper mapper;
        private readonly DataBaseContext context;
        private readonly IGroupManager groupManager;
        private readonly IIdentityManager identityManager;

        public ParticipantManager(IMapper mapper, DataBaseContext context, IGroupManager groupManager, IIdentityManager identityManager)
        {
            this.mapper = mapper;
            this.context = context;
            this.groupManager = groupManager;
            this.identityManager = identityManager;
        }

        public async Task<ServiceContentResponse<ParticipantDTO>> CreateParticipant(CreateParticipantDTO model)
        {
            var groupResponse = await groupManager.GetGroup(model.GroupId);
            if(groupResponse.Succeed == false)
            {
                return Decline<ParticipantDTO>(groupResponse.Messages);
            } 
            
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

        public async Task<ServiceResponse> DeleteParticipant(string participantId)
        {
            var participant = await context.Participants.FindAsync(participantId);
            if (participant == null ? true : participant.IsActive == false)
            {
                Decline("Participant is not found");
            }

            var groupResponse = await groupManager.GetGroup(participant.GroupId);
            if (groupResponse.Succeed == false)
            {
                return Decline(groupResponse.Messages);
            }

            participant.IsActive = false;

            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline("Participant delete is failed");
            }

            return Accept();
        }

        public async Task<ServiceContentResponse<IEnumerable<ParticipantDTO>>> GetGroupParticipants(string groupId)
        {
            var groupResponse = await groupManager.GetGroup(groupId);
            if (groupResponse.Succeed == false)
            {
                return Decline<IEnumerable<ParticipantDTO>>(groupResponse.Messages);
            }

            try
            {
                var participants = await context.Participants.Where(v => v.GroupId == groupId && v.IsActive)
                                                             .Select(v => mapper.Map<ParticipantDTO>(v)).ToArrayAsync();
                return Accept(participants.AsEnumerable());
            }
            catch
            {
                return Decline<IEnumerable<ParticipantDTO>>("Participant request is failed");
            }
        }

        public async Task<ServiceContentResponse<ParticipantDTO>> GetParticipant(string partiicpantId)
        {
            var participant = await context.Participants.FindAsync(partiicpantId);
            if (participant == null ? true : participant.IsActive == false)
            {
                Decline<ParticipantDTO>("Participant is not found");
            }

            var groupResponse = await groupManager.GetGroup(participant.GroupId);
            if (groupResponse.Succeed == false)
            {
                return Decline<ParticipantDTO>(groupResponse.Messages);
            }

            return Accept(mapper.Map<ParticipantDTO>(participant));
        }

        public async Task<ServiceResponse> UpdateParticipant(ParticipantDTO model)
        {
            var participant = await context.Participants.FindAsync(model.Id);
            if (participant == null ? true : participant.IsActive == false)
            {
                Decline("Participant is not found");
            }

            var groupResponse = await groupManager.GetGroup(participant.GroupId);
            if (groupResponse.Succeed == false)
            {
                return Decline(groupResponse.Messages);
            }

            mapper.Map(model, participant);

            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline("Participant update is failed");
            }

            return Accept();
        }
    }
}
