using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    class ParameterLangAttribute : Attribute
    {
        public string PolishParameter { get; }
        public string EnglishParameter { get; }

        public ParameterLangAttribute(string polish, string english)
        {
            PolishParameter = polish;
            EnglishParameter = english;
        }

        public string GetParameterName(Translator.Language lang)
        {
            switch (lang)
            {
                case Translator.Language.Polish:
                    return PolishParameter;
                case Translator.Language.English:
                    return EnglishParameter;
                default:
                    return EnglishParameter;
            }
        }
    }
}
