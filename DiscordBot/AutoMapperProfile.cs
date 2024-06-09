using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using discordbot.dal;
using discordbot.dal.Entities;
using DiscordBot.Objects.Models;

namespace DiscordBot.Objects
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ArtEntry, ArtEntryModel>().ReverseMap();
        }
    }
}
