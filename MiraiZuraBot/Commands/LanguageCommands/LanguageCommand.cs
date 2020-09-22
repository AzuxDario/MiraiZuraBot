using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Services.LanguageService;
using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.LanguageCommands
{
    [GroupLang("Język", "Language")]
    class LanguageCommand : BaseCommandModule
    {
        private LanguageService _languageService;
        private Translator _translator;

        public LanguageCommand(LanguageService languageService, Translator translator)
        {
            _languageService = languageService;
            _translator = translator;
        }

        [Command("dostepneJezyki")]
        [Description("Pokazuje dostępne języki.")]
        public async Task ShowLanguages(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            List<string> languages = _translator.GetAvailableLanguages();

            await PostLongMessageHelper.PostLongMessage(ctx, languages, _translator.GetString(lang, "languagesAvailable"), ", ");
        }

        [Command("zmienJezyk")]
        [Description("Zmień język.")]
        [RequireUserPermissions(DSharpPlus.Permissions.ManageGuild)]
        public async Task ChangeLanguage(CommandContext ctx, [Description("Język."), RemainingText] string language)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            List<string> languages = _translator.GetAvailableLanguages();

            if(!languages.Contains(language))
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "languagesAvailable"), _translator.GetString(lang, "languagesNotOnList"));
            }
            else
            {
                Translator.Language newLang = _translator.GetEnumForString(language);
                _languageService.ChangeLanguage(ctx.Guild.Id, newLang);
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(newLang, "languagesAvailable"), _translator.GetString(newLang, "languagesChanged"));
            }
        }
    }
}
