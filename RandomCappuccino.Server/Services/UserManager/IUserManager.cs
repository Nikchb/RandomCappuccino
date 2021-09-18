using RandomCappuccino.Server.Services.UserManager.DTOs;

namespace RandomCappuccino.Server.Services.UserManager
{
    public interface IUserManager
    {
        public ServiceResponse<UserDTO> GetCurrentUser();

    }
}
