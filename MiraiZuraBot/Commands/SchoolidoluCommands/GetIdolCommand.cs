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
        private SchoolidoluHelper _schoolidoluHelper;

        public GetIdolCommand(SchoolidoluService schoolidoluService, SchoolidoluHelper schoolidoluHelper)
        {
            _schoolidoluService = schoolidoluService;
            _schoolidoluHelper = schoolidoluHelper;
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
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Imie:** ", cardObject.Name);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Japońskie imie:** ", cardObject.Japanese_name);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Wiek:** ", cardObject.Age);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Szkoła:** ", cardObject.School);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Urodziny (MM-dd):** ", cardObject.Birthday);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Znak zodiaku:** ", cardObject.Astrological_sign);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Grupa krwi:** ", cardObject.Blood);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Wzrost:** ", cardObject.Height);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Wymiary: ** ", cardObject.Measurements);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Ulubione jedzenie: ** ", cardObject.Favorite_food);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Nielubiane jedzenie: ** ", cardObject.Least_favorite_food);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Hobby: ** ", cardObject.Hobbies);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Atrybut: ** ", cardObject.Attribute);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Rok: ** ", cardObject.Year);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Main unit: ** ", cardObject.Main_unit);
            _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Sub unit: ** ", cardObject.Sub_unit);
            if(cardObject.Cv != null)
            {
                _schoolidoluHelper.AddTitledLineToStringBuilder(idolDescription, "**Seiyuu: ** ", cardObject.Cv.Name);
            }

            return idolDescription.ToString();
        }

        
    }
}
