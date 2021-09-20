using RandomCappuccino.Server.Services.UserManager.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.UserManager
{
    public interface IUserManager
    {
        Task<ServiceContentResponse<UserDTO>> CreateUser(CreateUserDTO model);

        Task<ServiceContentResponse<UserDTO>> GetUserInfo(string userId);

        Task<ServiceContentResponse<UserDTO>> UpdateUserInfo(string userId, UpdateUserInfoDTO model);

        Task<ServiceResponse> UpdateUserPassword(string userId, UpdateUserPasswordDTO model);

        Task<ServiceContentResponse<UserDTO>> CheckPassword(string email, string password);

        Task<ServiceContentResponse<IEnumerable<string>>> GetUserRoles(string userId);

        Task<ServiceResponse> AddUserRoles(string userId, params string[] roles);     

        Task<ServiceResponse> RemoveUserRoles(string userId, params string[] roles);
    }
}
