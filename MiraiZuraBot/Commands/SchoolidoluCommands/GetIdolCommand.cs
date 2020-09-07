using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Containers.Schoolidolu;
using MiraiZuraBot.Containers.Schoolidolu.Idols;
using MiraiZuraBot.Helpers;
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
    class GetIdolCommand : BaseCommandModule
    {
        private SchoolidoluService _schoolidoluService;

        public GetIdolCommand(SchoolidoluService schoolidoluService)
        {
            _schoolidoluService = schoolidoluService;
        }

        [Command("idolka")]
        [Description("Pokazuje idolkę na bazie jej nazwy.\nnp:\n*idolka Watanabe You \n*idolka Sonoda Umi")]
        public async Task Idol(CommandContext ctx, [Description("Imie postaci."), RemainingText] string name)
        {
            await ctx.TriggerTypingAsync();

            var idolObject = _schoolidoluService.GetIdolByName(name);

            if (idolObject.StatusCode == HttpStatusCode.OK)
            {
                string description = MakeIdolDescription(idolObject.Data);
                await PostEmbedHelper.PostEmbed(ctx, ctx.RawArgumentString, description, idolObject.Data.Chibi, PostEmbedHelper.GetSchoolidoluFotter());
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

            var idolsResponse = _schoolidoluService.GetRandomIdol();

            if (idolsResponse.StatusCode == HttpStatusCode.OK)
            {
                 string description = MakeIdolDescription(idolsResponse.Data.Results[0]);
                await PostEmbedHelper.PostEmbed(ctx, ctx.RawArgumentString, description, idolsResponse.Data.Results[0].Chibi, PostEmbedHelper.GetSchoolidoluFotter());
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
            if(field != null && field.ToString() != "")
            {
                builder.Append(description).Append(field).AppendLine();
            }
        }
    }
}
