using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Containers.Schoolidolu.Idols;
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
    class GetIdolCommand : BaseCommandModule
    {
        private EmbedFooter footer = new EmbedFooter { Text = "Powered by schoolido.lu", IconUrl = "https://i.schoolido.lu/android/icon.png" };

        [Command("idolka")]
        [Description("Pokazuje idolkę na bazie jej nazwy.\nnp:\n*idolka Watanabe You \n*idolka Sonoda Umi")]
        public async Task Idol(CommandContext ctx, [Description("Imie postaci.")] params string[] name)
        {
            await ctx.TriggerTypingAsync();

            var client = new HttpClient();
            IdolObject idolObject;

            var response = client.GetAsync("http://schoolido.lu/api/idols/" + ctx.RawArgumentString + "/").Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                idolObject = JsonConvert.DeserializeObject<IdolObject>(response.Content.ReadAsStringAsync().Result);

                string description = MakeIdolDescription(idolObject);
                await PostEmbedHelper.PostEmbed(ctx, ctx.RawArgumentString, description, idolObject.Chibi, footer);
            }
            else
            {
                await ctx.RespondAsync("Podana idolka nie istnieje.");
            }
        }

        [Command("losowaIdolka")]
        [Description("Pokazuje losową idolkę.\nnp:\n*losowaIdolka")]
        public async Task RandomIdol(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var client = new HttpClient();
            IdolsResponse idolsResponse;

            var response = client.GetAsync("http://schoolido.lu/api/idols/" + ctx.RawArgumentString + "/").Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                idolsResponse = JsonConvert.DeserializeObject<IdolsResponse>(response.Content.ReadAsStringAsync().Result);

                string description = MakeIdolDescription(idolsResponse.Results[0]);
                await PostEmbedHelper.PostEmbed(ctx, ctx.RawArgumentString, description, idolsResponse.Results[0].Chibi, footer);
            }
            else
            {
                await ctx.RespondAsync("Wystąpił błąd.");
            }
        }

        public string MakeIdolDescription(IdolObject cardObject)
        {
            StringBuilder idolDescription = new StringBuilder();
            idolDescription.Append("**Imie:** ").Append(cardObject.Name).AppendLine();
            idolDescription.Append("**Japońskie imie:** ").Append(cardObject.Japanese_name).AppendLine();
            idolDescription.Append("**Wiek:** ").Append(cardObject.Age).AppendLine();
            idolDescription.Append("**Szkoła:** ").Append(cardObject.School).AppendLine();
            idolDescription.Append("**Urodziny (MM-dd):** ").Append(cardObject.Birthday).AppendLine();
            idolDescription.Append("**Znak zodiaku:** ").Append(cardObject.Astrological_sign).AppendLine();
            idolDescription.Append("**Grupa krwi:** ").Append(cardObject.Blood).AppendLine();
            idolDescription.Append("**Wzrost:** ").Append(cardObject.Height).AppendLine();
            idolDescription.Append("**Wymiary: ** ").Append(cardObject.Measurements).AppendLine();
            idolDescription.Append("**Ulubione jedzenie: ** ").Append(cardObject.Favorite_food).AppendLine();
            idolDescription.Append("**Nielubiane jedzenie: ** ").Append(cardObject.Least_favorite_food).AppendLine();
            idolDescription.Append("**Hobby: ** ").Append(cardObject.Hobbies).AppendLine();
            idolDescription.Append("**Atrybut: ** ").Append(cardObject.Attribute).AppendLine();
            idolDescription.Append("**Rok: ** ").Append(cardObject.Year).AppendLine();
            idolDescription.Append("**Main unit: ** ").Append(cardObject.Main_unit).AppendLine();
            idolDescription.Append("**Sub unit: ** ").Append(cardObject.Sub_unit).AppendLine();
            idolDescription.Append("**Seiyuu: ** ").Append(cardObject.Cv.Name).AppendLine();

            return idolDescription.ToString();
        }
    }
}
