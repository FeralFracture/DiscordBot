using AutoMapper;
using DiscordBot.Dal.Entities;
using DiscordBot.Objects.Models;

namespace DiscordBot.Objects
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ArtEntry, ArtEntryModel>().ReverseMap();
            CreateMap<Server, ServerModel>().ReverseMap();
            CreateMap<Role, RoleModel>().ReverseMap();
        }
    }
}
