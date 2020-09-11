using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Containers.Schoolidolu;
using MiraiZuraBot.Containers.Schoolidolu.Idols;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Helpers.SchoolidoluHelper;
using MiraiZuraBot.Services.SchoolidoluService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.SchoolidoluCommands
{
    [CommandsGroup("SIF")]
    class GetIdolCommand : BaseCommandModule
    {
        private SchoolidoluService _schoolidoluService;
        private SchoolidoluHelper _schoolidoluHelper;

        public GetIdolCommand(SchoolidoluService schoolidoluService, SchoolidoluHelper schoolidoluHelper)
        {
            _schoolidoluService = schoolidoluService;
            _schoolidoluHelper = schoolidoluHelper;
    }

        [Command("idolka")]
        [Description("Pokazuje idolkę na bazie jej nazwy.\nnp:\n*idolka Watanabe You \n*idolka Sonoda Umi")]
        public async Task Idol(CommandContext ctx, [Description("Imie postaci."), RemainingText] string name)
        {
            await ctx.TriggerTypingAsync();

            var idolObject = _schoolidoluService.GetIdolByName(name);

            if (idolObject.StatusCode == HttpStatusCode.OK)
            {
                string description = _schoolidoluHelper.MakeIdolDescription(idolObject.Data);
                await PostEmbedHelper.PostEmbed(ctx, "Idolka", description, idolObject.Data.Chibi, idolObject.Data.Chibi_small, SchoolidoluHelper.GetSchoolidoluFotter());
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "Idolka", "Podana idolka nie istnieje.", null, null, SchoolidoluHelper.GetSchoolidoluFotter());
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
                string description = _schoolidoluHelper.MakeIdolDescription(idolsResponse.Data.Results[0]);
                await PostEmbedHelper.PostEmbed(ctx, "Losowa idolka", description, idolsResponse.Data.Results[0].Chibi, idolsResponse.Data.Results[0].Chibi_small, SchoolidoluHelper.GetSchoolidoluFotter());
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "Losowa idolka", "Wystąpił błąd.", null, null, SchoolidoluHelper.GetSchoolidoluFotter());
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
                await PostEmbedHelper.PostEmbed(ctx, "Wyszukiwanie idolek", "Wystąpił błąd podczas wyszukiwania idolek. Przed zapytaniem podaj numer strony.\nnp. `wyszukajIdolke 1 You`",
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
                    await PostEmbedHelper.PostEmbed(ctx, "Wyszukiwanie idolek", _schoolidoluHelper.MakeSearchIdolDescription(idolObject.Data, 10, intPage),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, "Wyszukiwanie idolek", "Brak wyników, spróbuj wyszukać inną frazę.",
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "Wyszukiwanie idolek",
                    "Wystąpił błąd podczas pobierania idolek. Mogło nastąpić odwołanie do nieistniejącej strony. Spróbuj wybrać stronę pierwszą.\n`wyszukajIdolke 1 " + keywords + "`",
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }
    }
}
