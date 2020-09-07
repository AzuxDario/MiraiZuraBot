using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Containers.Schoolidolu;
using MiraiZuraBot.Containers.Schoolidolu.Idols;
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
                await PostEmbedHelper.PostEmbed(ctx, ctx.RawArgumentString, description, idolObject.Data.Chibi, SchoolidoluHelper.GetSchoolidoluFotter());
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
                await PostEmbedHelper.PostEmbed(ctx, ctx.RawArgumentString, description, idolsResponse.Data.Results[0].Chibi, SchoolidoluHelper.GetSchoolidoluFotter());
            }
            else
            {
                await ctx.RespondAsync("Wystąpił błąd.");
            }
        }

        private string MakeIdolDescription(IdolObject cardObject)
        {
            StringBuilder idolDescription = new StringBuilder();
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Imie:** ", cardObject.Name);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Japońskie imie:** ", cardObject.Japanese_name);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Wiek:** ", cardObject.Age);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Szkoła:** ", cardObject.School);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Urodziny (MM-dd):** ", cardObject.Birthday);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Znak zodiaku:** ", cardObject.Astrological_sign);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Grupa krwi:** ", cardObject.Blood);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Wzrost:** ", cardObject.Height);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Wymiary: ** ", cardObject.Measurements);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Ulubione jedzenie: ** ", cardObject.Favorite_food);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Nielubiane jedzenie: ** ", cardObject.Least_favorite_food);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Hobby: ** ", cardObject.Hobbies);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Atrybut: ** ", cardObject.Attribute);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Rok: ** ", cardObject.Year);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Main unit: ** ", cardObject.Main_unit);
            SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Sub unit: ** ", cardObject.Sub_unit);
            if(cardObject.Cv != null)
            {
                SchoolidoluHelper.AddLineToStringBuilder(idolDescription, "**Seiyuu: ** ", cardObject.Cv.Name);
            }

            return idolDescription.ToString();
        }

        
    }
}
