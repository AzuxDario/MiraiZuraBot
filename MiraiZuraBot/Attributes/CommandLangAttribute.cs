using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    class CommandLangAttribute : Attribute
    {
        public string PolishCommand { get; }
        public string EnglishCommand { get; }

        public CommandLangAttribute(string polish, string english)
        {
            PolishCommand = polish;
            EnglishCommand = english;
        }

        public string GetCommand(Translator.Language lang)
        {
            switch (lang)
            {
                case Translator.Language.Polish:
                    return PolishCommand;
                case Translator.Language.English:
                    return EnglishCommand;
                default:
                    return EnglishCommand;
            }
        }
    }
}
