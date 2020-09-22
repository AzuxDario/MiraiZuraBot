using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Helpers.TimeHelper;
using MiraiZuraBot.Services.LanguageService;
using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.TimeCommands
{
    [GroupLang("Czas", "Time")]
    class TimeCommand : BaseCommandModule
    {
        private TimeHelper _timeHelper;
        private LanguageService _languageService;
        private Translator _translator;

        public TimeCommand(TimeHelper timeHelper, LanguageService languageService, Translator translator)
        {
            _timeHelper = timeHelper;
            _languageService = languageService;
            _translator = translator;
        }

        [Command("jst")]
        [CommandLang("jst", "jst")]
        [Description("Pokazuje japoński czas.")]
        public async Task Jst(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "timeJst"), _timeHelper.GetCurrentJapanTime().ToString("HH:mm:ss d.MM.yyyy JST"), null, null, null);
        }
    }
}
