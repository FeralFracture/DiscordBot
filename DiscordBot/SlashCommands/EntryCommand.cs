using DiscordBot.Biz.Interfaces;
using DiscordBot.Dal.Repositories;
using DiscordBot.Objects.Models;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discordbot.SlashCommands
{
    public class EntryCommand : ApplicationCommandModule
    {
        private readonly IArtEntryBiz _artEntryBiz;
        private readonly ILogger _logger;

        public EntryCommand(IArtEntryBiz artEntry, ILogger logger)
        {
            _artEntryBiz = artEntry;
            _logger = logger;
        }

        [SlashCommand("LogEntry", "Log an entry")]
        public async Task EnterData(InteractionContext ctx,
            [Option("Title", "Title for this entry")] string title,
            [Option("Description", "Short description for this entry")] string description,
            [Option("Hours", "Time spent")] long hours,
            [Option("Minutes", "Time spent")] long minutes)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("compiling..."));

            if (minutes > 59)
            {
                hours += minutes / 60;
                minutes = minutes % 60;
            }

            string outputText = $"{DiscordEmoji.FromName(ctx.Client, ":page_facing_up:", false)} Title: {title} \n" +
                $"{DiscordEmoji.FromName(ctx.Client, ":pencil:", false)} Description: {description} \n" +
                $"{DiscordEmoji.FromName(ctx.Client, ":stopwatch:", false)} Time: {hours}:{minutes:D2} \n" +
                $"{DiscordEmoji.FromName(ctx.Client, ":calendar_spiral:", false)} Date: {DateTime.Now.AddHours(2).ToShortDateString()} \n" +
                $"{DiscordEmoji.FromName(ctx.Client, ":person_walking:", false)} User: {ctx.User.Mention}";
            
            try
            {
                _artEntryBiz.Upsert(new ArtEntryModel
                {
                    Title = title,
                    Description = description,
                    Hours = (int)hours,
                    Minutes = (int)minutes,
                    UserId = ctx.User.Id
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var errorMessage = new DiscordWebhookBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Red)
                    .WithTitle("An Error has occured.")
                    .WithDescription(ex.GetType().Name));
                await ctx.EditResponseAsync(errorMessage);
                _logger.LogError(ex.Message);
                return;
            }

            var outputMessage = new DiscordWebhookBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Azure)
                .WithTitle("Entered Data")
                .WithDescription(outputText));

            await ctx.EditResponseAsync(outputMessage);
        }

        [SlashCommand("MonthStats", "Stats for this month")]
        public async Task GetMonthlyData(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("compiling..."));

            var dataList = _artEntryBiz.GetForMonth(ctx.User.Id, DateTime.Now);
            int minutes = 0;
            int hours = 0;

            foreach (var item in dataList)
            {
                minutes += item.Minutes;
                hours += item.Hours;
            }

            hours += minutes / 60;
            minutes %= 60;

            string outputText =
               $"{DiscordEmoji.FromName(ctx.Client, ":stopwatch:", false)} Total Time: {hours}:{minutes:D2} \n" +
              $"{DiscordEmoji.FromName(ctx.Client, ":1234:", false)} Total Entries: {dataList.Count} \n" +
              $"{DiscordEmoji.FromName(ctx.Client, ":calendar_spiral:", false)} Month: {DateTime.Now.Month} \n" +
               $"{DiscordEmoji.FromName(ctx.Client, ":person_walking:", false)} User: {ctx.User.Mention}";

            var outputMessage = new DiscordWebhookBuilder()
             .AddEmbed(new DiscordEmbedBuilder()
            .WithColor(DiscordColor.Azure)
            .WithTitle("Data for Month")
            .WithDescription(outputText));

            await ctx.EditResponseAsync(outputMessage);
        }
    }
}
