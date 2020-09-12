using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Services.TriviaService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.RandomMessagesCommands
{
    [CommandsGroup("Tekst")]
    class TriviaCommand : BaseCommandModule
    {
        private TriviaService _triviaService;

        public TriviaCommand(TriviaService triviaService)
        {
            _triviaService = triviaService;
        }

        [Command("tematyCiekawostek")]
        [Description("Pokazuje tematy ciekawostek.")]
        public async Task TriviaTopics(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            List<string> topics = _triviaService.GetTopics();
            await PostLongMessageHelper.PostLongMessage(ctx, topics, "Tematy ciekawostek");
        }
    }
}
