using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Containers.Schoolidolu.Cards;
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

namespace MiraiZuraBot.Commands.SchoolidoluCommands
{
    [GroupLang("SIF", "SIF")]
    class GetEventsCommand : BaseCommandModule
    {
        private SchoolidoluService _schoolidoluService;
        private SchoolidoluHelper _schoolidoluHelper;
        private LanguageService _languageService;
        private Translator _translator;

        public GetEventsCommand(SchoolidoluService schoolidoluService, SchoolidoluHelper schoolidoluHelper, LanguageService languageService, Translator translator)
        {
            _schoolidoluService = schoolidoluService;            
            _schoolidoluHelper = schoolidoluHelper;
            _languageService = languageService;
            _translator = translator;
        }

        [Command("obecnyEventEN")]
        [Aliases("currentEventEN")]
        [CommandLang("obecnyEventEN", "currentEventEN")]
        [DescriptionLang("Pokazuje obecnie trwający event na serwerze EN.", "Shows the current event on the EN server.")]
        public async Task CurrentWorldEvent(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "-english_end" },
                { "page_size", "1" }
            };

            var eventObject = _schoolidoluService.GetEvent(options);

            if (eventObject.StatusCode == HttpStatusCode.OK)
            {
                if (eventObject.Data.Results[0].World_current == true)
                {
                    List<CardObject> eventCards = GetCardsForEvent(eventObject.Data.Results[0], true);

                    if (eventObject.Data.Results[0].English_image != null)
                    {
                        await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventCurrentEN"),
                            _schoolidoluHelper.MakeCurrentWorldEventDescription(lang, eventObject.Data.Results[0], false, eventCards),
                            "https:" + eventObject.Data.Results[0].English_image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventCurrentEN"),
                            _schoolidoluHelper.MakeCurrentWorldEventDescription(lang, eventObject.Data.Results[0], false, eventCards),
                            null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    } 
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventCurrentEN"), _translator.GetString(lang, "eventNoCurrentEN"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventCurrentEN"), _translator.GetString(lang, "eventCurrentENError"), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("obecnyEventJP")]
        [Aliases("currentEventJP")]
        [CommandLang("obecnyEventJP", "currentEventJP")]
        [DescriptionLang("Pokazuje obecnie trwający event na serwerze JP.", "Shows the current event on the JP server.")]
        public async Task CurrentJapanEvent(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "-end" },
                { "page_size", "1" }
            };

            var eventObject = _schoolidoluService.GetEvent(options);

            if (eventObject.StatusCode == HttpStatusCode.OK)
            {
                if (eventObject.Data.Results[0].Japan_current == true)
                {
                    List<CardObject> eventCards = GetCardsForEvent(eventObject.Data.Results[0], false);

                    if (eventObject.Data.Results[0].Image != null)
                    {
                        await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventCurrentJP"),
                            _schoolidoluHelper.MakeCurrentJapanEventDescription(lang, eventObject.Data.Results[0], false, eventCards),
                            "https:" + eventObject.Data.Results[0].Image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventCurrentJP"),
                            _schoolidoluHelper.MakeCurrentJapanEventDescription(lang, eventObject.Data.Results[0], false, eventCards),
                            null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventCurrentJP"), _translator.GetString(lang, "eventNoCurrentJP"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventCurrentJP"), _translator.GetString(lang, "eventCurrentJPError"), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("nastepnyEventEN")]
        [Aliases("nextEventEN")]
        [CommandLang("nastepnyEventEN", "nextEventEN")]
        [DescriptionLang("Pokazuje następny event na serwerze EN.", "Shows the next event on the EN server.")]
        public async Task NextWorldEvent(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "-english_end" },
                { "page_size", "1" }
            };

            var eventObject = _schoolidoluService.GetEvent(options);

            if (eventObject.StatusCode == HttpStatusCode.OK)
            {
                if (eventObject.Data.Results[0].English_status == "announced")
                {
                    if (eventObject.Data.Results[0].English_image != null)
                    {
                        await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventNextEN"), _schoolidoluHelper.MakeCurrentWorldEventDescription(lang, eventObject.Data.Results[0], false),
                            "https:" + eventObject.Data.Results[0].English_image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventNextEN"), _schoolidoluHelper.MakeCurrentWorldEventDescription(lang, eventObject.Data.Results[0], false), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventNextEN"), _translator.GetString(lang, "eventNoNextEN"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventNextEN"), _translator.GetString(lang, "eventNextENError"), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("nastepnyEventJP")]
        [Aliases("nextEventJP")]
        [CommandLang("nastepnyEventJP", "nextEventJP")]
        [DescriptionLang("Pokazuje następny event na serwerze JP.", "Shows the next event on the JP server.")]
        public async Task NextJapanEvent(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "-end" },
                { "page_size", "1" }
            };

            var eventObject = _schoolidoluService.GetEvent(options);

            if (eventObject.StatusCode == HttpStatusCode.OK)
            {
                if (eventObject.Data.Results[0].Japan_status == "announced")
                {
                    if (eventObject.Data.Results[0].Image != null)
                    {
                        await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventNextJP"),
                            _schoolidoluHelper.MakeCurrentJapanEventDescription(lang, eventObject.Data.Results[0], false),"https:" + eventObject.Data.Results[0].Image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventNextJP"),
                            _schoolidoluHelper.MakeCurrentJapanEventDescription(lang, eventObject.Data.Results[0], false), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventNextJP"), _translator.GetString(lang, "eventNoNextJP"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventNextJP"), _translator.GetString(lang, "eventNextJPError"), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("poprzedniEventEN")]
        [Aliases("previousEventEN")]
        [CommandLang("poprzedniEventEN", "previousEventEN")]
        [DescriptionLang("Pokazuje ostatnio zakończony event na serwerze EN.", "Shows the recent completed event on the EN server.")]
        public async Task LastFinishedWorldEvent(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "-english_end" },
                { "page_size", "3" }
            };

            var eventObject = _schoolidoluService.GetEvent(options);

            if (eventObject.StatusCode == HttpStatusCode.OK)
            {
                int i = 0;
                for (; i < 3; i++)
                {
                    if (eventObject.Data.Results[i].English_status == "finished")
                    {
                        break;
                    }
                }
                List<CardObject> eventCards = GetCardsForEvent(eventObject.Data.Results[0], true);

                if (eventObject.Data.Results[i].English_image != null)
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventPrevEN"),
                        _schoolidoluHelper.MakeCurrentWorldEventDescription(lang, eventObject.Data.Results[i], true, eventCards),
                        "https:" + eventObject.Data.Results[i].English_image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventPrevEN"),
                        _schoolidoluHelper.MakeCurrentWorldEventDescription(lang, eventObject.Data.Results[i], true, eventCards),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventPrevEN"), _translator.GetString(lang, "eventPrevENError"), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("poprzedniEventJP")]
        [Aliases("previousEventJP")]
        [CommandLang("poprzedniEventJP", "previousEventJP")]
        [DescriptionLang("Pokazuje ostatnio zakończony event na serwerze JP.", "Shows the recent completed event on the JP server.")]
        public async Task LastFinishedJapanEvent(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "-end" },
                { "page_size", "3" }
            };

            var eventObject = _schoolidoluService.GetEvent(options);

            if (eventObject.StatusCode == HttpStatusCode.OK)
            {
                int i = 0;
                for (; i < 3; i++)
                {
                    if (eventObject.Data.Results[i].Japan_status == "finished")
                    {
                        break;
                    }
                }
                List<CardObject> eventCards = GetCardsForEvent(eventObject.Data.Results[0], false);

                if (eventObject.Data.Results[i].Image != null)
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventPrevJP"),
                        _schoolidoluHelper.MakeCurrentJapanEventDescription(lang, eventObject.Data.Results[i], true, eventCards),
                        "https:" + eventObject.Data.Results[i].Image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventPrevJP"),
                        _schoolidoluHelper.MakeCurrentJapanEventDescription(lang, eventObject.Data.Results[i], true, eventCards),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventPrevJP"), _translator.GetString(lang, "eventPrevJPError"), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("eventEN")]
        [CommandLang("eventEN", "eventEN")]
        [DescriptionLang("Pokazuje event na serwerze EN.\n------------------------------\nDlaczego nazwa eventu po japońsku?" +
            "\nKomenda pobiera jeden event korzystając z endpointa który wyszukuje event po jego dokładnej japońskiej nazwie." +
            "\n------------------------------\nSkąd wziąć japońską nazwę?\nKomendą `wyszukajEvent 1 <nazwa japońska bądź angielska>`",
            "Shows event on the EN server.\n------------------------------\nWhy the name of the event in Japanese?" +
            "\nThe command takes one event using an endpoint that searches for an event by its exact Japanese name." +
            "\n------------------------------\nWhere to get the Japanese name?\nBy command `searchEvent 1  <Japanese or English name>`")]
        public async Task GetWorldEvent(CommandContext ctx, [DescriptionLang("Nazwa eventu po japońsku", "The name of the event in Japanese"), ParameterLang("Nazwa", "Name"), RemainingText] string name)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            var eventObject = _schoolidoluService.GetEventByName(name);

            if (eventObject.StatusCode == HttpStatusCode.OK)
            {
                List<CardObject> eventCards = null;
                // The event at EN might not have happened
                if (eventObject.Data.English_name != null && eventObject.Data.English_name != "")
                {
                    eventCards = GetCardsForEvent(eventObject.Data, true);
                }                

                bool finished = true;
                if(eventObject.Data.English_status == "announced" || eventObject.Data.English_status == "ongoing")
                {
                    finished = false;
                }

                if (eventObject.Data.English_image != null)
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventEN"), _schoolidoluHelper.MakeCurrentWorldEventDescription(lang, eventObject.Data, finished, eventCards),
                        "https:" + eventObject.Data.English_image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventEN"), _schoolidoluHelper.MakeCurrentWorldEventDescription(lang, eventObject.Data, finished, eventCards),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventEN"),
                    _translator.GetString(lang, "eventENError"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("eventJP")]
        [CommandLang("eventJP", "eventJP")]
        [DescriptionLang("Pokazuje event na serwerze JP.\n------------------------------\nSkąd wziąć japońską nazwę?\nKomendą `wyszukajEvent 1 <nazwa japońska bądź angielska>`",
            "Shows event on the JP server.\n------------------------------\nWhere to get the Japanese name?\nBy command `searchEvent 1  <Japanese or English name>`")]
        public async Task GetJapanEvent(CommandContext ctx, [DescriptionLang("Nazwa eventu po japońsku", "The name of the event in Japanese"), ParameterLang("Nazwa", "Name"), RemainingText] string name)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            var eventObject = _schoolidoluService.GetEventByName(name);

            if (eventObject.StatusCode == HttpStatusCode.OK)
            {
                List<CardObject> eventCards = GetCardsForEvent(eventObject.Data, false);

                bool finished = true;
                if (eventObject.Data.Japan_status == "announced" || eventObject.Data.Japan_status == "ongoing")
                {
                    finished = false;
                }

                if (eventObject.Data.Image != null)
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventJP"), _schoolidoluHelper.MakeCurrentJapanEventDescription(lang, eventObject.Data, finished, eventCards),
                        "https:" + eventObject.Data.Image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventJP"), _schoolidoluHelper.MakeCurrentJapanEventDescription(lang, eventObject.Data, finished, eventCards),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventJP"),
                    _translator.GetString(lang, "eventJPError"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("losowyEventEN")]
        [Aliases("randomEventEN")]
        [CommandLang("losowyEventEN", "randomEventEN")]
        [DescriptionLang("Pokazuje losowy event z serwera EN.", "Shows a random event from the EN server.")]
        public async Task GetRandomWorldEvent(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "random" },
                { "is_english", "True" },
                { "page_size", "1"}
            };

            var eventObject = _schoolidoluService.GetEvent(options);

            if (eventObject.StatusCode == HttpStatusCode.OK)
            {
                List<CardObject> eventCards = null;
                // The event at EN might not have happened
                if (eventObject.Data.Results[0].English_name != null && eventObject.Data.Results[0].English_name != "")
                {
                    eventCards = GetCardsForEvent(eventObject.Data.Results[0], true);
                }

                bool finished = true;
                if (eventObject.Data.Results[0].English_status == "announced" || eventObject.Data.Results[0].English_status == "ongoing")
                {
                    finished = false;
                }

                if (eventObject.Data.Results[0].English_image != null)
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventRandomEN"),
                        _schoolidoluHelper.MakeCurrentWorldEventDescription(lang, eventObject.Data.Results[0], finished, eventCards),
                        "https:" + eventObject.Data.Results[0].English_image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventRandomEN"),
                        _schoolidoluHelper.MakeCurrentWorldEventDescription(lang, eventObject.Data.Results[0], finished, eventCards),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventRandomEN"),
                    _translator.GetString(lang, "eventRandomENError"), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("losowyEventJP")]
        [Aliases("randomEventJP")]
        [CommandLang("losowyEventJP", "randomEventJP")]
        [DescriptionLang("Pokazuje losowy event z serwera JP.", "Shows a random event from the JP server.")]
        public async Task GetRandomJapanEvent(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "random" },
                { "page_size", "1"}
            };

            var eventObject = _schoolidoluService.GetEvent(options);

            if (eventObject.StatusCode == HttpStatusCode.OK)
            {
                List<CardObject> eventCards = GetCardsForEvent(eventObject.Data.Results[0], false);

                bool finished = true;
                if (eventObject.Data.Results[0].Japan_status == "announced" || eventObject.Data.Results[0].Japan_status == "ongoing")
                {
                    finished = false;
                }

                if (eventObject.Data.Results[0].Image != null)
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventRandomJP"),
                        _schoolidoluHelper.MakeCurrentJapanEventDescription(lang, eventObject.Data.Results[0], finished, eventCards),
                        "https:" + eventObject.Data.Results[0].Image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventRandomJP"),
                        _schoolidoluHelper.MakeCurrentJapanEventDescription(lang, eventObject.Data.Results[0], finished, eventCards),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventRandomJP"),
                    _translator.GetString(lang, "eventRandomJPError"), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("wyszukajEvent")]
        [Aliases("searchEvent")]
        [CommandLang("wyszukajEvent", "searchEvent")]
        [DescriptionLang("Wyszukuje nazwy eventów.\nnp:\n`wyszukajEvent 1 Medley`\nPolecam jako początkową stronę podać `1`.",
            "Search for events.\ne.g.\n`searchEvent 1 Medley`\nI recommend to choose `1` as the initial page.")]
        public async Task SearchEvent(CommandContext ctx, [DescriptionLang("Strona wyników", "Result page"), ParameterLang("Strona", "Page")] string page,
            [DescriptionLang("Fraza do wyszukania", "The phrase to search for"), ParameterLang("Słowa kluczowe", "Keywords"), RemainingText] string keywords)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            int intPage;

            if (!int.TryParse(page, out intPage))
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventSearch"), _translator.GetString(lang, "eventSearchNoPage"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                return;
            }

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "search", keywords },
                { "page", intPage.ToString() }
            };

            var eventObject = _schoolidoluService.GetEvent(options);

            if (eventObject.StatusCode == HttpStatusCode.OK)
            {

                if (eventObject.Data.Count != 0)
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventSearch"), _schoolidoluHelper.MakeSearchEventDescription(lang, eventObject.Data, 10, intPage),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventSearch"), _translator.GetString(lang, "eventSearchNoResult"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "eventSearch"),
                    string.Format(_translator.GetString(lang, "eventSearchError"), keywords.Trim()),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        private List<CardObject> GetCardsForEvent(EventObject eventObject, bool isWorld)
        {
            List<CardObject> eventCards = null;
            if (isWorld == true)
            {
                if (eventObject.English_name != null && eventObject.English_name != "")
                {
                    Dictionary<string, string> eventCardsOptions = new Dictionary<string, string>
                    {
                        { "event_english_name", eventObject.English_name }
                    };
                    var cards = _schoolidoluService.GetCard(eventCardsOptions);
                    if (cards.StatusCode == HttpStatusCode.OK)
                    {
                        eventCards = cards.Data.Results;
                    }
                }
            }
            else
            {
                Dictionary<string, string> eventCardsOptions = new Dictionary<string, string>
                {
                    { "event_japanese_name", eventObject.Japanese_name }
                };
                var cards = _schoolidoluService.GetCard(eventCardsOptions);
                if (cards.StatusCode == HttpStatusCode.OK)
                {
                    eventCards = cards.Data.Results;
                }
            }

            return eventCards;
        }
    }
}
