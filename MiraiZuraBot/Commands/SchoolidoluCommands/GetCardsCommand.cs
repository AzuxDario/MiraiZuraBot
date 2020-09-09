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
                        string description = _schoolidoluHelper.MakeCardDescription(cardData.Data, true);
                        await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardData.Data.Id + " : " + cardData.Data.Idol.Name, description, "http:" + cardData.Data.Card_idolized_image,
                                                        SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        string description = _schoolidoluHelper.MakeCardDescription(cardData.Data, false);
                        await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardData.Data.Id + " : " + cardData.Data.Idol.Name, description, "http:" + cardData.Data.Card_image,
                                                        SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                }
                else
                {
                    // Some cards are only idolised
                    if (cardData.Data.Card_image != null)
                    {
                        string description = _schoolidoluHelper.MakeCardDescription(cardData.Data, false);
                        await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardData.Data.Id + " : " + cardData.Data.Idol.Name, description, "http:" + cardData.Data.Card_image,
                                                        SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                    else
                    {
                        string description = _schoolidoluHelper.MakeCardDescription(cardData.Data, true);
                        await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardData.Data.Id + " : " + cardData.Data.Idol.Name, description, "http:" + cardData.Data.Card_idolized_image,
                                                        SchoolidoluHelper.GetSchoolidoluFotter());
                    }
                }
            }
            else
            {
                await ctx.RespondAsync("Podana karta nie istnieje.");
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

            if (cardsResponse.StatusCode == HttpStatusCode.OK)
            {
                
                // Some cards are only idolised
                if (cardsResponse.Data.Results[0].Card_image != null)
                {
                    string description = _schoolidoluHelper.MakeCardDescription(cardsResponse.Data.Results[0], false);
                    await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardsResponse.Data.Results[0].Id + " : " + cardsResponse.Data.Results[0].Idol.Name, description,
                                                    "http:" + cardsResponse.Data.Results[0].Card_image, SchoolidoluHelper.GetSchoolidoluFotter());
                }
                else
                {
                    string description = _schoolidoluHelper.MakeCardDescription(cardsResponse.Data.Results[0], true);
                    await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardsResponse.Data.Results[0].Id + " : " + cardsResponse.Data.Results[0].Idol.Name, description,
                                                    "http:" + cardsResponse.Data.Results[0].Card_idolized_image, SchoolidoluHelper.GetSchoolidoluFotter());
                }
            }
            else
            {
                await ctx.RespondAsync("Podana idolka nie istnieje.");
            }
        }

        public string MakeCardDescription(CardObject cardObject, bool isIdolised)
        {
            StringBuilder cardDescription = new StringBuilder();
            cardDescription.Append("**ID:** ").Append(cardObject.Id).AppendLine();
            cardDescription.Append("**Postać:** ").Append(cardObject.Idol.Name).AppendLine();
            cardDescription.Append("**Rzadkość:** ").Append(cardObject.Rarity).AppendLine();
            cardDescription.Append("**Atrybut:** ").Append(cardObject.Attribute).AppendLine();
            cardDescription.Append("**Set:** ").Append(cardObject.Translated_collection).AppendLine();
            cardDescription.Append("**Data wypuszczenia (yyyy-MM-dd) :** ").Append(cardObject.Release_date).AppendLine();
            cardDescription.Append("**URL:** ").Append(cardObject.Website_url).AppendLine();
            cardDescription.Append("**Typ skilla:** ").Append(cardObject.Skill).AppendLine();
            cardDescription.Append("**Szczegóły skilla:** ").Append(cardObject.Skill_details).AppendLine();
            cardDescription.Append("**Center skill:** ").Append(cardObject.Center_skill).AppendLine();
            cardDescription.Append("**Szczegóły centera:** ").Append(cardObject.Center_skill_details).AppendLine();
            cardDescription.Append("**HP:** ").Append(cardObject.Hp).AppendLine();
            if (isIdolised)
            {
                cardDescription.Append("**Smile:** ").Append(cardObject.Minimum_statistics_smile).Append(" - ").Append(cardObject.Idolized_maximum_statistics_smile).AppendLine();
                cardDescription.Append("**Pure:** ").Append(cardObject.Minimum_statistics_pure).Append(" - ").Append(cardObject.Idolized_maximum_statistics_pure).AppendLine();
                cardDescription.Append("**Cool:** ").Append(cardObject.Minimum_statistics_cool).Append(" - ").Append(cardObject.Idolized_maximum_statistics_cool).AppendLine();
            }
            else
            {
                cardDescription.Append("**Smile:** ").Append(cardObject.Minimum_statistics_smile).Append(" - ").Append(cardObject.Non_idolized_maximum_statistics_smile).AppendLine();
                cardDescription.Append("**Pure:** ").Append(cardObject.Minimum_statistics_pure).Append(" - ").Append(cardObject.Non_idolized_maximum_statistics_pure).AppendLine();
                cardDescription.Append("**Cool:** ").Append(cardObject.Minimum_statistics_cool).Append(" - ").Append(cardObject.Non_idolized_maximum_statistics_cool).AppendLine();
            }
            return cardDescription.ToString();
        }
    }
}
