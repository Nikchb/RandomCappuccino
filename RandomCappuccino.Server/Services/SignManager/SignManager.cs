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
    public class SignManager : ServiceBase<SignResponseDTO>, ISignManager
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
            var response = await userManager.CheckPassword(model.Email, HashPassword(model.Password));
            if(response.Succeed == false)
            {
                return Decline(response.Messages);
            }

            var userInfo = response.Content;

            var token = tokenManager.GenerateToken(userInfo.Id, userInfo.Roles);

            return Accept(new SignResponseDTO { Token = token });
        }

        public async Task<ServiceContentResponse<SignResponseDTO>> SignUp(SignRequestDTO model)
        {
            var response = await userManager.CreateUser(
                new CreateUserDTO(
                    email: model.Email,
                    password: HashPassword(model.Password),
                    roles: "Customer"
                    ));
            if(response.Succeed == false)
            {
                Decline(response.Messages);
            }

            var userInfo = response.Content;

            var token = tokenManager.GenerateToken(userInfo.Id, userInfo.Roles);

            return Accept(new SignResponseDTO { Token = token });
        }

        private string HashPassword(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            using (var hash = SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);                
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                {     
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                }
                return hashedInputStringBuilder.ToString();
            }
        }
    }
}
