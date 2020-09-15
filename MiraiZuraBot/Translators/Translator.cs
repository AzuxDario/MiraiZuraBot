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
        public enum Language { Polish };

        private Dictionary<Language, Dictionary<string, string>> languageStrings;
        private const string translationsDirectory = "translations/";

        public Translator()
        {
            languageStrings = new Dictionary<Language, Dictionary<string, string>>();

            ReadFile(Language.Polish, "polish.json");

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

        private void ReadFile(Language lang, string filename)
        {
            try
            {
                string file = File.ReadAllText(translationsDirectory + filename);

                Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(file);
                languageStrings.Add(lang, dict);
            }
            catch (FileNotFoundException)
            {
                Bot.DiscordClient.DebugLogger.LogMessage(DSharpPlus.LogLevel.Critical, Bot.botname, "Can't find language file: " + filename, DateTime.Now);
                Environment.Exit(-1);
            } 
            catch (DirectoryNotFoundException)
            {
                Bot.DiscordClient.DebugLogger.LogMessage(DSharpPlus.LogLevel.Critical, Bot.botname, "Can't find directory: " + translationsDirectory, DateTime.Now);
                Environment.Exit(-1);
            }
            catch (JsonSerializationException)
            {
                Bot.DiscordClient.DebugLogger.LogMessage(DSharpPlus.LogLevel.Critical, Bot.botname, "Problem in JSON file: " + filename, DateTime.Now);
                Environment.Exit(-1);
            }
            catch
            {
                Bot.DiscordClient.DebugLogger.LogMessage(DSharpPlus.LogLevel.Critical, Bot.botname, "Problem during reading: " + filename, DateTime.Now);
                Environment.Exit(-1);
            }
        }
    }
}
