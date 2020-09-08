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
                        await PostEmbedHelper.PostEmbed(ctx, "Obecny event EN", _schoolidoluHelper.MakeWorldEventDescription(eventObject.Data.Results[0], eventCards), "https:" + eventObject.Data.Results[0].English_image, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Obecny event EN", _schoolidoluHelper.MakeWorldEventDescription(eventObject.Data.Results[0], eventCards), null, SchoolidoluHelper.GetSchoolidoluFotter());
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
                        await PostEmbedHelper.PostEmbed(ctx, "Obecny event JP", _schoolidoluHelper.MakeJapanEventDescription(eventObject.Data.Results[0], eventCards), "https:" + eventObject.Data.Results[0].Image, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Obecny event JP", _schoolidoluHelper.MakeJapanEventDescription(eventObject.Data.Results[0], eventCards), null, SchoolidoluHelper.GetSchoolidoluFotter());
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
                        await PostEmbedHelper.PostEmbed(ctx, "Następny event EN", _schoolidoluHelper.MakeWorldEventDescription(eventObject.Data.Results[0]), "https:" + eventObject.Data.Results[0].English_image, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Następny event EN", _schoolidoluHelper.MakeWorldEventDescription(eventObject.Data.Results[0]), null, SchoolidoluHelper.GetSchoolidoluFotter());
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
                        await PostEmbedHelper.PostEmbed(ctx, "Następny event JP", _schoolidoluHelper.MakeJapanEventDescription(eventObject.Data.Results[0]), "https:" + eventObject.Data.Results[0].Image, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Następny event JP", _schoolidoluHelper.MakeJapanEventDescription(eventObject.Data.Results[0]), null, SchoolidoluHelper.GetSchoolidoluFotter());
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
