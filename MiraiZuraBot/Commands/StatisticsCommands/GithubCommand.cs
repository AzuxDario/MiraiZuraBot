using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.StatisticsCommands
{
    [CommandsGroup("Statystyka")]
    class GithubCommand : BaseCommandModule
    {
        [Command("github")]
        [Description("Zwraca link do repozytorium")]
        public async Task Github(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await PostEmbedHelper.PostEmbed(ctx, "Github", "**[Link](https://github.com/AzuxDario/MiraiZuraBot)** do repozytorium bota.");
        }
    }
}
