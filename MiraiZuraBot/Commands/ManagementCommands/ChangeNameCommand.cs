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
    [GroupLang("Zarządzanie", "Management")]
    class ChangeNameCommand : BaseCommandModule
    {
        [Command("zmienNazwe")]
        [Aliases("changeName")]
        [CommandLang("zmienNazwe", "changeName")]
        [DescriptionLang("Zmień imie bota.", "Changes bot name.")]
        [RequireOwner]
        public async Task ChangeName(CommandContext ctx, [DescriptionLang("Nowe imie", "New name"), ParameterLang("Imie", "Name"), RemainingText] string name)
        {
            await Bot.DiscordClient.UpdateCurrentUserAsync(name);
        }

        [Command("zmienPseudonim")]
        [Aliases("changeNick")]
        [CommandLang("zmienPseudonim", "changeNick")]
        [DescriptionLang("Zmień pseudonim bota.", "Changes bot nickname.")]
        [RequireOwner]
        public async Task ChangeNick(CommandContext ctx, [DescriptionLang("Nowy pseudonim", "New nickname"), ParameterLang("Pseudomin", "Nick"), RemainingText] string name)
        {
            await ctx.Guild.CurrentMember.ModifyAsync(p => p.Nickname = name);
        }

        [Command("usunPseudonim")]
        [Aliases("removeNick")]
        [CommandLang("usunPseudonim", "removeNick")]
        [DescriptionLang("Usuń pseudonim bota.", "Remove bot nickname.")]
        [RequireOwner]
        public async Task RemoveNick(CommandContext ctx)
        {
            await ctx.Guild.CurrentMember.ModifyAsync(p => p.Nickname = null);
        }
    }
}
