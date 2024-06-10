using AutoMapper;
using DiscordBot.Biz.Interfaces;
using DiscordBot.Dal.Entities;
using DiscordBot.Objects.Models;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DiscordBot.SlashCommands
{
    public class RoleCommands : ApplicationCommandModule
    {
        private readonly IRoleBiz _roleBiz;
        private readonly ILogger<RoleCommands> _logger;
        private readonly IMapper _mapper;
        public RoleCommands(IRoleBiz roleBiz, ILogger<RoleCommands> logger, IMapper mapper)
        {
            _roleBiz = roleBiz;
            _logger = logger;
            _mapper = mapper;
        }

        [SlashCommand("SetRolePermissionLevel", "Set the permission level for a particular role"), RequirePermissionRole(1)]
        public async Task SetRolePermLevel(InteractionContext ctx,
            [Option("Role", "Target Role")] DiscordRole role,
            [Option("Permission_Level", "whatever level of permission this role should have")] long perm_level)
        {
            await ctx.DeferAsync();

            var embed = new DiscordEmbedBuilder
            {
                Color = role.Color,
                Title = "Role permission level",
                Description = $"The role {role.Mention} 's permission level is set to {perm_level}"
            };
            try
            {
                var model = _mapper.Map<RoleModel>(role);
                model.ParentDiscordServerId = ctx.Guild.Id;
                model.permission_level = (int)perm_level;
                model.UpdatedAt = DateTime.UtcNow;

                _roleBiz.Upsert(model, x => x.DiscordRoleId == role.Id);
                _logger.LogInformation($"[Guild Name/ID: {ctx.Guild.Name}/{ctx.Guild.Id}] Changed/Added Permission Level of role \"{role.Name}\" (DiscordID: {role.Id}) to: {(int)perm_level}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Guild Name/ID: {ctx.Guild.Name}/{ctx.Guild.Id}] Failed to Change/Add Permission Level | Exception of type {ex.GetType()} occured." +
                    $"\n" +
                    $"Message: {ex.Message}");
                var errorEmbed = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.DarkRed,
                    Title = "Error Occured",
                    Description = $"An error occured. Please Try again."
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(errorEmbed));
                return;
            }
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
        }
        [SlashCommand("UnsetRolePermissions", "Removes Role from list of roles with permissions")]
        public async Task RemoveRole(InteractionContext ctx, [Option("Role", "Target Role")] DiscordRole role)
        {
            await ctx.DeferAsync();
            var embed = new DiscordEmbedBuilder
            {
                Color = role.Color,
                Title = "Remove role from perm list",
                Description = $"{role.Mention} has been removed from/is not in the permissions list."
            };
            try
            {
                _roleBiz.Delete(_roleBiz.GetByUlongId(role.Id));
            }
            catch (Exception e)
            {
                _logger.LogError($"[Guild Name/ID: {ctx.Guild.Name}/{ctx.Guild.Id}] Failed to remove role | Exception of type {e.GetType()} occured." +
                 $"\n" +
                 $"Message: {e.Message}");
                var errorEmbed = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.DarkRed,
                    Title = "Error Occured",
                    Description = $"An error occured. Please Try again."
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(errorEmbed));
                return;
            }
            _logger.LogInformation($"[Guild Name/ID: {ctx.Guild.Name}/{ctx.Guild.Id}] Removed role \"{role.Name}\" (DiscordID: {role.Id}) from perm list.");

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
        }
    }
}
