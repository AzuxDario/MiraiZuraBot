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
        [Aliases("availableLanguages")]
        [CommandLang("dostepneJezyki", "availableLanguages")]
        [DescriptionLang("Pokazuje dostępne języki.", "Shows available languages.")]
        public async Task ShowLanguages(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            List<string> languages = _translator.GetAvailableLanguages();

            await PostLongMessageHelper.PostLongMessage(ctx, languages, _translator.GetString(lang, "languagesAvailable"), ", ");
        }

        [Command("zmienJezyk")]
        [Aliases("changeLanguage")]
        [CommandLang("zmienJezyk", "changeLanguage")]
        [DescriptionLang("Zmienia język bota na serwerze.", "Changes bot language on server.")]
        [RequireUserPermissions(DSharpPlus.Permissions.ManageGuild)]
        public async Task ChangeLanguage(CommandContext ctx, [DescriptionLang("Język z listy dostępnych", "Language from the list of available"), ParameterLang("Język", "Language"), RemainingText] string language)
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
