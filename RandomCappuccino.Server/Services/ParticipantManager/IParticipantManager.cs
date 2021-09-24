using RandomCappuccino.Server.Services.ParticipantManager.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.ParticipantManager
{
    public interface IParticipantManager
    {
        Task<ServiceContentResponse<ParticipantDTO>> CreateParticipant(CreateParticipantDTO model);

        Task<ServiceContentResponse<ParticipantDTO>> GetParticipant(string partcipantId);

        Task<ServiceContentResponse<IEnumerable<ParticipantDTO>>> GetGroupParticipants(string groupId);

        Task<ServiceResponse> UpdateParticipant(ParticipantDTO model);

        Task<ServiceResponse> DeleteParticipant(string participantId);
    }
}
