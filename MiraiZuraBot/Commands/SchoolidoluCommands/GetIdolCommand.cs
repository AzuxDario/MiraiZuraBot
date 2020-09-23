using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Helpers.SchoolidoluHelper;
using MiraiZuraBot.Services.LanguageService;
using MiraiZuraBot.Services.SchoolidoluService;
using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static MiraiZuraBot.Translators.Translator;

namespace MiraiZuraBot.Commands.SchoolidoluCommands
{
    [GroupLang("SIF", "SIF")]
    class GetIdolCommand : BaseCommandModule
    {
        private SchoolidoluService _schoolidoluService;
        private SchoolidoluHelper _schoolidoluHelper;
        private LanguageService _languageService;
        private Translator _translator;

        public GetIdolCommand(SchoolidoluService schoolidoluService, SchoolidoluHelper schoolidoluHelper, LanguageService languageService, Translator translator)
        {
            _schoolidoluService = schoolidoluService;
            _schoolidoluHelper = schoolidoluHelper;
            _languageService = languageService;
            _translator = translator;
    }

        [Command("idolka")]
        [Aliases("idol")]
        [CommandLang("idolka", "idol")]
        [DescriptionLang("Pokazuje idolkę na bazie jej nazwy.\nnp:\n`idolka Watanabe You` \n`idolka Sonoda Umi`", "Shows the idol based on its name.\ne.g.\n`idol Watanabe You`\n`idol Sonoda Umi`")]
        public async Task Idol(CommandContext ctx, [DescriptionLang("Imie idolki", "Idol name"), ParameterLang("Imie", "Name"), RemainingText] string name)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            var idolObject = _schoolidoluService.GetIdolByName(name);

            if (idolObject.StatusCode == HttpStatusCode.OK)
            {
                string description = _schoolidoluHelper.MakeIdolDescription(lang, idolObject.Data);
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "idol"), description, idolObject.Data.Chibi, idolObject.Data.Chibi_small, SchoolidoluHelper.GetSchoolidoluFotter(),
                    _schoolidoluHelper.GetColorForAttribute(idolObject.Data.Attribute));
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "idol"), _translator.GetString(lang, "idolDoesntExist") , null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("losowaIdolka")]
        [Aliases("randomIdol")]
        [CommandLang("losowaIdolka", "randomIdol")]
        [DescriptionLang("Pokazuje losową idolkę.\nnp:\n`losowaIdolka`", "Shows a random idol.\ne.g.\n`randomIdol`")]
        public async Task RandomIdol(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "random" },
                { "page_size", "1" }
            };

            var idolsResponse = _schoolidoluService.GetIdol(options);

            if (idolsResponse.StatusCode == HttpStatusCode.OK)
            {
                string description = _schoolidoluHelper.MakeIdolDescription(lang, idolsResponse.Data.Results[0]);
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "idolRandom"), description, idolsResponse.Data.Results[0].Chibi, idolsResponse.Data.Results[0].Chibi_small, SchoolidoluHelper.GetSchoolidoluFotter(),
                    _schoolidoluHelper.GetColorForAttribute(idolsResponse.Data.Results[0].Attribute));
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "idolRandom"), _translator.GetString(lang, "idolError"), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("wyszukajIdolke")]
        [Aliases("searchIdol")]
        [CommandLang("wyszukajIdolke", "searchIdol")]
        [DescriptionLang("Wyszukuje idolki.\nnp:\n`wyszukajIdolke 1 You`\nPolecam jako początkową stronę podać `1`." +
            "\nWyszukiwanie odbywa się po imionach, urodzinach, wymiarach, jedzeniu, hobby oraz danych seiyuu.",
            "Search for idols.\ne.g.\n`searchIdol 1 You`\nI recommend to choose `1` as the initial page." +
            "\nSearch by names, birthdays, dimensions, food, hobbies and seiyuu data.")]
        public async Task SearchIdol(CommandContext ctx, [DescriptionLang("Strona wyników", "Result page"), ParameterLang("Strona", "Page")] string page,
            [DescriptionLang("Fraza do wyszukania", "The phrase to search for"), ParameterLang("Słowa kluczowe", "Keywords"), RemainingText] string keywords)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            int intPage;

            if (!int.TryParse(page, out intPage))
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "idolSearch"), _translator.GetString(lang, "idolSearchNoPage"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                return;
            }

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "search", keywords },
                { "page", intPage.ToString() }
            };

            var idolObject = _schoolidoluService.GetIdol(options);

            if (idolObject.StatusCode == HttpStatusCode.OK)
            {

                if (idolObject.Data.Count != 0)
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "idolSearch"), _schoolidoluHelper.MakeSearchIdolDescription(lang, idolObject.Data, 10, intPage),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "idolSearch"), _translator.GetString(lang, "idolSearchNoResult"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "idolSearch"),
                    string.Format(_translator.GetString(lang, "idolSearchError"), keywords),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }
    }
}
