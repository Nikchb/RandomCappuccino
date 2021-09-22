using RandomCappuccino.Server.Services.UserManager.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.UserManager
{
    public interface IUserManager
    {
        Task<ServiceContentResponse<UserDTO>> CreateUser(CreateUserDTO model);

        Task<ServiceContentResponse<UserDTO>> GetUserInfo();

        Task<ServiceContentResponse<UserDTO>> UpdateUserInfo(UpdateUserInfoDTO model);

        Task<ServiceResponse> UpdateUserPassword(UpdateUserPasswordDTO model);

        Task<ServiceContentResponse<UserDTO>> CheckPassword(string email, string password);

        Task<ServiceContentResponse<IEnumerable<string>>> GetUserRoles();

        Task<ServiceContentResponse<IEnumerable<string>>> GetUserRoles(string userId);

        Task<ServiceResponse> AddUserRoles(params string[] roles);     

        Task<ServiceResponse> RemoveUserRoles(params string[] roles);
    }
}
