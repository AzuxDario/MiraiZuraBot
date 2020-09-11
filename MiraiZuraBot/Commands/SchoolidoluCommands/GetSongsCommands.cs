using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Containers.Schoolidolu.Event;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Helpers.SchoolidoluHelper;
using MiraiZuraBot.Services.SchoolidoluService;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.SchoolidoluCommands
{
    [CommandsGroup("SIF")]
    class GetSongsCommands : BaseCommandModule
    {
        private SchoolidoluService _schoolidoluService;
        private SchoolidoluHelper _schoolidoluHelper;

        public GetSongsCommands(SchoolidoluService schoolidoluService, SchoolidoluHelper schoolidoluHelper)
        {
            _schoolidoluService = schoolidoluService;
            _schoolidoluHelper = schoolidoluHelper;
        }

        [Command("losowaPiosenka")]
        [Description("Pokazuje losową piosenkę.\nnp:\n*losowaPiosenka")]
        public async Task RandomSong(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "random" },
                { "page_size", "1" }
            };

            var songsResponse = _schoolidoluService.GetSong(options);

            if (songsResponse.StatusCode == HttpStatusCode.OK)
            {
                await PostEmbedHelper.PostEmbed(ctx, "Losowa piosenka", _schoolidoluHelper.MakeSongDescription(songsResponse.Data.Results[0]),
                    songsResponse.Data.Results[0].Image != null ? "https:" + songsResponse.Data.Results[0].Image : null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
            else
            {
                await ctx.RespondAsync("Wystąpił błąd.");
            }
        }

        [Command("piosenka")]
        [Description("Pokazuje piosenkę.\n------------------------------\nSkąd wziąć japońską nazwę ?\nKomendą `wyszukajEvent 1 <nazwa japońska bądź angielska>`")]
        public async Task GetJapanEvent(CommandContext ctx, [Description("Nazwa piosenki po japońsku."), RemainingText] string name)
        {
            await ctx.TriggerTypingAsync();

            var songObject = _schoolidoluService.GetSongByName(name);

            if (songObject.StatusCode == HttpStatusCode.OK)
            {
                EventObject eventObject = null;
                if(songObject.Data.Event != null)
                {
                    var songObjectwithEvent = _schoolidoluService.GetSongByNameWithEvent(name);
                    eventObject = songObjectwithEvent.Data.Event;
                }
                await PostEmbedHelper.PostEmbed(ctx, "Piosenka", _schoolidoluHelper.MakeSongDescription(songObject.Data, eventObject),
                    songObject.Data.Image != null ? "https:" + songObject.Data.Image : null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "Piosenka",
                    "Wystąpił błąd podczas pobierania piosnki. Sprawdź czy podałeś poprawną nazwę. Pamiętaj aby podać japońską nazwę piosenki.",
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("wyszukajPiosenke")]
        [Description("Wyszukuje piosenki.\nnp:\n`wyszukajPiosenke 1 Snow`\nPolecam jako początkową stronę podać `1`." +
            "\nWyszukiwanie odbywa się po nazwach.")]
        public async Task SearchIdol(CommandContext ctx, [Description("Strona wyników.")] string page, [Description("Słowa kluczowe."), RemainingText] string keywords)
        {
            await ctx.TriggerTypingAsync();

            int intPage;

            if (!int.TryParse(page, out intPage))
            {
                await PostEmbedHelper.PostEmbed(ctx, "Wyszukiwanie piosenek", "Wystąpił błąd podczas wyszukiwania piosenek. Przed zapytaniem podaj numer strony.\nnp. `wyszukajPiosenke 1 Snow`",
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                return;
            }

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "search", keywords },
                { "page", intPage.ToString() }
            };

            var songObject = _schoolidoluService.GetSong(options);

            if (songObject.StatusCode == HttpStatusCode.OK)
            {

                if (songObject.Data.Count != 0)
                {
                    await PostEmbedHelper.PostEmbed(ctx, "Wyszukiwanie piosenek", _schoolidoluHelper.MakeSearchSongDescription(songObject.Data, 10, intPage),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, "Wyszukiwanie piosenek", "Brak wyników, spróbuj wyszukać inną frazę.",
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "Wyszukiwanie piosenek",
                    "Wystąpił błąd podczas pobierania piosenek. Mogło nastąpić odwołanie do nieistniejącej strony. Spróbuj wybrać stronę pierwszą.\n`wyszukajPiosenke 1 " + keywords + "`",
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }
    }
}
