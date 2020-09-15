﻿using DSharpPlus.Entities;
using MiraiZuraBot.Containers.Schoolidolu;
using MiraiZuraBot.Containers.Schoolidolu.Cards;
using MiraiZuraBot.Containers.Schoolidolu.Event;
using MiraiZuraBot.Containers.Schoolidolu.Idols;
using MiraiZuraBot.Containers.Schoolidolu.Songs;
using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MiraiZuraBot.Helpers.SchoolidoluHelper
{
    class SchoolidoluHelper
    {
        private Translator tr;

        public SchoolidoluHelper(Translator translator)
        {
            tr = translator;
        }

        public static DiscordEmbedBuilder.EmbedFooter GetSchoolidoluFotter()
        {
            return new DiscordEmbedBuilder.EmbedFooter { Text = "Powered by schoolido.lu", IconUrl = "https://i.schoolido.lu/android/icon.png" };
        }

        public string MakeCardDescription(Translator.Language lang, CardObject cardObject, bool isIdolised)
        {
            StringBuilder cardDescription = new StringBuilder();
            cardDescription.AppendFormat(":name_badge: **{0}** ({1})", tr.GetString(lang, "cardCharacter"), cardObject.Idol.Name).AppendLine();

            cardDescription.Append(":pencil: ").AppendFormat("**ID** ({0})", cardObject.Id).AppendLine();

            cardDescription.Append(":love_letter: ").AppendFormat("**Rzadkość** ({0})", cardObject.Rarity).AppendLine();

            cardDescription.Append(GetEmojiForAttribute(cardObject.Attribute)).Append(" ").AppendFormat("**Atrybut** ({0})", cardObject.Attribute).AppendLine();

            cardDescription.Append(":dress: ").AppendFormat("**Set** ({0})", cardObject.Translated_collection ?? "brak").AppendLine();

            cardDescription.Append(":calendar: ").AppendFormat("**Data wypuszczenia (yyyy-MM-dd)** ({0})", cardObject.Release_date ?? "brak").AppendLine();   
            
            cardDescription.Append(GetEmojiAvailability(cardObject.Japan_only)).Append(" ")
                .AppendFormat("** Dostępność ** ({0})", cardObject.Japan_only.Value ? "Dostępne tylko na JP" : "Dostępne na JP i EN" ?? "brak").AppendLine();

            cardDescription.Append(":heart: ").AppendFormat("**HP** ({0})", cardObject.Hp?.ToString() ?? "brak").AppendLine();

            cardDescription.Append(":dizzy: ").AppendFormat("**Skill** ({0})", cardObject.Skill ?? "brak").AppendLine();

            cardDescription.Append(cardObject.Skill_details ?? "brak").AppendLine();

            cardDescription.Append(":sparkles: ").AppendFormat("**Center skill** ({0})", cardObject.Center_skill ?? "brak").AppendLine();

            cardDescription.Append(cardObject.Center_skill_details ?? "brak").AppendLine();
            
            cardDescription.Append(":globe_with_meridians: ").AppendFormat("**URL** [{0}]({1})", "schoolido.lu", cardObject.Website_url).AppendLine();

            cardDescription.Append(":notepad_spiral: **Statystyki** ").AppendLine();
            cardDescription.Append(":red_circle: ").AppendFormat("Smile: {0} - {1} - {2}",
                cardObject.Minimum_statistics_smile ?? "brak",
                cardObject.Non_idolized_maximum_statistics_smile ?? "brak",
                cardObject.Idolized_maximum_statistics_smile ?? "brak").AppendLine();
            cardDescription.Append(":green_circle: ").AppendFormat("Pure: {0} - {1} - {2}",
                cardObject.Minimum_statistics_pure ?? "brak",
                cardObject.Non_idolized_maximum_statistics_pure ?? "brak",
                cardObject.Idolized_maximum_statistics_pure ?? "brak").AppendLine();
            cardDescription.Append(":blue_circle: ").AppendFormat("Cool: {0} - {1} - {2}",
                cardObject.Minimum_statistics_cool ?? "brak",
                cardObject.Non_idolized_maximum_statistics_cool ?? "brak",
                cardObject.Idolized_maximum_statistics_cool ?? "brak").AppendLine();

            

            if(cardObject.Rarity == "UR")
            {
                cardDescription.Append(":handshake: ").Append("**Para** ").AppendLine();
                {
                    if(cardObject.Ur_pair != null)
                    {
                        cardDescription.AppendFormat("{0} ({1})", cardObject.Ur_pair.Card?.Name ?? "brak", cardObject.Ur_pair.Card?.Id ?? "brak").AppendLine();
                        if(cardObject.Ur_pair.Card != null && cardObject.Ur_pair.Card.Id != null)
                        {
                            cardDescription.AppendFormat("*Możesz zobaczyć tę kartę komendą `karta {0}`.*", cardObject.Ur_pair.Card.Id).AppendLine();
                        }
                    }
                    else
                    {
                        cardDescription.Append("Karta nie ma pary.").AppendLine();
                    }
                }
            }

            cardDescription.Append(":stadium: ").Append("**Event**").Append(" ").AppendLine();
            if (cardObject.Event == null)
            {
                cardDescription.Append("Karta nie pochodzi z eventu.").AppendLine();
            }
            else
            {
                if(cardObject.Other_event == null)
                {
                    cardDescription.Append(":name_badge: ").Append("**Event JP**").AppendLine();
                    cardDescription.Append(cardObject.Event.Japanese_name).AppendLine();
                    cardDescription.Append(":name_badge: ").Append("**Event EN** ").AppendLine();
                    cardDescription.Append(cardObject.Event.English_name ?? "brak").AppendLine();
                }
                else
                {
                    cardDescription.Append("Karta na EN była w innym evencie niż na JP.").AppendLine();
                    cardDescription.Append(":name_badge: ").Append("**Event JP**").AppendLine();
                    cardDescription.Append(cardObject.Event.Japanese_name).AppendLine();
                    cardDescription.Append(":name_badge: ").Append("**Event EN** ").AppendLine();
                    cardDescription.Append(cardObject.Other_event.English_name ?? "brak").AppendLine();
                }
            }

            return cardDescription.ToString();
        }

        public string MakeIdolDescription(IdolObject idolObject)
        {
            StringBuilder idolDescription = new StringBuilder();
            idolDescription.Append(":name_badge: ").AppendFormat("**Imie** ({0} ({1}))", idolObject.Name, idolObject.Japanese_name ?? "brak kanji").AppendLine();

            idolDescription.Append(":school: ").AppendFormat("**Szkoła** ({0})", idolObject.School ?? "brak").AppendLine();

            idolDescription.Append(":microphone: ").AppendFormat("**Main unit** ({0})", idolObject.Main_unit ?? "brak").AppendLine();

            idolDescription.Append(":notes: ").AppendFormat("**Sub unit** ({0})", idolObject.Sub_unit ?? "brak").AppendLine();

            idolDescription.Append(GetEmojiForYear(idolObject.Year)).Append(" ").AppendFormat("**Rok** ({0})", idolObject.Year ?? "brak").AppendLine();

            idolDescription.Append(":notes: ").AppendFormat("**Sub unit** ({0})", idolObject.Sub_unit ?? "brak").AppendLine();

            idolDescription.Append(":calendar: ").AppendFormat("**Wiek** ({0})", idolObject.Age?.ToString() ?? "brak").AppendLine();

            idolDescription.Append(":birthday: ").AppendFormat("**Urodziny (MM-dd)** ({0})", idolObject.Birthday ?? "brak").AppendLine();

            idolDescription.Append(GetEmojiForZodiacSign(idolObject.Astrological_sign)).Append(" ").AppendFormat("**Znak zodiaku** ({0})", idolObject.Astrological_sign ?? "brak").AppendLine();

            idolDescription.Append(GetEmojiForBloodType(idolObject.Blood)).Append(" ").AppendFormat("**Grupa krwi** ({0})", idolObject.Blood ?? "brak").AppendLine();

            idolDescription.Append(":straight_ruler: ").AppendFormat("**Wzrost** ({0})", idolObject.Height?.ToString() ?? "brak").AppendLine();

            idolDescription.Append(GetEmojiForAttribute(idolObject.Attribute)).Append(" ").AppendFormat("**Atrybut** ({0})", idolObject.Attribute ?? "brak").AppendLine();
            
            idolDescription.Append(":ramen: ").Append("**Ulubione jedzenie**").Append(" ").AppendLine();
            idolDescription.Append(idolObject.Favorite_food ?? "brak").AppendLine();
            idolDescription.Append(":broccoli: ").Append("**Nielubiane jedzenie**").Append(" ").AppendLine();
            idolDescription.Append(idolObject.Least_favorite_food ?? "brak").AppendLine();
            idolDescription.Append(":ping_pong: ").Append("**Hobby**").Append(" ").AppendLine();
            idolDescription.Append(idolObject.Hobbies ?? "brak").AppendLine();
            
            idolDescription.Append(":microphone2: **Seiyuu** ").AppendLine();
            idolDescription.Append(idolObject.Cv?.Name ?? "brak").AppendLine();
            idolDescription.Append(":globe_with_meridians: **URL** ").AppendLine().Append("[").Append("schoolido.lu").Append("](").Append(idolObject.Website_url).Append(")");

            if (idolObject.Wiki_url != null && idolObject.Wiki_url != "")
            {
                idolDescription.Append(" [").Append("wiki").Append("](").Append(idolObject.Wiki_url.Replace("%20", "_")).Append(")");
            }
            if (idolObject.Wikia_url != null && idolObject.Wikia_url != "")
            {
                idolDescription.Append(" [").Append("wikia").Append("](").Append(idolObject.Wikia_url).Append(")");
            }
            if (idolObject.Official_url != null && idolObject.Official_url != "")
            {
                idolDescription.Append(" [").Append("lovelive-anime.jp").Append("](").Append(idolObject.Official_url).Append(")");
            }

            idolDescription.AppendLine();

            return idolDescription.ToString();
        }

        public string MakeCurrentWorldEventDescription(EventObject eventObject, bool finished, List<CardObject> eventCards = null)
        {
            StringBuilder eventDescription = new StringBuilder();
            eventDescription.Append(":name_badge: **Nazwa** ").AppendLine();
            eventDescription.Append(eventObject.English_name ?? "brak").AppendLine();
            eventDescription.Append(":name_badge: **Japońska nazwa** ").AppendLine();
            eventDescription.Append(eventObject.Japanese_name).AppendLine();
            eventDescription.Append(":clock2: **Czas trwania** ").AppendLine();
            eventDescription.Append(ConvertToPolandTimeFromUtc(eventObject.English_beginning)?.ToString("HH:mm dd.MM.yyyy") ?? "brak daty rozpoczęcia").Append(" - ")
                            .Append(ConvertToPolandTimeFromUtc(eventObject.English_end)?.ToString("HH:mm dd.MM.yyyy") ?? "brak daty zakończenia").AppendLine();
            if (finished == false)
            {
                eventDescription.Append(":timer: **Pozostały czas** ").AppendLine();
                eventDescription.Append(GetTimeToEventEnd(ConvertToPolandTimeFromUtc(eventObject.English_end)) ?? "nie można obliczyć").AppendLine();
            }
            eventDescription.Append(":clock9: **Czas trwania (UTC)** ").AppendLine();
            eventDescription.Append(eventObject.English_beginning?.ToString("HH:mm dd.MM.yyyy") ?? "Brak daty rozpoczęcia").Append(" - ")
                            .Append(eventObject.English_end?.ToString("HH:mm dd.MM.yyyy") ?? "Brak daty zakończenia").AppendLine();
            eventDescription.Append(":globe_with_meridians: **URL** ").AppendLine().Append("[").Append("schoolido.lu").Append("](").Append(eventObject.Website_url).Append(")").AppendLine();
            eventDescription.Append(":notepad_spiral: **Dodatkowe informacje** ").AppendLine();
            eventDescription.Append(eventObject.Note ?? "brak").AppendLine();

            if (finished == true)
            {
                eventDescription.Append(":medal: **Tiery** (punkty)").AppendLine();
                eventDescription.Append(":first_place: **T1** ").Append(eventObject.English_t1_points?.ToString() ?? "brak").AppendLine();
                eventDescription.Append(":second_place: **T2** ").Append(eventObject.English_t2_points?.ToString() ?? "brak").AppendLine();
            }

            if (eventCards != null)
            {
                eventDescription.Append(":microphone: **Karty** ").Append(" (").Append(eventCards.Count).Append(")").AppendLine();
                foreach (CardObject eventCard in eventCards)
                {
                    eventDescription.Append(eventCard.Idol.Name).Append(" (").Append(eventCard.Id).Append(")").AppendLine();
                }
                if (eventCards.Count == 0)
                {
                    eventDescription.Append("Obecnie brak o kartach").AppendLine();
                }
                eventDescription.AppendLine().Append("*Możesz użyć komendy `karta <id>` aby uzyskać więcej informacji o danej karcie*");
            }
            else
            {
                eventDescription.Append(":microphone: **Karty** ").Append(" (").Append("brak").Append(")").AppendLine();
            }

            return eventDescription.ToString();
        }

        public string MakeCurrentJapanEventDescription(EventObject eventObject, bool finished, List<CardObject> eventCards = null)
        {
            StringBuilder eventDescription = new StringBuilder();
            eventDescription.Append(":name_badge: **Nazwa** ").AppendLine();
            eventDescription.Append(eventObject.Japanese_name).Append(" (").Append(eventObject.Romaji_name ?? "brak romaji").Append(")").AppendLine();
            eventDescription.Append(":clock2: **Czas trwania** ").AppendLine();
            eventDescription.Append(eventObject.Beginning?.ToString("HH:mm dd.MM.yyyy") ?? "brak daty rozpoczęcia").Append(" - ").Append(eventObject.End?.ToString("HH:mm dd.MM.yyyy") ?? "brak daty zakończenia").AppendLine();
            if (finished == false)
            {
                eventDescription.Append(":timer: **Pozostały czas** ").AppendLine();
                eventDescription.Append(GetTimeToEventEnd(eventObject.End) ?? "nie można obliczyć").AppendLine();
            }
            eventDescription.Append(":clock9: **Czas trwania (JST)** ").AppendLine();
            eventDescription.Append(ConvertToJapanTimeFromPoland(eventObject.Beginning)?.ToString("HH:mm dd.MM.yyyy") ?? "Brak daty rozpoczęcia").Append(" - ")
                            .Append(ConvertToJapanTimeFromPoland(eventObject.End)?.ToString("HH:mm dd.MM.yyyy") ?? "Brak daty zakończenia").AppendLine();
            eventDescription.Append(":globe_with_meridians: **URL** ").AppendLine().Append("[").Append("schoolido.lu").Append("](").Append(eventObject.Website_url).Append(")").AppendLine();
            eventDescription.Append(":notepad_spiral: **Dodatkowe informacje** ").AppendLine();
            eventDescription.Append(eventObject.Note ?? "brak").AppendLine();

            if (finished == true)
            {
                eventDescription.Append(":medal: **Tiery** ").AppendLine();
                eventDescription.Append(":first_place: **T1** ").Append(eventObject.Japanese_t1_points?.ToString() ?? "brak").AppendLine();
                eventDescription.Append(":second_place: **T2** ").Append(eventObject.Japanese_t2_points?.ToString() ?? "brak").AppendLine();
            }

            if (eventCards != null)
            {
                eventDescription.Append(":microphone: **Karty** ").Append(" (").Append(eventCards.Count).Append(")").AppendLine();
                foreach (CardObject eventCard in eventCards)
                {
                    eventDescription.Append(eventCard.Idol.Name).Append(" (").Append(eventCard.Id).Append(")").AppendLine();
                }
                if(eventCards.Count == 0)
                {
                    eventDescription.Append("Obecnie brak o kartach").AppendLine();
                }
                eventDescription.AppendLine().Append("*Możesz użyć komendy `karta <id>` aby uzyskać więcej informacji o danej karcie*");
            }
            else
            {
                eventDescription.Append(":microphone: **Karty** ").Append(" (").Append("brak").Append(")").AppendLine();
            }

            return eventDescription.ToString();
        }

        public string MakeSongDescription(SongObject songObject, EventObject eventObject = null)
        {
            StringBuilder songDescription = new StringBuilder();
            songDescription.Append(":name_badge: **Tytuł**").Append(" (").Append(songObject.Name);
            if (songObject.Romaji_name != null)
            {
                songDescription.Append(" (").Append(songObject.Romaji_name).Append(")");
            }
            songDescription.Append(")").AppendLine();
            songDescription.Append(":microphone: **Main unit**").Append(" (").Append(songObject.Main_unit ?? "brak").Append(")").AppendLine();
            songDescription.Append(GetEmojiForAttribute(songObject.Attribute)).Append(" **Atrybut**").Append(" (").Append(songObject.Attribute ?? "brak").Append(")").AppendLine();
            songDescription.Append(":watch: **Czas**").Append(" (").Append(songObject.Time.ToString() ?? "brak").Append(")").AppendLine();
            songDescription.Append(":stopwatch: **BPM**").Append(" (").Append(songObject.Bpm.ToString() ?? "brak").Append(")").AppendLine();
            songDescription.Append(":stadium: **Event** ").AppendLine();
            if (eventObject == null)
            {
                songDescription.Append("Piosenka nie była używana w evencie.").AppendLine();
            }
            else
            {
                songDescription.Append("Piosenka  używana w evencie: ").Append(eventObject.Japanese_name);
                if (eventObject.English_name != null)
                {
                    songDescription.Append(" (").Append(eventObject.English_name).Append(")");
                }
                songDescription.AppendLine().Append("*Możesz użyć komendy `eventJP "+ eventObject.Japanese_name + "` lub `eventEN " + eventObject.Japanese_name + "` aby uzyskać więcej informacji o danym evencie.*").AppendLine();
            }

            songDescription.Append(":notepad_spiral: **Statystyki** ").AppendLine();
            songDescription.Append("Easy - :star: Trudność ").Append(songObject.Easy_difficulty.ToString() ?? "brak")
                           .Append(" - :musical_note: Nutki ").Append(songObject.Easy_notes.ToString() ?? "brak").AppendLine();
            songDescription.Append("Normal - :star: Trudność ").Append(songObject.Normal_difficulty.ToString() ?? "brak")
                           .Append(" - :musical_note: Nutki ").Append(songObject.Normal_notes.ToString() ?? "brak").AppendLine();
            songDescription.Append("Hard - :star: Trudność ").Append(songObject.Hard_difficulty.ToString() ?? "brak")
                           .Append(" - :musical_note: Nutki ").Append(songObject.Hard_notes.ToString() ?? "brak").AppendLine();
            songDescription.Append("Expert - :star: Trudność ").Append(songObject.Expert_difficulty.ToString() ?? "brak")
                           .Append(" - :musical_note: Nutki ").Append(songObject.Expert_notes.ToString() ?? "brak").AppendLine();
            songDescription.Append("Master - :star: Trudność ").Append(songObject.Master_difficulty.ToString() ?? "brak")
                           .Append(" - :musical_note: Nutki ").Append(songObject.Master_notes.ToString() ?? "brak").AppendLine();

            songDescription.Append(":globe_with_meridians: **URL** ").AppendLine().Append("[").Append("schoolido.lu").Append("](").Append(songObject.Website_url).Append(")");

            songDescription.AppendLine();

            return songDescription.ToString();
        }

        public string MakeSearchCardDescription(PaginatedResponse<CardObject> cardObjects, int elemPerPage, int page)
        {
            StringBuilder eventDescription = new StringBuilder();
            eventDescription.Append(":notepad_spiral: **Wyników ").Append(cardObjects.Count).Append(". Oto ").Append(cardObjects.Results.Count).Append("**").AppendLine();
            eventDescription.Append(":name_badge: ").Append("Imie").Append(" - :pencil: ").Append("ID").Append(" - :love_letter: ").Append("Rzadkość").
                Append(" - ").Append(GetEmojiForAttribute(null)).Append(" ").Append("Atrybut").AppendLine();
            eventDescription.Append("---------------------------------------------------------").AppendLine();
            foreach (var cardObject in cardObjects.Results)
            {
                eventDescription.Append(":name_badge: ").Append(cardObject.Idol.Name).Append(" - :pencil: ").Append(cardObject.Id).Append(" - :love_letter: ").Append(cardObject.Rarity).
                 Append(" - ").Append(GetEmojiForAttribute(cardObject.Attribute)).Append(" ").Append(cardObject.Attribute).AppendLine();
            }
            eventDescription.Append("---------------------------------------------------------").AppendLine();
            eventDescription.Append("Strona ").Append(page).Append(" z ").Append(cardObjects.Count / elemPerPage + 1);

            return eventDescription.ToString();
        }

        public string MakeSearchEventDescription(PaginatedResponse<EventObject> eventObjects, int elemPerPage, int page)
        {
            StringBuilder eventDescription = new StringBuilder();
            eventDescription.Append(":notepad_spiral: **Wyników ").Append(eventObjects.Count).Append(". Oto ").Append(eventObjects.Results.Count).Append("**").AppendLine();
            eventDescription.Append(":japan: ").Append("Nazwa z serwera JP").Append(" - :earth_africa: ").Append("Nazwa z serwera EN").AppendLine();
            eventDescription.Append("---------------------------------------------------------").AppendLine();
            foreach (var eventObject in eventObjects.Results)
            {
                eventDescription.Append(":japan: ").Append(eventObject.Japanese_name).Append(" - :earth_africa: ").Append(eventObject.English_name ?? "brak angielskiej nazwy").AppendLine();
            }
            eventDescription.Append("---------------------------------------------------------").AppendLine();
            eventDescription.Append("Strona ").Append(page).Append(" z ").Append(eventObjects.Count / elemPerPage + 1);

            return eventDescription.ToString();
        }

        public string MakeSearchIdolDescription(PaginatedResponse<IdolObject> idolObjects, int elemPerPage, int page)
        {
            StringBuilder eventDescription = new StringBuilder();
            eventDescription.Append(":notepad_spiral: **Wyników ").Append(idolObjects.Count).Append(". Oto ").Append(idolObjects.Results.Count).Append("**").AppendLine();
            eventDescription.Append(":name_badge: ").Append("Imie").Append(" - :microphone: ").Append("Main unit").Append(" - :school: ").Append("Szkoła").
                Append(" - ").Append(GetEmojiForAttribute(null)).Append(" ").Append("Atrybut").AppendLine();
            eventDescription.Append("---------------------------------------------------------").AppendLine();
            foreach (var idolObject in idolObjects.Results)
            {
                eventDescription.Append(":name_badge: ").Append(idolObject.Name).Append(" - :microphone: ").Append(idolObject.Main_unit ?? "brak").Append(" - :school: ").Append(idolObject.School ?? "brak").
                 Append(" - ").Append(GetEmojiForAttribute(idolObject.Attribute)).Append(" ").Append(idolObject.Attribute ?? "brak").AppendLine();
            }
            eventDescription.Append("---------------------------------------------------------").AppendLine();
            eventDescription.Append("Strona ").Append(page).Append(" z ").Append(idolObjects.Count / elemPerPage + 1);

            return eventDescription.ToString();
        }

        public string MakeSearchSongDescription(PaginatedResponse<SongObject> songObjects, int elemPerPage, int page)
        {
            StringBuilder songDescription = new StringBuilder();
            songDescription.Append(":notepad_spiral: **Wyników ").Append(songObjects.Count).Append(". Oto ").Append(songObjects.Results.Count).Append("**").AppendLine();
            songDescription.Append(":name_badge: ").Append("Japońska nazwa").Append(" - :name_badge: ").Append("Romaji").Append(" - :microphone: ").Append("Main unit").
                Append(" - ").Append(GetEmojiForAttribute(null)).Append(" ").Append("Atrybut").AppendLine();
            songDescription.Append("---------------------------------------------------------").AppendLine();
            foreach (var songObject in songObjects.Results)
            {
                songDescription.Append(":name_badge: ").Append(songObject.Name).Append(" - :name_badge: ").Append(songObject.Romaji_name ?? "brak").Append(" - :microphone: ").Append(songObject.Main_unit ?? "brak").
                 Append(" - ").Append(GetEmojiForAttribute(songObject.Attribute)).Append(" ").Append(songObject.Attribute ?? "brak").AppendLine();
            }
            songDescription.Append("---------------------------------------------------------").AppendLine();
            songDescription.Append("Strona ").Append(page).Append(" z ").Append(songObjects.Count / elemPerPage + 1);

            return songDescription.ToString();
        }

        private string GetEmojiAvailability(bool? value)
        {
            switch (value)
            {
                case true:
                    return ":japan:";
                case false:
                    return ":earth_africa:";
                default:
                    return ":flag_white:";
            }
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
                    return ":scorpius:";
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

        public string GetColorForAttribute(string attribute)
        {
            switch (attribute)
            {
                case "Smile":
                    return "#e91a8b";
                case "Pure":
                    return "#00bb44";
                case "Cool":
                    return "#03bbfe";
                case "All":
                    return "#c958f6";
                default:
                    return "#ffffff";
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
