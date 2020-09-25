using MiraiZuraBot.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiraiZuraBot.Translators
{
    class Translator
    {
        public enum Language { Polish, English };

        public enum DbString { Birthdays, Trivia, RandomMessages };

        private List<string> availableLanguages = new List<string>() { "English", "Polski" };

        private Dictionary<Language, Dictionary<string, string>> languageStrings;
        private Dictionary<Language, Dictionary<DbString, Dictionary<string, string>>> databaseStrings;
        private const string translationsDirectory = "translations/";

        public Translator()
        {
            languageStrings = new Dictionary<Language, Dictionary<string, string>>();

            languageStrings.Add(Language.Polish, ReadFile(Language.Polish, "polish.json"));
            languageStrings.Add(Language.English, ReadFile(Language.English, "english.json"));

            databaseStrings = new Dictionary<Language, Dictionary<DbString, Dictionary<string, string>>>();

            databaseStrings.Add(Language.Polish,
                new Dictionary<DbString, Dictionary<string, string>>
                {
                    { DbString.Birthdays, ReadFile(Language.Polish, "polishBirthdays.json") },
                    { DbString.Trivia, ReadFile(Language.Polish, "polishTrivia.json") }
                }
            );

            databaseStrings.Add(Language.English,
                new Dictionary<DbString, Dictionary<string, string>>
                {
                    { DbString.Birthdays, ReadFile(Language.English, "englishBirthdays.json") },
                    { DbString.Trivia, ReadFile(Language.English, "englishTrivia.json") }
                }
            );

        }

        public string GetString(Language lang, string stringName)
        {
            try
            {
                return languageStrings[lang][stringName];
            }
            catch (KeyNotFoundException)
            {
                return "String not found";
            }
        }

        public string GetDbString(Language lang, DbString dbString, string stringName)
        {
            try
            {
                return databaseStrings[lang][dbString][stringName];
            }
            catch (KeyNotFoundException)
            {
                return "String not found";
            }
        }

        public Language GetEnumForString(string language)
        {
            switch (language)
            {
                case "English":
                    return Language.English;
                case "Polski":
                    return Language.Polish;
                default:
                    return GetDefaultLanguage();
            }
        }

        public Language GetDefaultLanguage()
        {
            return Language.English;
        }

        public List<string> GetAvailableLanguages()
        {
            return availableLanguages;
        }

        private Dictionary<string, string> ReadFile(Language lang, string filename)
        {
            try
            {
                string file = File.ReadAllText(translationsDirectory + filename);

                return JsonConvert.DeserializeObject<Dictionary<string, string>>(file);
            }
            catch (FileNotFoundException ex)
            {
                Bot.DiscordClient.DebugLogger.LogMessage(DSharpPlus.LogLevel.Critical, Bot.botname, "Can't find language file: " + filename + " Errored: " + ex.Message, DateTime.Now);
                Environment.Exit(-1);
            } 
            catch (DirectoryNotFoundException ex)
            {
                Bot.DiscordClient.DebugLogger.LogMessage(DSharpPlus.LogLevel.Critical, Bot.botname, "Can't find directory: " + translationsDirectory + " Errored: " + ex.Message, DateTime.Now);
                Environment.Exit(-1);
            }
            catch (JsonSerializationException ex)
            {
                Bot.DiscordClient.DebugLogger.LogMessage(DSharpPlus.LogLevel.Critical, Bot.botname, "Problem in JSON file: " + filename + " Errored: " + ex.Message, DateTime.Now);
                Environment.Exit(-1);
            }
            catch (Exception ex)
            {
                Bot.DiscordClient.DebugLogger.LogMessage(DSharpPlus.LogLevel.Critical, Bot.botname, "Problem during reading: " + filename + " Errored: " + ex.Message, DateTime.Now);
                Environment.Exit(-1);
            }
            return null;
        }
    }
}
