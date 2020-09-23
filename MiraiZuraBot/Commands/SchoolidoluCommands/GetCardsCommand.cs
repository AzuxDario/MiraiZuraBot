using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Helpers.SchoolidoluHelper;
using MiraiZuraBot.Services.LanguageService;
using MiraiZuraBot.Services.SchoolidoluService;
using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.SchoolidoluCommands
{
    [GroupLang("SIF", "SIF")]
    class GetCardsCommand : BaseCommandModule
    {
        private SchoolidoluService _schoolidoluService;
        private SchoolidoluHelper _schoolidoluHelper;
        private LanguageService _languageService;
        private Translator _translator;

        public GetCardsCommand(SchoolidoluService schoolidoluService, SchoolidoluHelper schoolidoluHelper, LanguageService languageService, Translator translator)
        {
            _schoolidoluService = schoolidoluService;
            _schoolidoluHelper = schoolidoluHelper;
            _languageService = languageService;
            _translator = translator;
        }


        [Command("karta")]
        [Aliases("card")]
        [CommandLang("karta", "card")]
        [DescriptionLang("Pokazuje karte na bazie jej id.\nnp:\n`karta 1599`\n`karta 1599 idolizowana`", "Shows card based on her id.\ne.g.\n`card 1599`\n`card 1599 idolised`")]
        public async Task Card(CommandContext ctx, [DescriptionLang("ID Karty", "Card ID"), ParameterLang("ID", "ID")] string id,
            [DescriptionLang("Napisz `idolizowana` dla idolizowanej karty", "Write `idolised` for idolised card"), ParameterLang("Idolizowana", "Idolised")] string isIdolised = null)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            var cardData = _schoolidoluService.GetCardById(id);

            if (cardData.StatusCode == HttpStatusCode.OK)
            {
                if (isIdolised == "idolizowana" || isIdolised == "idolised")
                {
                    // Some cards might not have idolised version
                    if (cardData.Data.Card_idolized_image != null)
                    {
                        await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "card"), _schoolidoluHelper.MakeCardDescription(lang, cardData.Data, true),
                            cardData.Data.Card_idolized_image != null ? "http:" + cardData.Data.Card_idolized_image : null,
                            cardData.Data.Round_card_idolized_image != null ? "http:" + cardData.Data.Round_card_idolized_image : null, SchoolidoluHelper.GetSchoolidoluFotter(),
                            _schoolidoluHelper.GetColorForAttribute(cardData.Data.Attribute));
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "card"), _schoolidoluHelper.MakeCardDescription(lang, cardData.Data, false),
                            cardData.Data.Card_image != null ? "http:" + cardData.Data.Card_image : null,
                            cardData.Data.Round_card_image != null ? "http:" + cardData.Data.Round_card_image : null, SchoolidoluHelper.GetSchoolidoluFotter(),
                            _schoolidoluHelper.GetColorForAttribute(cardData.Data.Attribute));
                    }
                }
                else
                {
                    // Some cards are only idolised
                    if (cardData.Data.Card_image != null)
                    {
                        await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "card"), _schoolidoluHelper.MakeCardDescription(lang, cardData.Data, false),
                            cardData.Data.Card_image != null ? "http:" + cardData.Data.Card_image : null,
                            cardData.Data.Round_card_image != null ? "http:" + cardData.Data.Round_card_image : null, SchoolidoluHelper.GetSchoolidoluFotter(),
                            _schoolidoluHelper.GetColorForAttribute(cardData.Data.Attribute));
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "card"), _schoolidoluHelper.MakeCardDescription(lang, cardData.Data, true),
                            cardData.Data.Card_idolized_image != null ? "http:" + cardData.Data.Card_idolized_image : null,
                            cardData.Data.Round_card_idolized_image != null ? "http:" + cardData.Data.Round_card_idolized_image : null, SchoolidoluHelper.GetSchoolidoluFotter(),
                            _schoolidoluHelper.GetColorForAttribute(cardData.Data.Attribute));
                    }
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "card"), _translator.GetString(lang, "cardDoesntExist"), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("losowaKarta")]
        [Aliases("randomCard")]
        [CommandLang("losowaKarta", "randomCard")]
        [DescriptionLang("Pokazuje losową karte. Można sprecyzować imie idolki.\nnp:\n`losowaKarta`\n`losowaKarta Watanabe You`", "Shows random card.\ne.g.\n`randomCard`\n`randomCard Watanabe You`")]
        public async Task RandomCard(CommandContext ctx, [DescriptionLang("Imie idolki", "Idol name"), ParameterLang("Imie", "Name"), RemainingText] string name)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "random" },
                { "page_size", "1" }
            };

            if (name != "")
            {
                options.Add("name", name);
            }

            var cardsResponse = _schoolidoluService.GetCard(options);

            if (cardsResponse.StatusCode == HttpStatusCode.OK && cardsResponse.Data.Results.Count > 0)
            {

                // Some cards are only idolised
                if (cardsResponse.Data.Results[0].Card_image != null)
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "cardRandom"),
                        _schoolidoluHelper.MakeCardDescription(lang, cardsResponse.Data.Results[0], false),
                            cardsResponse.Data.Results[0].Card_image != null ? "http:" + cardsResponse.Data.Results[0].Card_image : null,
                            cardsResponse.Data.Results[0].Round_card_image != null ? ("https:" + cardsResponse.Data.Results[0].Round_card_image) : null, SchoolidoluHelper.GetSchoolidoluFotter(),
                            _schoolidoluHelper.GetColorForAttribute(cardsResponse.Data.Results[0].Attribute));
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "cardRandom"),
                        _schoolidoluHelper.MakeCardDescription(lang, cardsResponse.Data.Results[0], true),
                            cardsResponse.Data.Results[0].Card_idolized_image != null ? "http:" + cardsResponse.Data.Results[0].Card_idolized_image : null,
                            cardsResponse.Data.Results[0].Round_card_idolized_image != null ? ("https:" + cardsResponse.Data.Results[0].Round_card_idolized_image) : null, SchoolidoluHelper.GetSchoolidoluFotter(),
                            _schoolidoluHelper.GetColorForAttribute(cardsResponse.Data.Results[0].Attribute));
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "card"), _translator.GetString(lang, "cardIdolDoesntExist"), null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("wyszukajKarte")]
        [Aliases("searchCard")]
        [CommandLang("wyszukajKarte", "searchCard")]
        [DescriptionLang("Wyszukuje karty na podstawie fraz oraz rzadkości i atrybutu.\nnp:\n`wyszukajKarte 1 Watanabe You UR cool`\nPolecam jako początkową stronę podać `1`." +
            "\nDozwolone atrybuty: `Smile`, `Pure`, `Cool`, `All`\nDozwolone rzadkości: `N`, `R`, `SR`, `SSR`, `UR`" +
            "\nMożna podać jedną wartość atrybutu oraz wiele rzadkości w dowolnym miejscu zapytania." +
            "\nWyszukiwanie odbywa się po imionach, skillach oraz eventach.",
            "Search for cards based on phrases, rarity and attribute.\ne.g.\n`searchCard 1 Watanabe You UR cool`\nI recommend to choose `1` as the initial page." +
            "\nAllowed attributes: `Smile`, `Pure`, `Cool`, `All` \nAllowed rarities: `N`, `R`, `SR`, `SSR`, `UR`" +
            "\nOne attribute value and multiple rarities can be specified anywhere in the query." +
            "\nSearch is done by names, skills and events.")]
        public async Task SearchCard(CommandContext ctx, [DescriptionLang("Strona wyników", "Result page"), ParameterLang("Strona", "Page")] string page,
            [DescriptionLang("Fraza do wyszukania", "The phrase to search for"), ParameterLang("Słowa kluczowe", "Keywords"), RemainingText] string keywords)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            int intPage;

            if(!int.TryParse(page, out intPage))
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "cardSearch"), _translator.GetString(lang, "cardSearchNoPage"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                return;
            }

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "rarity", FindAndRemoveRarity(keywords, out keywords)},
                { "attribute", FindAndRemoveAttribute(keywords, out keywords)},
                { "search", keywords.Trim() },
                { "page", intPage.ToString() }
            };

            var cardObject = _schoolidoluService.GetCard(options);

            if (cardObject.StatusCode == HttpStatusCode.OK)
            {

                if (cardObject.Data.Count != 0)
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "cardSearch"), _schoolidoluHelper.MakeSearchCardDescription(lang, cardObject.Data, 10, intPage),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "cardSearch"), _translator.GetString(lang, "cardSearchNoResult"),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "cardSearch"),
                    string.Format(_translator.GetString(lang, "cardSearchError"), keywords.Trim()),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        private string FindAndRemoveRarity(string query, out string newQuery)
        {
            List<string> rarityToFind = new List<string>(){ " N ", " R ", " SR ", " SSR ", " UR "};
            List<string> rarityToAdd = new List<string>() { "N", "R", "SR", "SSR", "UR" };

            return FindAndRemove(rarityToFind, rarityToAdd, query, out newQuery);
        }

        private string FindAndRemoveAttribute(string query, out string newQuery)
        {
            List<string> attributeToFind = new List<string>() { " Smile ", " Pure ", " Cool ", " All "};
            List<string> attributeToAdd = new List<string>() { "Smile", "Pure", "Cool", "All" };

            return FindAndRemove(attributeToFind, attributeToAdd, query, out newQuery);
        }

        private string FindAndRemove(List<string> thingsToFind, List<string> thingsToAdd, string query, out string newQuery)
        {
            List<string> foundThings = new List<string>();

            query += " ";

            for (int i = 0; i < thingsToFind.Count; i++)
            {
                if (query.IndexOf(thingsToFind[i], StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    foundThings.Add(thingsToAdd[i]);
                    query = query.Replace(thingsToFind[i], " ", true, null);
                }
            }
            newQuery = query;
            return string.Join(",", foundThings);
        }
    }
}
