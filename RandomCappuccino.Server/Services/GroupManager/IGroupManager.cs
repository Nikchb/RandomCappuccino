using RandomCappuccino.Server.Services.GroupManager.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.GroupManager
{
    public interface IGroupManager
    {
        Task<ServiceContentResponse<GroupDTO>> CreateGroup(CreateGroupDTO model);

        Task<ServiceResponse> UpdateGroup(GroupDTO model);

        Task<ServiceContentResponse<IEnumerable<GroupDTO>>> GetGroups();

        Task<ServiceContentResponse<GroupDTO>> GetGroup(string groupId);

        Task<ServiceResponse> DeleteGroup(string groupId);        
    }
}
