using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    class AliasLangAttribute : Attribute
    {
        public List<string> PolishAliases { get; }
        public List<string> EnglishAliases { get; }

        public AliasLangAttribute(List<string> polish, List<string> english)
        {
            PolishAliases = polish;
            EnglishAliases = english;
        }

        public List<string> GetAliases(Translator.Language lang)
        {
            switch (lang)
            {
                case Translator.Language.Polish:
                    return PolishAliases;
                case Translator.Language.English:
                    return EnglishAliases;
                default:
                    return EnglishAliases;
            }
        }
    }
}
