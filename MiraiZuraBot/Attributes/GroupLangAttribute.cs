using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    class GroupLangAttribute : Attribute
    {
        public string PolishGroup { get; }
        public string EnglishGroup { get; }

        public GroupLangAttribute(string polish, string english)
        {
            PolishGroup = polish;
            EnglishGroup = english;
        }

        public string GetGroup(Translator.Language lang)
        {
            switch (lang)
            {
                case Translator.Language.Polish:
                    return PolishGroup;
                case Translator.Language.English:
                    return EnglishGroup;
                default:
                    return EnglishGroup;
            }
        }
    }
}
