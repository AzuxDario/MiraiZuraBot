using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Net.Models;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.ManagementCommands
{
    [CommandsGroup("Zarządzanie")]
    class ChangeNameCommand : BaseCommandModule
    {
        [Command("zmienNazwe")]
        [Description("Zmień imię bota.")]
        [RequireOwner]
        public async Task ChangeName(CommandContext ctx, [Description("Nowe imię.")] [RemainingText] string name)
        {
            await Bot.DiscordClient.UpdateCurrentUserAsync(name);
        }

        [Command("zmienPseudonim")]
        [Description("Zmień pseudonim bota.")]
        [RequireOwner]
        public async Task ChangeNick(CommandContext ctx, [Description("Nowy pseudonim.")] [RemainingText] string name)
        {
            await ctx.Guild.CurrentMember.ModifyAsync(p => p.Nickname = name);
        }

        [Command("usunPseudonim")]
        [Description("Usuń pseudonim bota.")]
        [RequireOwner]
        public async Task RemoveNick(CommandContext ctx)
        {
            await ctx.Guild.CurrentMember.ModifyAsync(p => p.Nickname = null);
        }
    }
}
