using AutoMapper;
using RandomCappuccino.Server.Authentication;
using RandomCappuccino.Server.Data;
using RandomCappuccino.Server.Services.SignManager.DTOs;
using RandomCappuccino.Server.Services.UserManager;
using RandomCappuccino.Server.Services.UserManager.DTOs;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.SignManager
{
    public class SignManager : ServiceBase, ISignManager
    {
        private readonly DataBaseContext context;
        private readonly IMapper mapper;
        private readonly TokenManager tokenManager;
        private readonly IUserManager userManager;
        
        public SignManager(DataBaseContext context, IMapper mapper, TokenManager tokenManager, IUserManager userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.tokenManager = tokenManager;
            this.userManager = userManager;
        }

        public async Task<ServiceContentResponse<SignResponseDTO>> SignIn(SignRequestDTO model)
        {
            var checkResponse = await userManager.CheckPassword(model.Email, model.Password);
            if(checkResponse.Succeed == false)
            {
                return Decline<SignResponseDTO>(checkResponse.Messages);
            }

            var userInfo = checkResponse.Content;

            var rolesResponse = await userManager.GetUserRoles(userInfo.Id);
            if(rolesResponse.Succeed == false)
            {
                return Decline<SignResponseDTO>(rolesResponse.Messages);
            }

            var userRoles = rolesResponse.Content;

            var token = tokenManager.GenerateToken(userInfo.Id, userRoles);

            return Accept(new SignResponseDTO { Token = token });
        }

        public async Task<ServiceContentResponse<SignResponseDTO>> SignUp(SignRequestDTO model)
        {
            var response = await userManager.CreateUser(
                new CreateUserDTO(
                    email: model.Email,
                    password: model.Password,
                    roles: "Customer"
                    ));
            if(response.Succeed == false)
            {
                Decline(response.Messages);
            }

            var userInfo = response.Content;

            var rolesResponse = await userManager.GetUserRoles(userInfo.Id);
            if (rolesResponse.Succeed == false)
            {
                return Decline<SignResponseDTO>(rolesResponse.Messages);
            }

            var userRoles = rolesResponse.Content;

            var token = tokenManager.GenerateToken(userInfo.Id, userRoles);            

            return Accept(new SignResponseDTO { Token = token });
        }
    }
}
