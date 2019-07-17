using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Containers.Schoolidolu.Cards;
using MiraiZuraBot.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static DSharpPlus.Entities.DiscordEmbedBuilder;

namespace MiraiZuraBot.Commands.SchoolidoluCommands
{
    [CommandsGroup("SIF")]
    class GetCardsCommand : BaseCommandModule
    {
        private EmbedFooter footer = new EmbedFooter { Text = "Powered by schoolido.lu", IconUrl = "https://i.schoolido.lu/android/icon.png" };

        [Command("karta")]
        [Description("Pokazuje karte na bazie jej id.\nnp:\n*karta 1599\n*karta 1599 idolizowana")]
        public async Task Card(CommandContext ctx, [Description("ID karty.")] string id, [Description("Czy idolizowana.")] string isIdolised = null)
        {
            await ctx.TriggerTypingAsync();

            var client = new HttpClient();
            CardObject cardObject;

            var response = client.GetAsync("http://schoolido.lu/api/cards/" + id + "/").Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                cardObject = JsonConvert.DeserializeObject<CardObject>(response.Content.ReadAsStringAsync().Result);

                if (isIdolised == "idolizowana")
                {
                    string description = MakeCardDescription(cardObject, true);
                    await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardObject.Id + " : " + cardObject.Idol.Name, description, "http:" + cardObject.Card_idolized_image, footer);
                }
                else
                {
                    string description = MakeCardDescription(cardObject, false);
                    await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardObject.Id + " : " + cardObject.Idol.Name, description, "http:" + cardObject.Card_image, footer);
                }
            }
            else
            {
                await ctx.RespondAsync("Podana karta nie istnieje.");
            }
        }

        [Command("losowaKarta")]
        [Description("Pokazuje losową karte. Można sprecyzować imie idolki.\nnp:\n*losowaKarta\n*losowaKarta Watanabe You")]
        public async Task RandomCard(CommandContext ctx, [Description("Imie idolki.")] params string[] name)
        {
            await ctx.TriggerTypingAsync();

            var client = new HttpClient();
            CardsResponse cardsResponse;

            var response = client.GetAsync("https://schoolido.lu/api/cards/?name=" + ctx.RawArgumentString + "&ordering=random&page_size=1").Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                cardsResponse = JsonConvert.DeserializeObject<CardsResponse>(response.Content.ReadAsStringAsync().Result);

                string description = MakeCardDescription(cardsResponse.Results[0], false);
                await PostEmbedHelper.PostEmbed(ctx, "Karta " + cardsResponse.Results[0].Id + " : " + cardsResponse.Results[0].Idol.Name, description, "http:" + cardsResponse.Results[0].Card_image, footer);
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
