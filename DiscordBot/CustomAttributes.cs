using DiscordBot.Biz.Interfaces;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;

namespace DiscordBot
{
    public static class AttributesHandler
    {
        public static async Task CmdErroredHandler(SlashCommandsExtension _, SlashCommandErrorEventArgs e)
        {
            var failedChecks = ((SlashExecutionChecksFailedException)e.Exception).FailedChecks;
            foreach (var failedCheck in failedChecks)
            {
                if (failedCheck is RequirePermissionRoleAttribute)
                {
                    var attribute = (RequirePermissionRoleAttribute)failedCheck;
                    await e.Context.CreateResponseAsync($"{attribute._minPermissionLevel} permission level required.");
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequirePermissionRoleAttribute : SlashCheckBaseAttribute
    {
        public readonly int _minPermissionLevel;

        public RequirePermissionRoleAttribute(int minPermissionLevel)
        {
            _minPermissionLevel = minPermissionLevel;
        }

        public override Task<bool> ExecuteChecksAsync(InteractionContext ctx)
        {
            // Accessing the service provider to get the IRoleBiz instance
            var roleBiz = ctx.Services.GetRequiredService<IRoleBiz>();

            // Check if the user has the required role and permission level
            var guild = ctx.Client.GetGuildAsync(ctx.Guild.Id).Result;
            if (guild == null)
                return Task.FromResult(false);

            var member = guild.GetMemberAsync(ctx.User.Id).Result;
            if (member == null)
                return Task.FromResult(false);

            // Check if any of the user's roles meet the permission level requirement
            return Task.FromResult(guild.OwnerId == ctx.User.Id ||
                member.Roles.Any(r => roleBiz.GetByUlongId(r.Id)?.permission_level >= _minPermissionLevel));
        }
    }
}
