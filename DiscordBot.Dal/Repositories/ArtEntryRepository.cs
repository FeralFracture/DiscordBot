using AutoMapper;
using discordbot.dal;
using discordbot.dal.Entities;
using DiscordBot.Objects.Interfaces.IRepositories;
using DiscordBot.Objects.Models;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DiscordBot.Dal.Repositories
{
    public class ArtEntryRepository : GenericRepository<ArtEntry, ArtEntryModel>
    {
        public ArtEntryRepository(BotDbContext context, IMapper mapper, ILogger<ArtEntryRepository> logger) : base(context, mapper, logger) { }

        public override void Upsert(ArtEntryModel model, Expression<Func<ArtEntry, bool>>? matchPredicate = null)
        {
            model.date = DateTime.UtcNow;
            base.Upsert(model, matchPredicate);
        }
    }
}
