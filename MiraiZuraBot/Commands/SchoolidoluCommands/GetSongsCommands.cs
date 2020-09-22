using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Containers.Schoolidolu.Event;
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
    class GetSongsCommands : BaseCommandModule
    {
        private SchoolidoluService _schoolidoluService;
        private SchoolidoluHelper _schoolidoluHelper;
        private LanguageService _languageService;
        private Translator _translator;

        public GetSongsCommands(SchoolidoluService schoolidoluService, SchoolidoluHelper schoolidoluHelper, LanguageService languageService, Translator translator)
        {
            _schoolidoluService = schoolidoluService;
            _schoolidoluHelper = schoolidoluHelper;
            _languageService = languageService;
            _translator = translator;
        }

        [Command("losowaPiosenka")]
        [Description("Pokazuje losową piosenkę.\nnp:\n*losowaPiosenka")]
        public async Task RandomSong(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "random" },
                { "page_size", "1" }
            };

            var songsResponse = _schoolidoluService.GetSong(options);

            if (songsResponse.StatusCode == HttpStatusCode.OK)
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "songRandom"), _schoolidoluHelper.MakeSongDescription(lang, songsResponse.Data.Results[0]),
                    songsResponse.Data.Results[0].Image != null ? "https:" + songsResponse.Data.Results[0].Image : null, null, SchoolidoluHelper.GetSchoolidoluFotter(),
                    _schoolidoluHelper.GetColorForAttribute(songsResponse.Data.Results[0].Attribute));
            }
            else
            {
                await ctx.RespondAsync(_translator.GetString(lang, "songRandomError"));
            }
        }

        [Command("piosenka")]
        [Description("Pokazuje piosenkę.\n------------------------------\nSkąd wziąć japońską nazwę ?\nKomendą `wyszukajEvent 1 <nazwa japońska bądź angielska>`")]
        public async Task GetJapanEvent(CommandContext ctx, [Description("Nazwa piosenki po japońsku."), RemainingText] string name)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            var songObject = _schoolidoluService.GetSongByName(name);

            if (songObject.StatusCode == HttpStatusCode.OK)
            {
                EventObject eventObject = null;
                if(songObject.Data.Event != null)
                {
                    var songObjectwithEvent = _schoolidoluService.GetSongByNameWithEvent(name);
                    eventObject = songObjectwithEvent.Data.Event;
                }
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "song"), _schoolidoluHelper.MakeSongDescription(lang, songObject.Data, eventObject),
                    songObject.Data.Image != null ? "https:" + songObject.Data.Image : null, null, SchoolidoluHelper.GetSchoolidoluFotter(),
                    _schoolidoluHelper.GetColorForAttribute(songObject.Data.Attribute));
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "song"),
                    _translator.GetString(lang, "songError"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("wyszukajPiosenke")]
        [Description("Wyszukuje piosenki.\nnp:\n`wyszukajPiosenke 1 Snow`\nPolecam jako początkową stronę podać `1`." +
            "\nWyszukiwanie odbywa się po nazwach.")]
        public async Task SearchIdol(CommandContext ctx, [Description("Strona wyników.")] string page, [Description("Słowa kluczowe."), RemainingText] string keywords)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            int intPage;

            if (!int.TryParse(page, out intPage))
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "songSearch"), _translator.GetString(lang, "songSearchNoPage"),
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
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "songSearch"), _schoolidoluHelper.MakeSearchSongDescription(lang, songObject.Data, 10, intPage),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "songSearch"), _translator.GetString(lang, "songSearchNoResult"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "songSearch"),
                    string.Format(_translator.GetString(lang, "songSearchError"), keywords.Trim()),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }
    }
}
