using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.ManagementCommands
{
    [GroupLang("Zarządzanie", "Management")]
    class DescriptionCommand : BaseCommandModule
    {
        private Timer refreshDescriptionTimer;
        private int refreshDescriptionInterval;

        private string game;

        public DescriptionCommand()
        {
            refreshDescriptionInterval = 1000 * 60;    // every 1 minute
            refreshDescriptionTimer = new Timer(RefreshDescriptionCallback, null, refreshDescriptionInterval, Timeout.Infinite);

            game = string.Empty;
        }

        [Command("opis")]
        [Aliases("description")]
        [CommandLang("opis", "description")]
        [Description("Zmień opis bota.")]
        [RequireOwner]
        public async Task Description(CommandContext ctx, [Description("New description.")] [RemainingText] string description)
        {
            if (ctx.Member.Id == Bot.configJson.Developer)
            {
                game = description;

                try
                {
                    await ctx.Client.UpdateStatusAsync(new DiscordActivity(description));
                }
                catch (Exception ie)
                {
                    Console.WriteLine("Error: Can't set status.");
                    Console.WriteLine("Exception: " + ie.Message);
                    Console.WriteLine("Inner Exception: " + ie?.InnerException?.Message);
                    Console.WriteLine("Stack trace: " + ie.StackTrace);
                }
            }
        }

        private void RefreshDescriptionCallback(object state)
        {
            if (game != string.Empty)
            {
                try
                {
                    Bot.DiscordClient.UpdateStatusAsync(new DiscordActivity(game));
                }
                catch (Exception ie)
                {
                    Console.WriteLine("Error: Can't update status.");
                    Console.WriteLine("Exception: " + ie.Message);
                    Console.WriteLine("Inner Exception: " + ie?.InnerException?.Message);
                    Console.WriteLine("Stack trace: " + ie.StackTrace);
                }
            }

            refreshDescriptionTimer.Change(refreshDescriptionInterval, Timeout.Infinite);
        }
    }
}
