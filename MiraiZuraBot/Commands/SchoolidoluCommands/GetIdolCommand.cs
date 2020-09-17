using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Containers.Schoolidolu;
using MiraiZuraBot.Containers.Schoolidolu.Idols;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Helpers.SchoolidoluHelper;
using MiraiZuraBot.Services.SchoolidoluService;
using MiraiZuraBot.Translators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static MiraiZuraBot.Translators.Translator;

namespace MiraiZuraBot.Commands.SchoolidoluCommands
{
    [CommandsGroup("SIF")]
    class GetIdolCommand : BaseCommandModule
    {
        private SchoolidoluService _schoolidoluService;
        private SchoolidoluHelper _schoolidoluHelper;
        private Translator _translator;

        public GetIdolCommand(SchoolidoluService schoolidoluService, SchoolidoluHelper schoolidoluHelper, Translator translator)
        {
            _schoolidoluService = schoolidoluService;
            _schoolidoluHelper = schoolidoluHelper;
            _translator = translator;
    }

        [Command("idolka")]
        [Description("Pokazuje idolkę na bazie jej nazwy.\nnp:\n*idolka Watanabe You \n*idolka Sonoda Umi")]
        public async Task Idol(CommandContext ctx, [Description("Imie postaci."), RemainingText] string name)
        {
            await ctx.TriggerTypingAsync();

            var idolObject = _schoolidoluService.GetIdolByName(name);

            if (idolObject.StatusCode == HttpStatusCode.OK)
            {
                string description = _schoolidoluHelper.MakeIdolDescription(Language.Polish, idolObject.Data);
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(Language.Polish, "idol"), description, idolObject.Data.Chibi, idolObject.Data.Chibi_small, SchoolidoluHelper.GetSchoolidoluFotter(),
                    _schoolidoluHelper.GetColorForAttribute(idolObject.Data.Attribute));
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(Language.Polish, "idol"), _translator.GetString(Language.Polish, "idolDoesntExist") , null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("losowaIdolka")]
        [Description("Pokazuje losową idolkę.\nnp:\n*losowaIdolka")]
        public async Task RandomIdol(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "random" },
                { "page_size", "1" }
            };

            var idolsResponse = _schoolidoluService.GetIdol(options);

            if (idolsResponse.StatusCode == HttpStatusCode.OK)
            {
                string description = _schoolidoluHelper.MakeIdolDescription(Language.Polish, idolsResponse.Data.Results[0]);
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(Language.Polish, "idolRandom"), description, idolsResponse.Data.Results[0].Chibi, idolsResponse.Data.Results[0].Chibi_small, SchoolidoluHelper.GetSchoolidoluFotter(),
                    _schoolidoluHelper.GetColorForAttribute(idolsResponse.Data.Results[0].Attribute));
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(Language.Polish, "idolRandom"), _translator.GetString(Language.Polish, "idolError"), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("wyszukajIdolke")]
        [Description("Wyszukuje idolki.\nnp:\n`wyszukajIdolke 1 You`\nPolecam jako początkową stronę podać `1`." +
            "\nWyszukiwanie odbywa się po imionach, urodzinach, wymiarach, jedzeniu, hobby oraz danych seiyuu.")]
        public async Task SearchIdol(CommandContext ctx, [Description("Strona wyników.")] string page, [Description("Słowa kluczowe."), RemainingText] string keywords)
        {
            await ctx.TriggerTypingAsync();

            int intPage;

            if (!int.TryParse(page, out intPage))
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(Language.Polish, "idolSearch"), _translator.GetString(Language.Polish, "idolSearchNoPage"),
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
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(Language.Polish, "idolSearch"), _schoolidoluHelper.MakeSearchIdolDescription(Language.Polish, idolObject.Data, 10, intPage),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(Language.Polish, "idolSearch"), _translator.GetString(Language.Polish, "idolSearchNoResult"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(Language.Polish, "idolSearch"),
                    string.Format(_translator.GetString(Language.Polish, "idolSearchError"), keywords),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }
    }
}
