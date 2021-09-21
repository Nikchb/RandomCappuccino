using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RandomCappuccino.Server.Data;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.GroupManager.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.GroupManager
{
    public class GroupManager : ServiceBase, IGroupManager
    {
        private readonly IMapper mapper;
        private readonly DataBaseContext context;

        public GroupManager(IMapper mapper, DataBaseContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<ServiceContentResponse<GroupDTO>> CreateGroup(string userId, CreateGroupDTO model)
        {
            var group = new Group { Name = model.Name, UserId = userId };
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

        public async Task<ServiceResponse> DeleteGroup(string userId, string groupId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return Decline<GroupDTO>("User is not found");
            }

            var group = await context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return Decline<GroupDTO>("Group is not found");
            }

            if (group.UserId != userId)
            {
                return Decline<GroupDTO>("Access is forbiden");
            }

            try
            {
                var participants = await context.Participants.Where(v => v.GroupId == group.Id).ToArrayAsync();
                var tours = await context.Tours.Where(v => v.GroupId == group.Id).ToArrayAsync();
                foreach(var tour in tours)
                {
                    var pairs = await context.TourPairs.Where(v => v.TourId == tour.Id).ToArrayAsync();
                    context.RemoveRange(pairs);
                }
                context.Tours.RemoveRange(tours);
                context.Participants.RemoveRange(participants);
                context.Groups.RemoveRange(group);
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline<GroupDTO>("Group delete is failed");
            }
            return Accept(mapper.Map<GroupDTO>(group));
        }

        public async Task<ServiceContentResponse<IEnumerable<GroupDTO>>> GetGroups(string userId)
        {
            IEnumerable<GroupDTO> groups;
            try
            {
                groups = await context.Groups.Where(v => v.UserId == userId).Select(v => mapper.Map<GroupDTO>(v)).ToArrayAsync();
            }
            catch
            {
                return Decline<IEnumerable<GroupDTO>>("User groups request is failed");
            }
            return Accept(groups);
        }

        public async Task<ServiceContentResponse<GroupDTO>> UpdateGroup(string userId, GroupDTO model)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return Decline<GroupDTO>("User is not found");
            }

            var group = await context.Groups.FindAsync(model.Id);
            if (group == null)
            {
                return Decline<GroupDTO>("Group is not found");
            }

            if(group.UserId != userId)
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
