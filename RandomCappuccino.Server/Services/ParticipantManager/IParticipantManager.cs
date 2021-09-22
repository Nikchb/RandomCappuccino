using RandomCappuccino.Server.Services.ParticipantManager.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.ParticipantManager
{
    public interface IParticipantManager
    {
        Task<ServiceContentResponse<ParticipantDTO>> CreateParticipant(string userId, CreateParticipantDTO model);

        Task<ServiceContentResponse<ParticipantDTO>> GetParticipant(string userId, string partcipantId);

        Task<ServiceContentResponse<IEnumerable<ParticipantDTO>>> GetGroupParticipants(string userId, string groupId);

        Task<ServiceContentResponse<ParticipantDTO>> UpdateParticipant(string userId, ParticipantDTO model);

        Task<ServiceResponse> DeleteParticipant(string userId, string participantId);
    }
}
