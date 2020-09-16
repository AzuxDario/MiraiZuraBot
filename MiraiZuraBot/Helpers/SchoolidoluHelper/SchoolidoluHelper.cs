using DSharpPlus.Entities;
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
            cardDescription.AppendFormat(":name_badge: **{0}** ({1})\n" +
                ":pencil: **{2}** ({3})\n" +
                ":love_letter: **{4}** ({5})\n" +
                "{6} **{7}** ({8})\n" +
                ":dress: **{9}** ({10})\n" +
                ":calendar: **{11}** ({12})\n" +
                "{13} **{14}** ({15})\n" +
                ":heart: **{16}** ({17})\n" +
                ":dizzy: **{18}** ({19})\n" +
                "{20}\n" +
                ":sparkles: **{21}** ({22})\n" +
                "{23}\n" +
                ":globe_with_meridians: **{24}** [schoolido.lu]({25})\n" +
                ":notepad_spiral: **{26}**\n" +
                ":red_circle: {27}: {28} - {29} - {30}\n" +
                ":green_circle: {31}: {32} - {33} - {34}\n" +
                ":blue_circle: {35}: {36} - {37} - {38}\n",
                tr.GetString(lang, "cardCharacter"), cardObject.Idol.Name,
                tr.GetString(lang, "cardID"), cardObject.Id,
                tr.GetString(lang, "cardRarity"), cardObject.Rarity,
                GetEmojiForAttribute(cardObject.Attribute), tr.GetString(lang, "cardAttribute"), cardObject.Attribute,
                tr.GetString(lang, "cardSet"), cardObject.Translated_collection ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "cardRelease"), cardObject.Release_date ?? tr.GetString(lang, "noData"),
                GetEmojiAvailability(cardObject.Japan_only), tr.GetString(lang, "cardAvailability"),
                cardObject.Japan_only.Value ? tr.GetString(lang, "cardOnlyJP") : tr.GetString(lang, "cardJPandEN") ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "cardHP"), cardObject.Hp?.ToString() ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "cardSkill"), cardObject.Skill ?? tr.GetString(lang, "noData"),
                cardObject.Skill_details ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "cardCenterSkill"), cardObject.Center_skill ?? tr.GetString(lang, "noData"),
                cardObject.Center_skill_details ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "cardURL"), cardObject.Website_url,
                tr.GetString(lang, "cardStats"),
                tr.GetString(lang, "smile"), cardObject.Minimum_statistics_smile ?? tr.GetString(lang, "noData"),
                cardObject.Non_idolized_maximum_statistics_smile ?? tr.GetString(lang, "noData"), cardObject.Idolized_maximum_statistics_smile ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "pure"), cardObject.Minimum_statistics_pure ?? tr.GetString(lang, "noData"),
                cardObject.Non_idolized_maximum_statistics_pure ?? tr.GetString(lang, "noData"), cardObject.Idolized_maximum_statistics_pure ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "cool"), cardObject.Minimum_statistics_cool ?? tr.GetString(lang, "noData"),
                cardObject.Non_idolized_maximum_statistics_cool ?? tr.GetString(lang, "noData"), cardObject.Idolized_maximum_statistics_cool ?? tr.GetString(lang, "noData"));           

            if(cardObject.Rarity == "UR")
            {
                cardDescription.AppendFormat(":handshake: **{0}**\n", tr.GetString(lang, "cardPair"));
                {
                    if(cardObject.Ur_pair != null)
                    {
                        cardDescription.AppendFormat("{0} ({1})\n", cardObject.Ur_pair.Card?.Name ?? tr.GetString(lang, "noData"), cardObject.Ur_pair.Card?.Id ?? tr.GetString(lang, "noData"));
                        if(cardObject.Ur_pair.Card != null && cardObject.Ur_pair.Card.Id != null)
                        {
                            cardDescription.Append("*").AppendFormat(tr.GetString(lang, "cardPairDetail"), cardObject.Ur_pair.Card.Id).Append("*\n");
                        }
                    }
                    else
                    {
                        cardDescription.Append(tr.GetString(lang, "cardNoPair")).AppendLine();
                    }
                }
            }

            cardDescription.AppendFormat(":stadium: **{0}**\n", tr.GetString(lang, "cardEvent"));
            if (cardObject.Event == null)
            {
                cardDescription.Append(tr.GetString(lang, "cardNoEvent")).AppendLine();
            }
            else
            {
                if(cardObject.Other_event == null)
                {
                    cardDescription.AppendFormat(":name_badge: **{0}**\n", tr.GetString(lang, "cardEventJP"));
                    cardDescription.Append(cardObject.Event.Japanese_name).AppendLine();
                    cardDescription.AppendFormat(":name_badge: **{0}**\n", tr.GetString(lang, "cardEventEN"));
                    cardDescription.Append(cardObject.Event.English_name ?? tr.GetString(lang, "noData")).AppendLine();
                }
                else
                {
                    cardDescription.Append(tr.GetString(lang, "cardEventDifferent")).AppendLine();
                    cardDescription.AppendFormat(":name_badge: **{0}**\n", tr.GetString(lang, "cardEventJP"));
                    cardDescription.Append(cardObject.Event.Japanese_name).AppendLine();
                    cardDescription.AppendFormat(":name_badge: **{0}**\n", tr.GetString(lang, "cardEventEN"));
                    cardDescription.Append(cardObject.Other_event.English_name ?? tr.GetString(lang, "noData")).AppendLine();
                }
            }

            return cardDescription.ToString();
        }

        public string MakeIdolDescription(Translator.Language lang, IdolObject idolObject)
        {
            StringBuilder idolDescription = new StringBuilder();
            idolDescription.AppendFormat(":name_badge: **{0}** ({1}({2}))\n" +
                ":school: **{3}** ({4})\n" +
                ":microphone: **{5}** ({6})\n" +
                ":notes: **{7}** ({8})\n" +
                "{9} **{10}** ({11})\n" +
                ":calendar: **{12}** ({13})\n" +
                ":birthday: **{14}** ({15})\n" +
                "{16} **{17}** ({18})\n" + 
                "{19} **{20}** ({21})\n" +
                ":straight_ruler: **{22}** ({23})\n" +
                "{24} **{25}** ({2})\n" + 
                ":ramen: **{27}**\n" +
                "{28}\n" +
                ":broccoli: **{29}**\n" +
                "{30}\n" +
                ":ping_pong: **{31}**\n" +
                "{32}\n" +
                ":microphone2: **{33}**\n" +
                "{34}\n" +
                ":globe_with_meridians: **{35}**\n" +
                "[schoolido.lu]({36})",
                tr.GetString(lang, "idolName"), idolObject.Name, idolObject.Japanese_name ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "idolSchool"), idolObject.School ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "idolMainUnit"), idolObject.Main_unit ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "idolSubUnit"), idolObject.Sub_unit ?? tr.GetString(lang, "noData"),
                GetEmojiForYear(idolObject.Year), tr.GetString(lang, "idolYear"), idolObject.Year ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "idolAge"), idolObject.Age?.ToString() ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "idolBirthday"), idolObject.Birthday ?? tr.GetString(lang, "noData"),
                GetEmojiForZodiacSign(idolObject.Astrological_sign), tr.GetString(lang, "idolZodiac"), idolObject.Astrological_sign ?? tr.GetString(lang, "noData"),
                GetEmojiForBloodType(idolObject.Blood), tr.GetString(lang, "idolBloodType"), idolObject.Blood ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "idolHeight"), idolObject.Height?.ToString() ?? tr.GetString(lang, "noData"),
                GetEmojiForAttribute(idolObject.Attribute), tr.GetString(lang, "idolAttribute"), idolObject.Attribute ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "idolFavoriteFood"),
                idolObject.Favorite_food ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "idolDislikedFood"),
                idolObject.Least_favorite_food ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "idolHobby"),
                idolObject.Hobbies ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "idolSeiyuu"),
                idolObject.Cv?.Name ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "idolURL"), idolObject.Website_url);

            if (idolObject.Wiki_url != null && idolObject.Wiki_url != "")
            {
                idolDescription.Append(" [wiki](").Append(idolObject.Wiki_url.Replace("%20", "_")).Append(")");
            }
            if (idolObject.Wikia_url != null && idolObject.Wikia_url != "")
            {
                idolDescription.Append(" [wikia](").Append(idolObject.Wikia_url).Append(")");
            }
            if (idolObject.Official_url != null && idolObject.Official_url != "")
            {
                idolDescription.Append(" [lovelive-anime.jp](").Append(idolObject.Official_url).Append(")");
            }

            idolDescription.AppendLine();

            return idolDescription.ToString();
        }

        public string MakeCurrentWorldEventDescription(Translator.Language lang, EventObject eventObject, bool finished, List<CardObject> eventCards = null)
        {
            StringBuilder eventDescription = new StringBuilder();
            eventDescription.AppendFormat(":name_badge: **{0}**\n" +
                "{1}\n" +
                ":name_badge: **{2}**\n" +
                "{3}\n" +
                ":clock2: **{4}**\n" +
                "{5} - {6}\n",
                tr.GetString(lang, "eventName"),
                eventObject.English_name ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "eventJapanName"),
                eventObject.Japanese_name,
                tr.GetString(lang, "eventDuration"),
                ConvertToPolandTimeFromUtc(eventObject.English_beginning)?.ToString("HH:mm dd.MM.yyyy") ?? tr.GetString(lang, "eventNoBeginDate"),
                ConvertToPolandTimeFromUtc(eventObject.English_end)?.ToString("HH:mm dd.MM.yyyy") ?? tr.GetString(lang, "eventNoEndDate"));

            if (finished == false)
            {
                eventDescription.AppendFormat(":timer: **{0}**\n" +
                    "{1}\n",
                    tr.GetString(lang, "eventRemainingTime"),
                    GetTimeToEventEnd(ConvertToPolandTimeFromUtc(eventObject.English_end)) ?? tr.GetString(lang, "eventCantCalculate"));
            }
            eventDescription.AppendFormat(":clock12: **{0}**\n" +
                "{1} - {2}\n" +
                ":globe_with_meridians: **{3}** [schoolido.lu]({4})\n" +
                ":notepad_spiral: **{5}**\n" +
                "{6}\n",
                tr.GetString(lang, "eventDurationUTC"),
                eventObject.English_beginning?.ToString("HH:mm dd.MM.yyyy") ?? tr.GetString(lang, "eventNoBeginDate"), eventObject.English_end?.ToString("HH:mm dd.MM.yyyy") ?? tr.GetString(lang, "eventNoEndDate"),
                tr.GetString(lang, "eventUrl"), eventObject.Website_url,
                tr.GetString(lang, "eventAdditionalInfo"),
                eventObject.Note ?? tr.GetString(lang, "noData"));

            if (finished == true)
            {
                eventDescription.AppendFormat(":medal: **{0}** ({1})\n" +
                    ":first_place: **{2}** {3}\n" +
                    ":second_place: **{4}** {5}\n",
                    tr.GetString(lang, "eventTiers"), tr.GetString(lang, "eventPoint"),
                    tr.GetString(lang, "eventT1"), eventObject.English_t1_points?.ToString() ?? tr.GetString(lang, "noData"),
                    tr.GetString(lang, "eventT2"), eventObject.English_t2_points?.ToString() ?? tr.GetString(lang, "noData"));
            }

            if (eventCards != null)
            {
                eventDescription.AppendFormat(":microphone: **{0}** ({1})\n",
                    tr.GetString(lang, "eventCards"), eventCards.Count);
                foreach (CardObject eventCard in eventCards)
                {
                    eventDescription.AppendFormat("{0} ({1})\n", eventCard.Idol.Name, eventCard.Id);
                }
                if (eventCards.Count == 0)
                {
                    eventDescription.Append(tr.GetString(lang, "eventCardsNotYet")).AppendLine();
                }
                eventDescription.Append("\n*").Append(tr.GetString(lang, "eventCardDetail")).Append("*");
            }
            else
            {
                eventDescription.AppendFormat(":microphone: **{0}** ({1})\n",
                    tr.GetString(lang, "eventCards"), tr.GetString(lang, "noData"));
            }

            return eventDescription.ToString();
        }

        public string MakeCurrentJapanEventDescription(Translator.Language lang, EventObject eventObject, bool finished, List<CardObject> eventCards = null)
        {
            StringBuilder eventDescription = new StringBuilder();
            eventDescription.AppendFormat(":name_badge: **{0}**\n" +
                "{1} ({2})\n" +
                ":clock2: **{3}**\n" +
                "{4} - {5}\n",
                tr.GetString(lang, "eventName"),
                eventObject.Japanese_name, eventObject.Romaji_name ?? tr.GetString(lang, "eventNoRomaji"),
                tr.GetString(lang, "eventDuration"),
                eventObject.Beginning?.ToString("HH:mm dd.MM.yyyy") ?? tr.GetString(lang, "eventNoBeginDate"),
                eventObject.End?.ToString("HH:mm dd.MM.yyyy") ?? tr.GetString(lang, "eventNoEndDate"));

            if (finished == false)
            {
                eventDescription.AppendFormat(":timer: **{0}**\n" +
                    "{1}\n",
                    tr.GetString(lang, "eventRemainingTime"),
                    GetTimeToEventEnd(eventObject.End) ?? tr.GetString(lang, "eventCantCalculate"));
            }
            eventDescription.AppendFormat(":clock9: **{0}**\n" +
                "{1} - {2}\n" +
                ":globe_with_meridians: **{3}** [schoolido.lu]({4})\n" +
                ":notepad_spiral: **{5}**\n" +
                "{6}\n",
                tr.GetString(lang, "eventDurationJST"),
                ConvertToJapanTimeFromPoland(eventObject.Beginning)?.ToString("HH:mm dd.MM.yyyy") ?? tr.GetString(lang, "eventNoBeginDate"),
                ConvertToJapanTimeFromPoland(eventObject.End)?.ToString("HH:mm dd.MM.yyyy") ?? tr.GetString(lang, "eventNoEndDate"),
                tr.GetString(lang, "eventUrl"), eventObject.Website_url,
                tr.GetString(lang, "eventAdditionalInfo"),
                eventObject.Note ?? tr.GetString(lang, "noData"));

            if (finished == true)
            {
                eventDescription.AppendFormat(":medal: **{0}** ({1})\n" +
                    ":first_place: **{2}** {3}\n" +
                    ":second_place: **{4}** {5}\n",
                    tr.GetString(lang, "eventTiers"), tr.GetString(lang, "eventPoint"),
                    tr.GetString(lang, "eventT1"), eventObject.Japanese_t1_points?.ToString() ?? tr.GetString(lang, "noData"),
                    tr.GetString(lang, "eventT2"), eventObject.Japanese_t2_points?.ToString() ?? tr.GetString(lang, "noData"));
            }

            if (eventCards != null)
            {
                eventDescription.AppendFormat(":microphone: **{0}** ({1})\n",
                    tr.GetString(lang, "eventCards"), eventCards.Count);
                foreach (CardObject eventCard in eventCards)
                {
                    eventDescription.AppendFormat("{0} ({1})\n", eventCard.Idol.Name, eventCard.Id);
                }
                if (eventCards.Count == 0)
                {
                    eventDescription.Append(tr.GetString(lang, "eventCardsNotYet")).AppendLine();
                }
                eventDescription.Append("\n*").Append(tr.GetString(lang, "eventCardDetail")).Append("*");
            }
            else
            {
                eventDescription.AppendFormat(":microphone: **{0}** ({1})\n",
                    tr.GetString(lang, "eventCards"), tr.GetString(lang, "noData"));
            }

            return eventDescription.ToString();
        }

        public string MakeSongDescription(Translator.Language lang, SongObject songObject, EventObject eventObject = null)
        {
            StringBuilder songDescription = new StringBuilder();
            songDescription.AppendFormat(":name_badge: **{0}** ({1} ({2}))\n" +
                ":microphone: **{3}** ({4})\n" +
                "{5} **{6}** ({7})\n" +
                ":watch: **{8}** {9}\n" +
                ":stopwatch: **{10}** {11}\n" +
                ":stadium: **{12}**\n",
                tr.GetString(lang, "songTitle"), songObject.Name, songObject.Romaji_name ?? "",
                tr.GetString(lang, "songMainUnit"), songObject.Main_unit ?? tr.GetString(lang, "noData"),
                GetEmojiForAttribute(songObject.Attribute), tr.GetString(lang, "songAttribute"), songObject.Attribute ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "songTime"), songObject.Time.ToString() ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "songBpm"), songObject.Bpm.ToString() ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "songEvent"));

            if (eventObject == null)
            {
                songDescription.Append(tr.GetString(lang, "songNotInEvent")).AppendLine();
            }
            else
            {
                songDescription.Append(tr.GetString(lang, "songInEvent")).Append(eventObject.Japanese_name);
                if (eventObject.English_name != null)
                {
                    songDescription.Append(" (").Append(eventObject.English_name).Append(")");
                }
                songDescription.AppendLine().Append("*").AppendFormat(tr.GetString(lang, "songEventDetail"), eventObject.Japanese_name).Append("*").AppendLine();
            }

            songDescription.AppendFormat(":notepad_spiral: **{0}**\n" +
                "{1} - :star: {2} {3} - :musical_note: {4} {5}\n" +
                "{6} - :star: {7} {8} - :musical_note: {9} {10}\n" +
                "{11} - :star: {12} {13} - :musical_note: {14} {15}\n" +
                "{16} - :star: {17} {18} - :musical_note: {19} {20}\n" +
                "{21} - :star: {22} {23} - :musical_note: {24} {25}\n" +
                ":globe_with_meridians: **{26}**\n" +
                "[schoolido.lu]({27})",
                tr.GetString(lang, "songStats"),
                tr.GetString(lang, "songEasy"), tr.GetString(lang, "songDifficulty"), songObject.Easy_difficulty.ToString() ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "songNotes"), songObject.Easy_notes.ToString() ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "songNormal"), tr.GetString(lang, "songDifficulty"), songObject.Normal_difficulty.ToString() ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "songNotes"), songObject.Normal_notes.ToString() ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "songHard"), tr.GetString(lang, "songDifficulty"), songObject.Hard_difficulty.ToString() ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "songNotes"), songObject.Hard_notes.ToString() ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "songExpert"), tr.GetString(lang, "songDifficulty"), songObject.Expert_difficulty.ToString() ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "songNotes"), songObject.Expert_notes.ToString() ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "songMaster"), tr.GetString(lang, "songDifficulty"), songObject.Master_difficulty.ToString() ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "songNotes"), songObject.Master_notes.ToString() ?? tr.GetString(lang, "noData"),
                tr.GetString(lang, "songURL"), songObject.Website_url);

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
