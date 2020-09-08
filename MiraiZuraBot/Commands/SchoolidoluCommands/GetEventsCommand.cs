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

        public GetEventsCommand(SchoolidoluService schoolidoluService)
        {
            _schoolidoluService = schoolidoluService;
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
                        await PostEmbedHelper.PostEmbed(ctx, "Obecny event EN", MakeWorldEventDescription(eventObject.Data.Results[0], eventCards), "https:" + eventObject.Data.Results[0].English_image, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Obecny event EN", MakeWorldEventDescription(eventObject.Data.Results[0], eventCards), null, SchoolidoluHelper.GetSchoolidoluFotter());
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
                        await PostEmbedHelper.PostEmbed(ctx, "Obecny event JP", MakeJapanEventDescription(eventObject.Data.Results[0], eventCards), "https:" + eventObject.Data.Results[0].Image, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Obecny event JP", MakeJapanEventDescription(eventObject.Data.Results[0], eventCards), null, SchoolidoluHelper.GetSchoolidoluFotter());
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
                        await PostEmbedHelper.PostEmbed(ctx, "Następny event EN", MakeWorldEventDescription(eventObject.Data.Results[0]), "https:" + eventObject.Data.Results[0].English_image, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Następny event EN", MakeWorldEventDescription(eventObject.Data.Results[0]), null, SchoolidoluHelper.GetSchoolidoluFotter());
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
                        await PostEmbedHelper.PostEmbed(ctx, "Następny event JP", MakeJapanEventDescription(eventObject.Data.Results[0]), "https:" + eventObject.Data.Results[0].Image, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Następny event JP", MakeJapanEventDescription(eventObject.Data.Results[0]), null, SchoolidoluHelper.GetSchoolidoluFotter());
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

        private string MakeWorldEventDescription(EventObject eventObject, List<CardObject> eventCards = null)
        {
            StringBuilder eventDescription = new StringBuilder();
            SchoolidoluHelper.AddTitledLineToStringBuilder(eventDescription, ":name_badge: **Nazwa** ", eventObject.English_name);            
            SchoolidoluHelper.AddDateTimeToStringBuilder(eventDescription, ":clock2: **Czas trwania** ", ConvertToPolandTimeFromUtc(eventObject.English_beginning), ConvertToPolandTimeFromUtc(eventObject.English_end));
            SchoolidoluHelper.AddTitledLineToStringBuilder(eventDescription, ":timer: **Pozostały czas** ", GetTimeToEventEnd(ConvertToPolandTimeFromUtc(eventObject.English_end)));
            SchoolidoluHelper.AddDateTimeToStringBuilder(eventDescription, ":clock12: **Czas trwania (UTC)** ", eventObject.English_beginning, eventObject.English_end);
            SchoolidoluHelper.AddUrlToStringBuilder(eventDescription, ":globe_with_meridians: **URL** ", "schoolido.lu", eventObject.Website_url);
            SchoolidoluHelper.AddTitledLineToStringBuilder(eventDescription, ":notepad_spiral: **Dodatkowe informacje** ", eventObject.Note);

            if(eventCards != null)
            {
                SchoolidoluHelper.AddLineToStringBuilder(eventDescription, ":microphone: **Karty** ", eventCards.Count);
                foreach(CardObject eventCard in eventCards)
                {
                    SchoolidoluHelper.AddLineToStringBuilder(eventDescription, eventCard.Idol.Name, eventCard.Id);
                }
                eventDescription.AppendLine().Append("*Możesz użyć komendy `karta <id>` aby uzyskać więcej informacji o danej karcie*");
            }

            return eventDescription.ToString();
        }

        private string MakeJapanEventDescription(EventObject eventObject, List<CardObject> eventCards = null)
        {
            StringBuilder eventDescription = new StringBuilder();
            SchoolidoluHelper.AddTitledLineToStringBuilder(eventDescription, ":name_badge: **Nazwa** ", eventObject.Japanese_name, eventObject.Romaji_name);
            SchoolidoluHelper.AddDateTimeToStringBuilder(eventDescription, ":clock2: **Czas trwania** ", eventObject.Beginning, eventObject.End);
            SchoolidoluHelper.AddTitledLineToStringBuilder(eventDescription, ":timer: **Pozostały czas** ", GetTimeToEventEnd(eventObject.End));
            SchoolidoluHelper.AddDateTimeToStringBuilder(eventDescription, ":clock9: **Czas trwania (JST)** ", ConvertToJapanTimeFromPoland(eventObject.Beginning), ConvertToJapanTimeFromPoland(eventObject.End));
            SchoolidoluHelper.AddUrlToStringBuilder(eventDescription, ":globe_with_meridians: **URL** ", "schoolido.lu", eventObject.Website_url);
            SchoolidoluHelper.AddTitledLineToStringBuilder(eventDescription, ":notepad_spiral: **Dodatkowe informacje** ", eventObject.Note);

            if (eventCards != null)
            {
                SchoolidoluHelper.AddLineToStringBuilder(eventDescription, ":microphone: **Karty** ", eventCards.Count);
                foreach (CardObject eventCard in eventCards)
                {
                    SchoolidoluHelper.AddLineToStringBuilder(eventDescription, eventCard.Idol.Name, eventCard.Id);
                }
                eventDescription.AppendLine().Append("*Możesz użyć komendy `karta <id>` aby uzyskać więcej informacji o danej karcie*");
            }

            return eventDescription.ToString();
        }

        private DateTime? ConvertToPolandTimeFromUtc(DateTime? time)
        {
            if (time == null)
            {
                return null;
            }
            else
            {
                TimeZoneInfo polandTimeZone;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    polandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                }
                else
                {
                    polandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");
                }
                return TimeZoneInfo.ConvertTimeFromUtc(time.Value, polandTimeZone);
            }
        }

        // Because .NET auto convert Japan Time to Poland Time during DateTime parsing so this is needed
        private DateTime? ConvertToJapanTimeFromPoland(DateTime? time)
        {
            if (time == null)
            {
                return null;
            }
            else
            {
                TimeZoneInfo japanTimeZone;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    japanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
                }
                else
                {
                    japanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Japan");
                }
                return TimeZoneInfo.ConvertTimeFromUtc(time.Value.ToUniversalTime(), japanTimeZone);
            }
        }

        private string GetTimeToEventEnd(DateTime? time)
        {
            if (time == null)
            {
                return null;
            }
            else
            {
                var difference = time - DateTime.Now;

                StringBuilder result = new StringBuilder();
                result.Append(difference.Value.Days)
                      .Append(" dni ")
                      .Append(difference.Value.Hours)
                      .Append(" godzin ")
                      .Append(difference.Value.Minutes)
                      .Append(" minut ")
                      .Append(difference.Value.Seconds)
                      .Append(" sekund ");

                return result.ToString();
            }
        }
    }
}
