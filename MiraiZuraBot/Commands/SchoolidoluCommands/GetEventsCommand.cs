using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Containers.Schoolidolu;
using MiraiZuraBot.Containers.Schoolidolu.Cards;
using MiraiZuraBot.Containers.Schoolidolu.Event;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Helpers.SchoolidoluHelper;
using MiraiZuraBot.Services.SchoolidoluService;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.SchoolidoluCommands
{
    [CommandsGroup("SIF")]
    class GetEventsCommand : BaseCommandModule
    {
        private SchoolidoluService _schoolidoluService;
        private SchoolidoluHelper _schoolidoluHelper;

        public GetEventsCommand(SchoolidoluService schoolidoluService, SchoolidoluHelper schoolidoluHelper)
        {
            _schoolidoluService = schoolidoluService;
            _schoolidoluHelper = schoolidoluHelper;
        }

        [Command("obecnyEventEN")]
        [Description("Pokazuje obecnie trwający event na serwerze EN.")]
        public async Task CurrentWorldEvent(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

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
                        await PostEmbedHelper.PostEmbed(ctx, "Obecny event EN", _schoolidoluHelper.MakeCurrentWorldEventDescription(eventObject.Data.Results[0], false, eventCards),
                            "https:" + eventObject.Data.Results[0].English_image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Obecny event EN", _schoolidoluHelper.MakeCurrentWorldEventDescription(eventObject.Data.Results[0], false, eventCards),
                            null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    } 
                }
                else
                {
                    await ctx.RespondAsync("Obecnie na serwerze EN nie trwa żaden event. Możesz spróbować sprawdzić nadchodzący event");
                }
            }
            else
            {
                await ctx.RespondAsync("Wystąpił błąd podczas pobierania eventu.");
            }
        }

        [Command("obecnyEventJP")]
        [Description("Pokazuje obecnie trwający event na serwerze JP.")]
        public async Task CurrentJapanEvent(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

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
                        await PostEmbedHelper.PostEmbed(ctx, "Obecny event JP", _schoolidoluHelper.MakeCurrentJapanEventDescription(eventObject.Data.Results[0], false, eventCards),
                            "https:" + eventObject.Data.Results[0].Image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Obecny event JP", _schoolidoluHelper.MakeCurrentJapanEventDescription(eventObject.Data.Results[0], false, eventCards),
                            null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                }
                else
                {
                    await ctx.RespondAsync("Obecnie na serwerze JP nie trwa żaden event. Możesz spróbować sprawdzić nadchodzący event");
                }
            }
            else
            {
                await ctx.RespondAsync("Wystąpił błąd podczas pobierania eventu.");
            }
        }

        [Command("nastepnyEventEN")]
        [Description("Pokazuje następny event na serwerze EN.")]
        public async Task NextWorldEvent(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

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
                        await PostEmbedHelper.PostEmbed(ctx, "Następny event EN", _schoolidoluHelper.MakeCurrentWorldEventDescription(eventObject.Data.Results[0], false),
                            "https:" + eventObject.Data.Results[0].English_image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Następny event EN", _schoolidoluHelper.MakeCurrentWorldEventDescription(eventObject.Data.Results[0], false), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                }
                else
                {
                    await ctx.RespondAsync("Obecnie na serwerze EN nie ma zapowiedzianego żadnego eventu. Możesz spróbować sprawdzić obecny event.");
                }
            }
            else
            {
                await ctx.RespondAsync("Wystąpił błąd podczas pobierania eventu.");
            }
        }

        [Command("nastepnyEventJP")]
        [Description("Pokazuje następny event na serwerze JP.")]
        public async Task NextJapanEvent(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

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
                        await PostEmbedHelper.PostEmbed(ctx, "Następny event JP", _schoolidoluHelper.MakeCurrentJapanEventDescription(eventObject.Data.Results[0], false),"https:" + eventObject.Data.Results[0].Image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Następny event JP", _schoolidoluHelper.MakeCurrentJapanEventDescription(eventObject.Data.Results[0], false), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                }
                else
                {
                    await ctx.RespondAsync("Obecnie na serwerze JP nie ma zapowiedzianego żadnego eventu. Możesz spróbować sprawdzić obecny event.");
                }
            }
            else
            {
                await ctx.RespondAsync("Wystąpił błąd podczas pobierania eventu.");
            }
        }

        [Command("poprzedniEventEN")]
        [Description("Pokazuje ostatnio zakończony event na serwerze EN.")]
        public async Task LastFinishedWorldEvent(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

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
                    await PostEmbedHelper.PostEmbed(ctx, "Poprzedni event EN", _schoolidoluHelper.MakeCurrentWorldEventDescription(eventObject.Data.Results[i], true, eventCards),
                        "https:" + eventObject.Data.Results[i].English_image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, "Poprzedni event EN", _schoolidoluHelper.MakeCurrentWorldEventDescription(eventObject.Data.Results[i], true, eventCards),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await ctx.RespondAsync("Wystąpił błąd podczas pobierania eventu.");
            }
        }

        [Command("poprzedniEventJP")]
        [Description("Pokazuje ostatnio zakończony event na serwerze JP.")]
        public async Task LastFinishedJapanEvent(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

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
                    await PostEmbedHelper.PostEmbed(ctx, "Poprzedni event JP", _schoolidoluHelper.MakeCurrentJapanEventDescription(eventObject.Data.Results[i], true, eventCards),
                        "https:" + eventObject.Data.Results[i].Image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, "Poprzedni event JP", _schoolidoluHelper.MakeCurrentJapanEventDescription(eventObject.Data.Results[i], true, eventCards),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await ctx.RespondAsync("Wystąpił błąd podczas pobierania eventu.");
            }
        }

        [Command("eventEN")]
        [Description("Pokazuje event na serwerze EN.")]
        public async Task GetWorldEvent(CommandContext ctx, [Description("Nazwa eventu po japońsku."), RemainingText] string name)
        {
            await ctx.TriggerTypingAsync();

            var eventObject = _schoolidoluService.GetEventByName(name);

            if (eventObject.StatusCode == HttpStatusCode.OK)
            {
                List<CardObject> eventCards = GetCardsForEvent(eventObject.Data, true);

                bool finished = true;
                if(eventObject.Data.English_status == "announced" || eventObject.Data.English_status == "ongoing")
                {
                    finished = false;
                }

                if (eventObject.Data.English_image != null)
                {
                    await PostEmbedHelper.PostEmbed(ctx, "Event EN", _schoolidoluHelper.MakeCurrentWorldEventDescription(eventObject.Data, finished, eventCards),
                        "https:" + eventObject.Data.English_image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, "Event EN", _schoolidoluHelper.MakeCurrentWorldEventDescription(eventObject.Data, finished, eventCards),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await ctx.RespondAsync("Wystąpił błąd podczas pobierania eventu. Sprawdź czy podałeś poprawną nazwę. Pamiętaj aby podać japońską nazwę eventu.");
            }
        }

        [Command("eventJP")]
        [Description("Pokazuje event na serwerze JP.")]
        public async Task GetJapanEvent(CommandContext ctx, [Description("Nazwa eventu po japońsku."), RemainingText] string name)
        {
            await ctx.TriggerTypingAsync();

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
                    await PostEmbedHelper.PostEmbed(ctx, "Event JP", _schoolidoluHelper.MakeCurrentJapanEventDescription(eventObject.Data, finished, eventCards),
                        "https:" + eventObject.Data.Image, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, "Event JP", _schoolidoluHelper.MakeCurrentJapanEventDescription(eventObject.Data, finished, eventCards),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await ctx.RespondAsync("Wystąpił błąd podczas pobierania eventu. Sprawdź czy podałeś poprawną nazwę. Pamiętaj aby podać japońską nazwę eventu.");
            }
        }

        private List<CardObject> GetCardsForEvent(EventObject eventObject, bool isWorld)
        {
            List<CardObject> eventCards = null;
            if (isWorld == true)
            {
                if (eventObject.English_name != null)
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
