namespace DiscordBot.Biz.Interfaces
{
    public interface IGenericDiscordBiz<TEntity, TModel> : IGenericBiz<TEntity, TModel>
    {
        TModel? GetByUlongId(ulong discordId);
    }
}
