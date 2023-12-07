using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace discordbot.SlashCommands
{
    public class EntryCommand : ApplicationCommandModule
    {
        [SlashCommand("Enter", "Entering data")]
        public async Task EnterData(InteractionContext ctx,
            [Option("Title", "Title for this entry")] string title,
            [Option("Description", "Short description for this entry")] string description,
            [Option("Hours", "Time spent")] long hours,
            [Option("Minutes", "Time spent")] long minutes
            )
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("compiling..."));

            if(minutes > 59)
            {
                hours += minutes / 60;
                minutes = minutes % 60;
            }

            string outputText = $"{DiscordEmoji.FromName(ctx.Client, ":page_facing_up:", false)} Title: {title} \n" +
                $"{DiscordEmoji.FromName(ctx.Client, ":pencil:", false)} Description: {description} \n" +
                $"{DiscordEmoji.FromName(ctx.Client, ":stopwatch:", false)} Time: {hours}:{minutes:D2} \n" +
                $"{DiscordEmoji.FromName(ctx.Client, ":calendar_spiral:", false)} Date: {DateTime.Now.AddHours(2).ToShortDateString()} \n" +
                $"{DiscordEmoji.FromName(ctx.Client, ":person_walking:", false)} User: {ctx.User.Mention}";

            var outputMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Azure)
                .WithTitle("Entered Data")
                .WithDescription(outputText)
                );

            await ctx.Channel.SendMessageAsync(outputMessage);
        }
    }
}
