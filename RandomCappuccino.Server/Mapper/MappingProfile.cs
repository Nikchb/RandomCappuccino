using AutoMapper;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.UserManager.DTOs;
using RandomCappuccino.Server.Services.SignManager.DTOs;
using RandomCappuccino.Server.Services.GroupManager.DTOs;

namespace RandomCappuccino.Server.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<UpdateUserInfoDTO, User>();

            CreateMap<Group, GroupDTO>();
            CreateMap<GroupDTO, Group>()
                .ForMember(dest => dest.Id, act => act.Ignore());                
        }
    }
}
