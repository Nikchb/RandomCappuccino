using AutoMapper;
using Grpc.Core;
using RandomCappuccino.Server.Services.SignManager;
using RandomCappuccino.Server.Services.SignManager.DTOs;
using RandomCappuccino.Shared;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.RPC
{
    public class SignServiceProvider : SignService.SignServiceBase
    {
        private readonly ISignManager signManager;
        private readonly IMapper mapper;

        public SignServiceProvider(ISignManager signManager, IMapper mapper)
        {
            this.signManager = signManager;
            this.mapper = mapper;
        }

        public override async Task<SignResponse> SignIn(SignRequest request, ServerCallContext context)
        {            
            var managerResponse = await signManager.SignIn(mapper.Map<SignRequestDTO>(request));

            var response = new SignResponse();

            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);
            response.Token = managerResponse.Content?.Token;

            return response;            
        }

        public override async Task<SignResponse> SignUp(SignRequest request, ServerCallContext context)
        {
            var managerResponse = await signManager.SignUp(mapper.Map<SignRequestDTO>(request));

            var response = new SignResponse();

            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);
            response.Token = managerResponse.Content?.Token;

            return response;
        }
    }
}
