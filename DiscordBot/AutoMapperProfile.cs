using AutoMapper;
using DiscordBot.Dal.Entities;
using DiscordBot.Objects.Models;
using DSharpPlus.Entities;

namespace DiscordBot.Objects
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ArtEntry, ArtEntryModel>().ReverseMap()
                .ForMember(dest => dest.ArtEntryId, opt =>
                {
                    opt.MapFrom(src => src.ArtEntryId);
                    opt.Condition(src => src.ArtEntryId != null);
                });
            CreateMap<Server, ServerModel>().ReverseMap()
                .ForMember(dest => dest.ServerId,
                opt =>
                {
                    opt.MapFrom(src => src.ServerId);
                    opt.Condition(src => src.ServerId != null);
                });
            CreateMap<Role, RoleModel>().ReverseMap()
            .ForMember(dest => dest.RoleId, opt =>
            {
                opt.MapFrom(src => src.RoleId);
                opt.Condition(src => src.RoleId != null);
            });
            CreateMap<CustomRole, CustomRoleModel>()
                .ForMember(dest => dest.RoleColor,
                opt => opt.MapFrom(src => new DiscordColor(src.RoleColorR, src.RoleColorG, src.RoleColorB)))
                .ReverseMap()
                .ForMember(dest => dest.RoleColorR, opt => opt.MapFrom(src => src.RoleColor.R))
                .ForMember(dest => dest.RoleColorG, opt => opt.MapFrom(src => src.RoleColor.G))
                .ForMember(dest => dest.RoleColorB, opt => opt.MapFrom(src => src.RoleColor.B))
                .ForMember(dest => dest.CustomRoleId, opt =>
                {
                    opt.MapFrom(src => src.CustomRoleId);
                    opt.Condition(src => src.CustomRoleId != null);
                });

            CreateMap<DiscordRole, RoleModel>()
                .ForMember(dest => dest.DiscordRoleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
