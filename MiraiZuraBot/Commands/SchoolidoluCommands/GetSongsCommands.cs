using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Helpers.SchoolidoluHelper;
using MiraiZuraBot.Services.SchoolidoluService;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.SchoolidoluCommands
{
    [CommandsGroup("SIF")]
    class GetSongsCommands : BaseCommandModule
    {
        private SchoolidoluService _schoolidoluService;
        private SchoolidoluHelper _schoolidoluHelper;

        public GetSongsCommands(SchoolidoluService schoolidoluService, SchoolidoluHelper schoolidoluHelper)
        {
            _schoolidoluService = schoolidoluService;
            _schoolidoluHelper = schoolidoluHelper;
        }

        [Command("losowaPiosenka")]
        [Description("Pokazuje losową piosenkę.\nnp:\n*losowaPiosenka")]
        public async Task RandomSong(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            Dictionary<string, string> options = new Dictionary<string, string>
            {
                { "ordering", "random" },
                { "page_size", "1" }
            };

            var songsResponse = _schoolidoluService.GetSong(options);

            if (songsResponse.StatusCode == HttpStatusCode.OK)
            {
                await PostEmbedHelper.PostEmbed(ctx, "Losowa piosenka", _schoolidoluHelper.MakeSongDescription(songsResponse.Data.Results[0]),
                    songsResponse.Data.Results[0].Image != null ? "https:" + songsResponse.Data.Results[0].Image : null, null, SchoolidoluHelper.GetSchoolidoluFotter());
            }
            else
            {
                await ctx.RespondAsync("Wystąpił błąd.");
            }
        }
    }
}
