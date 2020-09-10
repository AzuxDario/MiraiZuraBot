using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Helpers
{
    class PostEmbedHelper
    {
        public static async Task PostEmbed(CommandContext ctx, string title = null, string description = null, string imageLink = null, string thumbnailLink = null, DiscordEmbedBuilder.EmbedFooter footer = null)
        {
            if (imageLink != null)
            {
                imageLink = imageLink.Replace(" ", "%20");
            }
            if(thumbnailLink != null)
            {
                thumbnailLink = thumbnailLink.Replace(" ", "%20");
            }
            var embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor("#00a8ff"),
                ImageUrl = imageLink,
                ThumbnailUrl = thumbnailLink,
                Description = description,
                Title = title,
                Footer = footer
            };
            await ctx.RespondAsync(null, false, embed);
        }

    }
}   
