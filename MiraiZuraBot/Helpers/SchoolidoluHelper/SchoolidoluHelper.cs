using DSharpPlus.Entities;
using MiraiZuraBot.Containers.Schoolidolu.Cards;
using MiraiZuraBot.Containers.Schoolidolu.Event;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MiraiZuraBot.Helpers.SchoolidoluHelper
{
    class SchoolidoluHelper
    {
        public static DiscordEmbedBuilder.EmbedFooter GetSchoolidoluFotter()
        {
            return new DiscordEmbedBuilder.EmbedFooter { Text = "Powered by schoolido.lu", IconUrl = "https://i.schoolido.lu/android/icon.png" };
        }

        public void AddLineToStringBuilder<T>(StringBuilder builder, string description, T field)
        {
            if (field != null && field.ToString() != "")
            {
                builder.Append(description).Append(" (").Append(field).Append(")").AppendLine();
            }
        }

        public void AddTitledLineToStringBuilder<T>(StringBuilder builder, string description, T field)
        {
            if (field != null && field.ToString() != "")
            {
                builder.Append(description).AppendLine().Append(field).AppendLine();
            }
        }

        public void AddTitledLineToStringBuilder<T, U>(StringBuilder builder, string description, T field, U field2)
        {
            if (field != null && field.ToString() != "")
            {
                builder.Append(description).AppendLine().Append(field);
            }
            if (field2 != null && field2.ToString() != "")
            {
                builder.Append(" (").Append(field2).Append(")");
            }
            builder.AppendLine();
        }

        public void AddDataTimeToStringBuilder(StringBuilder builder, string description, DateTime? field)
        {
            if (field != null)
            {
                builder.Append(description).AppendLine().Append(field.Value.ToString("HH:mm dd.MM.yyyy"));
            }
        }

        public void AddDateTimeToStringBuilder(StringBuilder builder, string description, DateTime? field, DateTime? field2)
        {
            if (field != null)
            {
                builder.Append(description).AppendLine().Append(field.Value.ToString("HH:mm dd.MM.yyyy"));
            }
            if (field2 != null)
            {
                builder.Append(" - ").Append(field2.Value.ToString("HH:mm dd.MM.yyyy"));
            }
            builder.AppendLine();
        }

        public void AddUrlToStringBuilder(StringBuilder builder, string description, string linkName, string url)
        {
            if (url != null && url.ToString() != "")
            {
                builder.Append(description).AppendLine().Append("[").Append(linkName).Append("](").Append(url).Append(")").AppendLine();
            }
        }

        public string MakeWorldEventDescription(EventObject eventObject, List<CardObject> eventCards = null)
        {
            StringBuilder eventDescription = new StringBuilder();
            AddTitledLineToStringBuilder(eventDescription, ":name_badge: **Nazwa** ", eventObject.English_name);
            AddDateTimeToStringBuilder(eventDescription, ":clock2: **Czas trwania** ", ConvertToPolandTimeFromUtc(eventObject.English_beginning), ConvertToPolandTimeFromUtc(eventObject.English_end));
            AddTitledLineToStringBuilder(eventDescription, ":timer: **Pozostały czas** ", GetTimeToEventEnd(ConvertToPolandTimeFromUtc(eventObject.English_end)));
            AddDateTimeToStringBuilder(eventDescription, ":clock12: **Czas trwania (UTC)** ", eventObject.English_beginning, eventObject.English_end);
            AddUrlToStringBuilder(eventDescription, ":globe_with_meridians: **URL** ", "schoolido.lu", eventObject.Website_url);
            AddTitledLineToStringBuilder(eventDescription, ":notepad_spiral: **Dodatkowe informacje** ", eventObject.Note);

            if (eventCards != null)
            {
                AddLineToStringBuilder(eventDescription, ":microphone: **Karty** ", eventCards.Count);
                foreach (CardObject eventCard in eventCards)
                {
                    AddLineToStringBuilder(eventDescription, eventCard.Idol.Name, eventCard.Id);
                }
                eventDescription.AppendLine().Append("*Możesz użyć komendy `karta <id>` aby uzyskać więcej informacji o danej karcie*");
            }

            return eventDescription.ToString();
        }

        public string MakeJapanEventDescription(EventObject eventObject, List<CardObject> eventCards = null)
        {
            StringBuilder eventDescription = new StringBuilder();
            AddTitledLineToStringBuilder(eventDescription, ":name_badge: **Nazwa** ", eventObject.Japanese_name, eventObject.Romaji_name);
            AddDateTimeToStringBuilder(eventDescription, ":clock2: **Czas trwania** ", eventObject.Beginning, eventObject.End);
            AddTitledLineToStringBuilder(eventDescription, ":timer: **Pozostały czas** ", GetTimeToEventEnd(eventObject.End));
            AddDateTimeToStringBuilder(eventDescription, ":clock9: **Czas trwania (JST)** ", ConvertToJapanTimeFromPoland(eventObject.Beginning), ConvertToJapanTimeFromPoland(eventObject.End));
            AddUrlToStringBuilder(eventDescription, ":globe_with_meridians: **URL** ", "schoolido.lu", eventObject.Website_url);
            AddTitledLineToStringBuilder(eventDescription, ":notepad_spiral: **Dodatkowe informacje** ", eventObject.Note);

            if (eventCards != null)
            {
                AddLineToStringBuilder(eventDescription, ":microphone: **Karty** ", eventCards.Count);
                foreach (CardObject eventCard in eventCards)
                {
                    AddLineToStringBuilder(eventDescription, eventCard.Idol.Name, eventCard.Id);
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
