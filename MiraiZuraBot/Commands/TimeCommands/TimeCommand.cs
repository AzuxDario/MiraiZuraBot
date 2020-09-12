using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Helpers.TimeHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.TimeCommands
{
    [CommandsGroup("Czas")]
    class TimeCommand : BaseCommandModule
    {
        private TimeHelper _timeHelper;

        public TimeCommand(TimeHelper timeHelper)
        {
            _timeHelper = timeHelper;
        }

        [Command("jst")]
        [Description("Pokazuje japoński czas.")]
        public async Task Jst(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await PostEmbedHelper.PostEmbed(ctx, "Czas JST", _timeHelper.GetCurrentJapanTime().ToString("HH:mm:ss d.MM.yyyy JST"), null, null, null);
        }
    }
}
