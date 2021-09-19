using AutoMapper;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.UserManager.DTOs;
using RandomCappuccino.Server.Services.SignManager.DTOs;

namespace RandomCappuccino.Server.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<UpdateUserInfoDTO, User>();
        }
    }
}
