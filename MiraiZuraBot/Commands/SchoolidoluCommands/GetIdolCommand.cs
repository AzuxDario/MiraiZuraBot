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
                string description = _schoolidoluHelper.MakeIdolDescription(idolObject.Data);
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
                string description = _schoolidoluHelper.MakeIdolDescription(idolsResponse.Data.Results[0]);
                await PostEmbedHelper.PostEmbed(ctx, ctx.RawArgumentString, description, idolsResponse.Data.Results[0].Chibi, SchoolidoluHelper.GetSchoolidoluFotter());
            }
            else
            {
                await ctx.RespondAsync("Wystąpił błąd.");
            }
        }       
    }
}
