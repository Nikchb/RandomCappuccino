using RandomCappuccino.Shared;
using Grpc.Core;
using RandomCappuccino.Server.Services.UserManager;
using RandomCappuccino.Server.Services.UserManager.DTOs;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RandomCappuccino.Server.RPC
{
    
    public class UserServiceProvider : UserService.UserServiceBase
    {
        private readonly IUserManager userManager;
        private readonly IMapper mapper;

        public UserServiceProvider(IUserManager userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async override Task<UserInfoResponse> GetUserInfo(UserInfoRequest request, ServerCallContext context)
        {
            var managerResponse = await userManager.GetUserInfo();

            var response = new UserInfoResponse();
            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);
            if (managerResponse.Content != null) response.UserInfo = mapper.Map<UserInfo>(managerResponse.Content);

            return response;
        }

        public async override Task<UserInfoResponse> UpdateUserInfo(UpdateUserInfoRequest request, ServerCallContext context)
        {
            var managerResponse = await userManager.UpdateUserInfo(mapper.Map<UpdateUserInfoDTO>(request));

            var response = new UserInfoResponse();
            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);
            if (managerResponse.Content != null) response.UserInfo = mapper.Map<UserInfo>(managerResponse.Content);

            return response;
        }

        public async override Task<UserServiceResponse> UpdateUserPassword(UpdateUserPasswordRequest request, ServerCallContext context)
        {
            var managerResponse = await userManager.UpdateUserPassword(mapper.Map<UpdateUserPasswordDTO>(request));

            var response = new UserServiceResponse();
            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);            

            return response;
        }
    }
}
