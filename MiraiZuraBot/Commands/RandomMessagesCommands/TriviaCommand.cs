using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
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

        [Command("ciekawostka")]
        [Description("Pokazuje ciekawostkę")]
        public async Task GetTrivia(CommandContext ctx, [Description("Temat z listy tematów.")] [RemainingText] string topic = null)
        {
            if(topic != null)
            {
                List<string> topics = _triviaService.GetTopics();
                if(!topics.Contains(topic))
                {
                    await PostEmbedHelper.PostEmbed(ctx, "Ciekawostka", "Brak podanego tematu. Sprawdź listę tematów komendą `tematyCiekawostek`");
                    return;
                }
            }
            var trivia = _triviaService.GetTrivia(topic);                
            await PostEmbedHelper.PostEmbed(ctx, "Ciekawostka", trivia.Content + "\nŹródło: " + trivia.Source, null, null, null);
        }
    }
}
