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

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "random" },
                { "page_size", "1" }
            };

            var idolsResponse = _schoolidoluService.GetIdol(options);

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
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Imie:** ", cardObject.Name);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Japońskie imie:** ", cardObject.Japanese_name);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Wiek:** ", cardObject.Age);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Szkoła:** ", cardObject.School);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Urodziny (MM-dd):** ", cardObject.Birthday);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Znak zodiaku:** ", cardObject.Astrological_sign);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Grupa krwi:** ", cardObject.Blood);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Wzrost:** ", cardObject.Height);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Wymiary: ** ", cardObject.Measurements);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Ulubione jedzenie: ** ", cardObject.Favorite_food);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Nielubiane jedzenie: ** ", cardObject.Least_favorite_food);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Hobby: ** ", cardObject.Hobbies);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Atrybut: ** ", cardObject.Attribute);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Rok: ** ", cardObject.Year);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Main unit: ** ", cardObject.Main_unit);
            SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Sub unit: ** ", cardObject.Sub_unit);
            if(cardObject.Cv != null)
            {
                SchoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Seiyuu: ** ", cardObject.Cv.Name);
            }

            return idolDescription.ToString();
        }

        
    }
}
