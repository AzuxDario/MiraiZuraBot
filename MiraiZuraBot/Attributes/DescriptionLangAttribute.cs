using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
    class DescriptionLangAttribute : Attribute
    {
        public string PolishDescription { get; }
        public string EnglishDescription { get; }

        public DescriptionLangAttribute(string polish, string english)
        {
            PolishDescription = polish;
            EnglishDescription = english;
        }

        public string GetDescription(Translator.Language lang)
        {
            switch (lang)
            {
                case Translator.Language.Polish:
                    return PolishDescription;
                case Translator.Language.English:
                    return EnglishDescription;
                default:
                    return EnglishDescription;
            }
        }
    }
}
