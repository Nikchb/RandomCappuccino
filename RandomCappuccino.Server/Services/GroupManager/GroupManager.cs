using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RandomCappuccino.Server.Data;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.GroupManager.DTOs;
using RandomCappuccino.Server.Services.IdentityManager;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.GroupManager
{
    public class GroupManager : ServiceBase, IGroupManager
    {
        private readonly IMapper mapper;
        private readonly DataBaseContext context;
        private readonly IIdentityManager identityManager;

        public GroupManager(IMapper mapper, DataBaseContext context, IIdentityManager identityManager)
        {
            this.mapper = mapper;
            this.context = context;
            this.identityManager = identityManager;
        }

        public async Task<ServiceContentResponse<GroupDTO>> CreateGroup(CreateGroupDTO model)
        {
            var validationResponce = ValidateModel(model);
            if (validationResponce.Succeed == false)
            {
                return Decline<GroupDTO>(validationResponce.Messages);
            }

            var user = await context.Users.FindAsync(identityManager.UserId);
            if (user == null)
            {
                return Decline<GroupDTO>("User is not found");
            }

            var group = new Group { Name = model.Name, UserId = identityManager.UserId };
            try
            {
                await context.Groups.AddAsync(group);
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline<GroupDTO>("Group creation is failed");
            }
            return Accept(mapper.Map<GroupDTO>(group));
        }

        public async Task<ServiceResponse> DeleteGroup(string groupId)
        {
            var group = await context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return Decline<GroupDTO>("Group is not found");
            }

            if (group.UserId != identityManager.UserId)
            {
                return Decline<GroupDTO>("Access is forbiden");
            }

            try
            {
                context.Groups.RemoveRange(group);
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline<GroupDTO>("Group delete is failed");
            }
            return Accept(mapper.Map<GroupDTO>(group));
        }

        public async Task<ServiceContentResponse<IEnumerable<GroupDTO>>> GetGroups()
        {
            IEnumerable<GroupDTO> groups;
            try
            {
                groups = await context.Groups.Where(v => v.UserId == identityManager.UserId)
                                             .Select(v => mapper.Map<GroupDTO>(v)).ToArrayAsync();
            }
            catch
            {
                return Decline<IEnumerable<GroupDTO>>("User groups request is failed");
            }
            return Accept(groups);
        }

        public async Task<ServiceContentResponse<GroupDTO>> GetGroup(string groupId)
        {
            var group = await context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return Decline<GroupDTO>("Group is not found");
            }

            if (group.UserId != identityManager.UserId)
            {
                return Decline<GroupDTO>("Access is forbiden");
            }

            return Accept(mapper.Map<GroupDTO>(group));
        }

        public async Task<ServiceContentResponse<GroupDTO>> UpdateGroup(GroupDTO model)
        {
            var validationResponce = ValidateModel(model);
            if (validationResponce.Succeed == false)
            {
                return Decline<GroupDTO>(validationResponce.Messages);
            }

            var group = await context.Groups.FindAsync(model.Id);
            if (group == null)
            {
                return Decline<GroupDTO>("Group is not found");
            }

            if(group.UserId != identityManager.UserId)
            {
                return Decline<GroupDTO>("Access is forbiden");
            }
            
            try
            {
                mapper.Map(model, group);                
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline<GroupDTO>("Group update is failed");
            }
            return Accept(mapper.Map<GroupDTO>(group));
        }
    }
}
