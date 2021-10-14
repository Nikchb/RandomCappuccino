using RandomCappuccino.Server.Services.SignManager.DTOs;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.SignManager
{
    public interface ISignManager
    {
        Task<ServiceContentResponse<SignResponseDTO>> SignIn(SignRequestDTO model);
        Task<ServiceContentResponse<SignResponseDTO>> SignUp(SignRequestDTO model);        
    }
}
