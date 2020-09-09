using DSharpPlus.Entities;
using MiraiZuraBot.Containers.Schoolidolu.Cards;
using MiraiZuraBot.Containers.Schoolidolu.Event;
using MiraiZuraBot.Containers.Schoolidolu.Idols;
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

        public string MakeCardDescription(CardObject cardObject, bool isIdolised)
        {
            StringBuilder cardDescription = new StringBuilder();
            cardDescription.Append(":name_badge: **Postać** ").AppendLine();
            cardDescription.Append(cardObject.Idol.Name).AppendLine();
            cardDescription.Append(":pencil: **ID** ").AppendLine();
            cardDescription.Append(cardObject.Id).AppendLine();
            cardDescription.Append(":love_letter: **Rzadkość** ").AppendLine();
            cardDescription.Append(cardObject.Rarity).AppendLine();
            cardDescription.Append(GetEmojiForAttribute(cardObject.Attribute)).Append(" **Atrybut** ").AppendLine();
            cardDescription.Append(cardObject.Attribute).AppendLine();
            cardDescription.Append(":dress: **Set** ").AppendLine();
            cardDescription.Append(cardObject.Translated_collection ?? "brak danych").AppendLine();
            cardDescription.Append(":calendar: **Data wypuszczenia (yyyy-MM-dd)** ").AppendLine();
            cardDescription.Append(cardObject.Release_date ?? "brak danych").AppendLine();
            cardDescription.Append(":dizzy: **Skill** ").Append("(").Append(cardObject.Skill ?? "brak danych").Append(")").AppendLine();
            cardDescription.Append(cardObject.Skill_details ?? "brak danych").AppendLine();
            cardDescription.Append(":sparkles: **Center skill** ").Append("(").Append(cardObject.Center_skill ?? "brak danych").Append(")").AppendLine();
            cardDescription.Append(cardObject.Center_skill_details ?? "brak danych").AppendLine();
            cardDescription.Append(":heart: **HP** ").AppendLine();
            cardDescription.Append(cardObject.Hp?.ToString() ?? "brak danych").AppendLine();
            AddUrlToStringBuilder(cardDescription, ":globe_with_meridians: **URL** ", "schoolido.lu", cardObject.Website_url);

            cardDescription.Append(":notepad_spiral: **Statystyki** ").AppendLine();
            cardDescription.Append(":red_circle: Smile: ").Append(cardObject.Minimum_statistics_smile ?? "brak danych")
                           .Append(" - ").Append(cardObject.Non_idolized_maximum_statistics_smile ?? "brak danych")
                           .Append(" - ").Append(cardObject.Idolized_maximum_statistics_smile ?? "brak danych").AppendLine();
            cardDescription.Append(":green_circle: Pure: ").Append(cardObject.Minimum_statistics_pure ?? "brak danych")
                           .Append(" - ").Append(cardObject.Non_idolized_maximum_statistics_pure ?? "brak danych")
                           .Append(" - ").Append(cardObject.Idolized_maximum_statistics_pure ?? "brak danych").AppendLine();
            cardDescription.Append(":blue_circle: Cool: ").Append(cardObject.Minimum_statistics_cool ?? "brak danych")
                           .Append(" - ").Append(cardObject.Non_idolized_maximum_statistics_cool ?? "brak danych")
                           .Append(" - ").Append(cardObject.Idolized_maximum_statistics_cool ?? "brak danych").AppendLine();

            return cardDescription.ToString();
        }

        public string MakeIdolDescription(IdolObject idolObject)
        {
            StringBuilder idolDescription = new StringBuilder();
            idolDescription.Append(":name_badge: **Imie** ").AppendLine();
            idolDescription.Append(idolObject.Name ?? "brak danych").Append(" (").Append(idolObject.Japanese_name ?? "brak kanji").Append(")").AppendLine();
            idolDescription.Append(":school: **Szkoła** ").AppendLine();
            idolDescription.Append(idolObject.School ?? "brak danych").AppendLine();
            idolDescription.Append(":microphone: **Main unit** ").AppendLine();
            idolDescription.Append(idolObject.Main_unit ?? "brak danych").AppendLine();
            idolDescription.Append(":notes: **Sub unit** ").AppendLine();
            idolDescription.Append(idolObject.Sub_unit ?? "brak danych").AppendLine();
            idolDescription.Append(GetEmojiForYear(idolObject.Year)).Append(" **Rok** ").AppendLine();
            idolDescription.Append(idolObject.Year ?? "brak danych").AppendLine();
            idolDescription.Append(":calendar: **Wiek** ").AppendLine();
            idolDescription.Append(idolObject.Age?.ToString() ?? "brak danych").AppendLine();
            idolDescription.Append(":birthday: **Urodziny (MM-dd)** ").AppendLine();
            idolDescription.Append(idolObject.Birthday ?? "brak danych").AppendLine();
            idolDescription.Append(GetEmojiForZodiacSign(idolObject.Astrological_sign)).Append(" **Znak zodiaku** ").AppendLine();
            idolDescription.Append(idolObject.Astrological_sign ?? "brak danych").AppendLine();
            idolDescription.Append(GetEmojiForBloodType(idolObject.Blood)).Append(" **Grupa krwi** ").AppendLine();
            idolDescription.Append(idolObject.Blood ?? "brak danych").AppendLine();
            idolDescription.Append(":straight_ruler: **Wzrost** ").AppendLine();
            idolDescription.Append(idolObject.Height?.ToString() ?? "brak danych").AppendLine();
            idolDescription.Append(":ramen: **Ulubione jedzenie** ").AppendLine();
            idolDescription.Append(idolObject.Favorite_food ?? "brak danych").AppendLine();
            idolDescription.Append(":broccoli: **Nielubiane jedzenie** ").AppendLine();
            idolDescription.Append(idolObject.Least_favorite_food ?? "brak danych").AppendLine();
            idolDescription.Append(":ping_pong: **Hobby** ").AppendLine();
            idolDescription.Append(idolObject.Hobbies ?? "brak danych").AppendLine();
            idolDescription.Append(GetEmojiForAttribute(idolObject.Attribute)).Append(" **Atrybut** ").AppendLine();
            idolDescription.Append(idolObject.Attribute ?? "brak danych").AppendLine();
            idolDescription.Append(":microphone2: **Saiyuu** ").AppendLine();
            idolDescription.Append(idolObject.Cv?.Name ?? "brak danych").AppendLine();

            return idolDescription.ToString();
        }

        public string MakeCurrentWorldEventDescription(EventObject eventObject, List<CardObject> eventCards = null)
        {
            StringBuilder eventDescription = new StringBuilder();
            eventDescription.Append(":name_badge: **Nazwa** ").AppendLine();
            eventDescription.Append(eventObject.English_name ?? "brak danych").AppendLine();
            eventDescription.Append(":clock2: **Czas trwania** ").AppendLine();
            eventDescription.Append(ConvertToPolandTimeFromUtc(eventObject.English_beginning)?.ToString("HH:mm dd.MM.yyyy") ?? "brak daty rozpoczęcia").Append("-")
                            .Append(ConvertToPolandTimeFromUtc(eventObject.English_end)?.ToString("HH:mm dd.MM.yyyy") ?? "brak daty zakończenia").AppendLine();
            eventDescription.Append(":timer: **Pozostały czas** ").AppendLine();
            eventDescription.Append(GetTimeToEventEnd(ConvertToPolandTimeFromUtc(eventObject.English_end)) ?? "nie można obliczyć").AppendLine();
            eventDescription.Append(":clock9: **Czas trwania (UTC)** ").AppendLine();
            eventDescription.Append(eventObject.English_beginning?.ToString("HH:mm dd.MM.yyyy") ?? "Brak daty rozpoczęcia").Append("-")
                            .Append(eventObject.English_end?.ToString("HH:mm dd.MM.yyyy") ?? "Brak daty zakończenia").AppendLine();
            AddUrlToStringBuilder(eventDescription, ":globe_with_meridians: **URL** ", "schoolido.lu", eventObject.Website_url);
            eventDescription.Append(":notepad_spiral: **Dodatkowe informacje** ").AppendLine();
            eventDescription.Append(eventObject.Note ?? "brak").AppendLine();

            if (eventCards != null)
            {
                eventDescription.Append(":microphone: **Karty** ").Append(" (").Append(eventCards.Count).Append(")").AppendLine();
                foreach (CardObject eventCard in eventCards)
                {
                    eventDescription.Append(eventCard.Idol.Name).Append(" (").Append(eventCard.Id).Append(")").AppendLine();
                }
                if (eventCards.Count == 0)
                {
                    eventDescription.Append("Obecnie brak danych o kartach").AppendLine();
                }
                eventDescription.AppendLine().Append("*Możesz użyć komendy `karta <id>` aby uzyskać więcej informacji o danej karcie*");
            }
            else
            {
                eventDescription.Append(":microphone: **Karty** ").Append(" (").Append("brak danych").Append(")").AppendLine();
            }

            return eventDescription.ToString();
        }

        public string MakeCurrentJapanEventDescription(EventObject eventObject, List<CardObject> eventCards = null)
        {
            StringBuilder eventDescription = new StringBuilder();
            eventDescription.Append(":name_badge: **Nazwa** ").AppendLine();
            eventDescription.Append(eventObject.Japanese_name ?? "brak danych").Append(" (").Append(eventObject.Romaji_name ?? "brak romaji").Append(")").AppendLine();
            eventDescription.Append(":clock2: **Czas trwania** ").AppendLine();
            eventDescription.Append(eventObject.Beginning?.ToString("HH:mm dd.MM.yyyy") ?? "brak daty rozpoczęcia").Append("-").Append(eventObject.End?.ToString("HH:mm dd.MM.yyyy") ?? "brak daty zakończenia").AppendLine();
            eventDescription.Append(":timer: **Pozostały czas** ").AppendLine();
            eventDescription.Append(GetTimeToEventEnd(eventObject.End) ?? "nie można obliczyć").AppendLine();
            eventDescription.Append(":clock9: **Czas trwania (JST)** ").AppendLine();
            eventDescription.Append(ConvertToJapanTimeFromPoland(eventObject.Beginning)?.ToString("HH:mm dd.MM.yyyy") ?? "Brak daty rozpoczęcia").Append("-")
                            .Append(ConvertToJapanTimeFromPoland(eventObject.End)?.ToString("HH:mm dd.MM.yyyy") ?? "Brak daty zakończenia").AppendLine();
            AddUrlToStringBuilder(eventDescription, ":globe_with_meridians: **URL** ", "schoolido.lu", eventObject.Website_url);
            eventDescription.Append(":notepad_spiral: **Dodatkowe informacje** ").AppendLine();
            eventDescription.Append(eventObject.Note ?? "brak").AppendLine();

            if (eventCards != null)
            {
                eventDescription.Append(":microphone: **Karty** ").Append(" (").Append(eventCards.Count).Append(")").AppendLine();
                foreach (CardObject eventCard in eventCards)
                {
                    eventDescription.Append(eventCard.Idol.Name).Append(" (").Append(eventCard.Id).Append(")").AppendLine();
                }
                if(eventCards.Count == 0)
                {
                    eventDescription.Append("Obecnie brak danych o kartach").AppendLine();
                }
                eventDescription.AppendLine().Append("*Możesz użyć komendy `karta <id>` aby uzyskać więcej informacji o danej karcie*");
            }
            else
            {
                eventDescription.Append(":microphone: **Karty** ").Append(" (").Append("brak danych").Append(")").AppendLine();
            }

            return eventDescription.ToString();
        }

        private string GetEmojiForYear(string year)
        {
            switch (year)
            {
                case "First":
                    return ":one:";
                case "Second":
                    return ":two:";
                case "Third":
                    return ":three:";
                default:
                    return ":1234:";
            }
        }

        private string GetEmojiForZodiacSign(string zodiac)
        {
            switch(zodiac)
            {
                case "Aries":
                    return ":aries:";
                case "Taurus":
                    return ":taurus:";
                case "Gemini":
                    return ":gemini:";
                case "Cancer":
                    return ":cancer:";
                case "Leo":
                    return ":leo:";
                case "Virgo":
                    return ":virgo:";
                case "Libra":
                    return ":libra:";
                case "Scorpio":
                    return ":scorpio:";
                case "Sagittarius":
                    return ":sagittarius:";
                case "Capricorn":
                    return ":capricorn:";
                case "Aquarius":
                    return ":aquarius:";
                case "Pisces":
                    return ":pisces:";
                default:
                    return ":milky_way:";
            }
        }

        private string GetEmojiForBloodType(string bloodType)
        {
            switch (bloodType)
            {
                case "A":
                    return ":a:";
                case "B":
                    return ":b:";
                case "AB":
                    return ":ab:";
                case "O":
                    return ":o2:";
                default:
                    return ":drop_of_blood:";
            }
        }
        
        private string GetEmojiForAttribute(string attribute)
        {
            switch (attribute)
            {
                case "Smile":
                    return ":red_circle:";
                case "Pure":
                    return ":green_circle:";
                case "Cool":
                    return ":blue_circle:";
                case "All":
                    return ":purple_circle:";
                default:
                    return ":white_circle:";
            }
        }

        public void AddUrlToStringBuilder(StringBuilder builder, string description, string linkName, string url)
        {
            if (url != null && url.ToString() != "")
            {
                builder.Append(description).AppendLine().Append("[").Append(linkName).Append("](").Append(url).Append(")").AppendLine();
            }
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
