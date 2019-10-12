using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.ManagementCommands
{
    [CommandsGroup("Management")]
    class ChangeNameCommand : BaseCommandModule
    {
        [Command("changeName")]
        [Description("Change bot descrition.")]
        public async Task ChangeName(CommandContext ctx, [Description("New name.")] [RemainingText] string name)
        {
            if (ctx.Member.Id == Bot.configJson.Developer)
            {
                await Bot.DiscordClient.UpdateCurrentUserAsync(name);
            }
        }
    }
}
