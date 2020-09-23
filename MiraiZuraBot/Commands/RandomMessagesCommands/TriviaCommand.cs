using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Services.LanguageService;
using MiraiZuraBot.Services.TriviaService;
using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.RandomMessagesCommands
{
    [GroupLang("Tekst", "Text")]
    class TriviaCommand : BaseCommandModule
    {
        private TriviaService _triviaService;
        private LanguageService _languageService;
        private Translator _translator;

        public TriviaCommand(TriviaService triviaService, LanguageService languageService, Translator translator)
        {
            _triviaService = triviaService;
            _languageService = languageService;
            _translator = translator;
        }

        [Command("tematyCiekawostek")]
        [Aliases("triviaTopics")]
        [CommandLang("tematyCiekawostek", "triviaTopics")]
        [DescriptionLang("Pokazuje tematy ciekawostek.", "Shows trivia topics")]
        [Description("Pokazuje tematy ciekawostek.")]
        public async Task TriviaTopics(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            List<string> topics = _triviaService.GetTopics();
            await PostLongMessageHelper.PostLongMessage(ctx, topics, _translator.GetString(lang, "triviaTopics"), ", ");
        }

        [Command("ciekawostka")]
        [Aliases("trivia")]
        [CommandLang("ciekawostka", "trivia")]
        [DescriptionLang("Pokazuje losową ciekawostkę.", "Shows random trivia.")]
        public async Task GetTrivia(CommandContext ctx, [DescriptionLang("Temat z listy tematów", "Topic from topic list"), ParameterLang("Temat", "Topic"), RemainingText] string topic = null)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            if (topic != null)
            {
                List<string> topics = _triviaService.GetTopics();
                if(!topics.Contains(topic))
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "trivia"), _translator.GetString(lang, "triviaWrongTopic"));
                    return;
                }
            }
            var trivia = _triviaService.GetTrivia(topic);
            await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "trivia"), string.Format("{0}\n{1}: {2}", trivia.Content, _translator.GetString(lang, "triviaSource"), trivia.Source), null, null, null);
        }
    }
}
