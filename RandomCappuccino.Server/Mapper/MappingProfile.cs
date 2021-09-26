using AutoMapper;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.UserManager.DTOs;
using RandomCappuccino.Server.Services.SignManager.DTOs;
using RandomCappuccino.Server.Services.GroupManager.DTOs;
using RandomCappuccino.Server.Services.ParticipantManager.DTOs;
using RandomCappuccino.Server.Services.TourManager.DTOs;
using RandomCappuccino.Server.Services.TourManager;

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

            CreateMap<CreateParticipantDTO, Participant>();
            CreateMap<Participant, ParticipantDTO>();            
            CreateMap<ParticipantDTO, Participant>()
                .ForMember(dest => dest.Id, act => act.Ignore());

            CreateMap<CreateTourDTO, Tour>();
            CreateMap<Tour, TourDTO>();
            CreateMap<TourPair, TourPairDTO>();
            CreateMap<TourPair, Pair>()
                .ForMember(dest => dest.Participant1, act => act.MapFrom(v => v.Participant1Id))
                .ForMember(dest => dest.Participant2, act => act.MapFrom(v => v.Participant2Id));
        }
    }
}
