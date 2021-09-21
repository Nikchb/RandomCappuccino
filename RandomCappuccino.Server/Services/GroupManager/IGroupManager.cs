using RandomCappuccino.Server.Services.GroupManager.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.GroupManager
{
    public interface IGroupManager
    {
        Task<ServiceContentResponse<GroupDTO>> CreateGroup(string userId, CreateGroupDTO model);

        Task<ServiceContentResponse<GroupDTO>> UpdateGroup(string userId, GroupDTO model);

        Task<ServiceContentResponse<IEnumerable<GroupDTO>>> GetGroups(string userId);

        Task<ServiceResponse> DeleteGroup(string userId, string groupId);        
    }
}
