using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using RandomCappuccino.Server.Services.GroupManager;
using RandomCappuccino.Server.Services.GroupManager.DTOs;
using RandomCappuccino.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.RPC
{
    [Authorize]
    public class GroupServiceProvider : GroupService.GroupServiceBase
    {
        private readonly IGroupManager groupManager;
        private readonly IMapper mapper;

        public GroupServiceProvider(IGroupManager groupManager, IMapper mapper)
        {
            this.groupManager = groupManager;
            this.mapper = mapper;
        }

        public async override Task<GroupsResponse> GetGroups(GetGroupsRequest request, ServerCallContext context)
        {
            var managerResponse = await groupManager.GetGroups();

            var response = new GroupsResponse();

            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);
            if(managerResponse.Content != null)
            {
                response.Groups.AddRange(managerResponse.Content.Select(v => mapper.Map<GroupInfo>(v)));
            }

            return response;
        }

        public async override Task<GroupResponse> CreateGroup(CreateGroupRequest request, ServerCallContext context)
        {
            var managerResponse = await groupManager.CreateGroup(mapper.Map<CreateGroupDTO>(request));

            var response = new GroupResponse();

            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);
            if (managerResponse.Content != null)
            {
                response.Group = mapper.Map<GroupInfo>(managerResponse.Content);
            }
            
            return response;
        }

        public async override Task<GroupResponse> GetGroup(GetGroupRequest request, ServerCallContext context)
        {
            var managerResponse = await groupManager.GetGroup(request.Id);

            var response = new GroupResponse();

            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);
            if (managerResponse.Content != null)
            {
                response.Group = mapper.Map<GroupInfo>(managerResponse.Content);
            }

            return response;
        }

        public async override Task<GroupResponse> UpdateGroup(UpdateGroupRequest request, ServerCallContext context)
        {
            var managerResponse = await groupManager.UpdateGroup(mapper.Map<GroupDTO>(request));

            var response = new GroupResponse();

            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);
            if (managerResponse.Content != null)
            {
                response.Group = mapper.Map<GroupInfo>(managerResponse.Content);
            }

            return response;
        }

        public async override Task<GroupServiceResponse> DeleteGroup(DeleteGroupRequest request, ServerCallContext context)
        {
            var managerResponse = await groupManager.DeleteGroup(request.Id);

            var response = new GroupServiceResponse();

            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);            

            return response;
        }
    }
}
