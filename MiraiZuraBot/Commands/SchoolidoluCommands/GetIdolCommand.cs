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

            var response = client.GetAsync("http://schoolido.lu/api/idols/?ordering=random&page_size=1").Result;
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

        private string MakeIdolDescription(IdolObject cardObject)
        {
            StringBuilder idolDescription = new StringBuilder();
            AddLineToIdolDescription(idolDescription, "**Imie:** ", cardObject.Name);
            AddLineToIdolDescription(idolDescription, "**Japońskie imie:** ", cardObject.Japanese_name);
            AddLineToIdolDescription(idolDescription, "**Wiek:** ", cardObject.Age);
            AddLineToIdolDescription(idolDescription, "**Szkoła:** ", cardObject.School);
            AddLineToIdolDescription(idolDescription, "**Urodziny (MM-dd):** ", cardObject.Birthday);
            AddLineToIdolDescription(idolDescription, "**Znak zodiaku:** ", cardObject.Astrological_sign);
            AddLineToIdolDescription(idolDescription, "**Grupa krwi:** ", cardObject.Blood);
            AddLineToIdolDescription(idolDescription, "**Wzrost:** ", cardObject.Height);
            AddLineToIdolDescription(idolDescription, "**Wymiary: ** ", cardObject.Measurements);
            AddLineToIdolDescription(idolDescription, "**Ulubione jedzenie: ** ", cardObject.Favorite_food);
            AddLineToIdolDescription(idolDescription, "**Nielubiane jedzenie: ** ", cardObject.Least_favorite_food);
            AddLineToIdolDescription(idolDescription, "**Hobby: ** ", cardObject.Hobbies);
            AddLineToIdolDescription(idolDescription, "**Atrybut: ** ", cardObject.Attribute);
            AddLineToIdolDescription(idolDescription, "**Rok: ** ", cardObject.Year);
            AddLineToIdolDescription(idolDescription, "**Main unit: ** ", cardObject.Main_unit);
            AddLineToIdolDescription(idolDescription, "**Sub unit: ** ", cardObject.Sub_unit);
            if(cardObject.Cv != null)
            {
                AddLineToIdolDescription(idolDescription, "**Seiyuu: ** ", cardObject.Cv.Name);
            }

            return idolDescription.ToString();
        }

        private void AddLineToIdolDescription<T>(StringBuilder builder, string description, T field)
        {
            if(field != null)
            {
                builder.Append(description).Append(field).AppendLine();
            }
        }
    }
}
