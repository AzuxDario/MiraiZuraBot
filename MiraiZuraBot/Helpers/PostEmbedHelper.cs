using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MiraiZuraBot.Helpers
{
    class PostEmbedHelper
    {
        public static async Task PostEmbed(CommandContext ctx, string title = null, string description = null, string imageLink = null, string thumbnailLink = null, DiscordEmbedBuilder.EmbedFooter footer = null,
            string color = null)
        {
            // Discord can't handle links with japanese characters
            if (imageLink != null)
            {
                imageLink = imageLink.Replace(" ", "%20");
                int pos = imageLink.LastIndexOf("/");
                string toChange = imageLink.Substring(pos);
                string changed = HttpUtility.UrlEncode(toChange, Encoding.UTF8);
                imageLink = imageLink.Remove(pos);
                imageLink += changed;
            }
            if(thumbnailLink != null)
            {
                thumbnailLink = thumbnailLink.Replace(" ", "%20");
                int posThumbnail = thumbnailLink.LastIndexOf("/");
                string toChangeThumbnail = imageLink.Substring(posThumbnail);
                string changedThumbnail = HttpUtility.UrlEncode(toChangeThumbnail, Encoding.UTF8);
                thumbnailLink = thumbnailLink.Remove(posThumbnail);
                thumbnailLink += changedThumbnail;
            }

            

            var embed = new DiscordEmbedBuilder
            {
                ImageUrl = imageLink,
                ThumbnailUrl = thumbnailLink,
                Description = description,
                Title = title,
                Footer = footer
            };

            if (color == null)
            {
                embed.Color = new DiscordColor("#00a8ff");
            }
            else
            {
                embed.Color = new DiscordColor(color);
            }

            await ctx.RespondAsync(null, false, embed);
        }
    }
}   
