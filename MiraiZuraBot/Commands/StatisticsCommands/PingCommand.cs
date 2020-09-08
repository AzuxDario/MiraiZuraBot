﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.StatisticsCommands
{
    [CommandsGroup("Statystyka")]
    class PingCommand : BaseCommandModule
    {
        [Command("ping")]
        [Description("Sprawdź ping.")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync($"{ctx.Client.Ping} ms");
        }
    }
}