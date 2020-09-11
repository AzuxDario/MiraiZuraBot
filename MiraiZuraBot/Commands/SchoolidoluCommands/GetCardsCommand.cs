using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Containers.Schoolidolu;
using MiraiZuraBot.Containers.Schoolidolu.Cards;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Helpers.SchoolidoluHelper;
using MiraiZuraBot.Services.SchoolidoluService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.SchoolidoluCommands
{
    [CommandsGroup("SIF")]
    class GetCardsCommand : BaseCommandModule
    {
        private SchoolidoluService _schoolidoluService;
        private SchoolidoluHelper _schoolidoluHelper;

        public GetCardsCommand(SchoolidoluService schoolidoluService, SchoolidoluHelper schoolidoluHelper)
        {
            _schoolidoluService = schoolidoluService;
            _schoolidoluHelper = schoolidoluHelper;
        }


        [Command("karta")]
        [Description("Pokazuje karte na bazie jej id.\nnp:\n*karta 1599\n*karta 1599 idolizowana")]
        public async Task Card(CommandContext ctx, [Description("ID karty.")] string id, [Description("Czy idolizowana.")] string isIdolised = null)
        {
            await ctx.TriggerTypingAsync();

            var cardData = _schoolidoluService.GetCardById(id);

            if (cardData.StatusCode == HttpStatusCode.OK)
            {
                if (isIdolised == "idolizowana")
                {
                    // Some cards might not have idolised version
                    if (cardData.Data.Card_idolized_image != null)
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardData.Data.Id + " : " + cardData.Data.Idol.Name, _schoolidoluHelper.MakeCardDescription(cardData.Data, true),
                            cardData.Data.Card_idolized_image != null ? "http:" + cardData.Data.Card_idolized_image : null,
                            cardData.Data.Round_card_idolized_image != null ? "http:" + cardData.Data.Round_card_idolized_image : null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardData.Data.Id + " : " + cardData.Data.Idol.Name, _schoolidoluHelper.MakeCardDescription(cardData.Data, false),
                            cardData.Data.Card_image != null ? "http:" + cardData.Data.Card_image : null,
                            cardData.Data.Round_card_image != null ? "http:" + cardData.Data.Round_card_image : null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                }
                else
                {
                    // Some cards are only idolised
                    if (cardData.Data.Card_image != null)
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardData.Data.Id + " : " + cardData.Data.Idol.Name, _schoolidoluHelper.MakeCardDescription(cardData.Data, false),
                            cardData.Data.Card_image != null ? "http:" + cardData.Data.Card_image : null,
                            cardData.Data.Round_card_image != null ? "http:" + cardData.Data.Round_card_image : null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardData.Data.Id + " : " + cardData.Data.Idol.Name, _schoolidoluHelper.MakeCardDescription(cardData.Data, true),
                            cardData.Data.Card_idolized_image != null ? "http:" + cardData.Data.Card_idolized_image : null,
                            cardData.Data.Round_card_idolized_image != null ? "http:" + cardData.Data.Round_card_idolized_image : null, SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "Karta", "Podana karta nie istnieje.", null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("losowaKarta")]
        [Description("Pokazuje losową karte. Można sprecyzować imie idolki.\nnp:\n*losowaKarta\n*losowaKarta Watanabe You")]
        public async Task RandomCard(CommandContext ctx, [Description("Imie idolki."), RemainingText] string name)
        {
            await ctx.TriggerTypingAsync();

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
                    await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardsResponse.Data.Results[0].Id + " : " + cardsResponse.Data.Results[0].Idol.Name,
                        _schoolidoluHelper.MakeCardDescription(cardsResponse.Data.Results[0], false),
                            cardsResponse.Data.Results[0].Card_image != null ? "http:" + cardsResponse.Data.Results[0].Card_image : null,
                            cardsResponse.Data.Results[0].Round_card_image != null ? ("https:" + cardsResponse.Data.Results[0].Round_card_image) : null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardsResponse.Data.Results[0].Id + " : " + cardsResponse.Data.Results[0].Idol.Name,
                        _schoolidoluHelper.MakeCardDescription(cardsResponse.Data.Results[0], true),
                            cardsResponse.Data.Results[0].Card_idolized_image != null ? "http:" + cardsResponse.Data.Results[0].Card_idolized_image : null,
                            cardsResponse.Data.Results[0].Round_card_idolized_image != null ? ("https:" + cardsResponse.Data.Results[0].Round_card_idolized_image) : null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "Karta", "Podana idolka nie istnieje.", null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        [Command("wyszukajKarte")]
        [Description("Wyszukuje karty na podstawie fraz oraz rzadkości i atrybutu.\nnp:\n`wyszukajKarte 1 Watanabe You UR cool`\nPolecam jako początkową stronę podać `1`." +
            "\nDozwolone atrybuty: `Smile`, `Pure`, `Cool`, `All`nDozwolone rzadkości: `N`, `R`, `SR`, `SSR`, `UR`" +
            "\n Można podać jedną wartość atrybutu oraz wiele rzadkości w dowolnym miejscu zapytania." +
            "\nWyszukiwanie odbywa się po imionach, skillach oraz eventach.")]
        public async Task SearchCard(CommandContext ctx, [Description("Strona wyników.")] int page, [Description("Słowa kluczowe."), RemainingText] string keywords)
        {
            await ctx.TriggerTypingAsync();

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "rarity", FindAndRemoveRarity(keywords, out keywords)},
                { "attribute", FindAndRemoveAttribute(keywords, out keywords)},
                { "search", keywords.Trim() },
                { "page", page.ToString() }
            };

            var cardObject = _schoolidoluService.GetCard(options);

            if (cardObject.StatusCode == HttpStatusCode.OK)
            {

                if (cardObject.Data.Count != 0)
                {
                    await PostEmbedHelper.PostEmbed(ctx, "Wyszukiwanie kart", _schoolidoluHelper.MakeSearchCardDescription(cardObject.Data, 10, page),
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    await PostEmbedHelper.PostEmbed(ctx, "Wyszukiwanie kart", "Brak wyników, spróbuj wyszukać inną frazę. Pamiętaj, że można podać tylko jeden atrybut.",
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, "Wyszukiwanie kart",
                    "Wystąpił błąd podczas pobierania kart. Mogło nastąpić odwołanie do nieistniejącej strony. Spróbuj wybrać stronę pierwszą.\n`wyszukajKarte 1 " + keywords + "`",
                        null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
        }

        private string FindAndRemoveRarity(string query, out string newQuery)
        {
            List<string> rarityToFind = new List<string>(){ " N ", " R ", " SR ", " SSR ", " UR "};
            List<string> rarityToAdd = new List<string>() { "N", "R", "SR", "SSR", "UR" };

            List<string> foundRarities = new List<string>();

            query += " ";

            for(int i = 0; i < rarityToFind.Count; i++)
            {
                if (query.IndexOf(rarityToFind[i], StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    foundRarities.Add(rarityToAdd[i]);
                    query = query.Replace(rarityToFind[i], " ", true, null);
                }
            }
            newQuery = query;
            return string.Join(",", foundRarities);
        }

        private string FindAndRemoveAttribute(string query, out string newQuery)
        {
            List<string> attributeToFind = new List<string>() { " Smile ", " Pure ", " Cool ", " All "};
            List<string> attributeToAdd = new List<string>() { "Smile", "Pure", "Cool", "All" };

            List<string> foundAttributes = new List<string>();

            query += " ";

            for (int i = 0; i < attributeToFind.Count; i++)
            {
                if (query.IndexOf(attributeToFind[i], StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    foundAttributes.Add(attributeToAdd[i]);
                    query = query.Replace(attributeToFind[i], " ", true, null);
                }
            }
            newQuery = query;
            return string.Join(",", foundAttributes);
        }
    }
}
