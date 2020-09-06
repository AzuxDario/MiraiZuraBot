﻿using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Helpers
{
    class PostEmbedHelper
    {
        public static async Task PostEmbed(CommandContext ctx, string title = null, string description = null, string imageLink = null, DiscordEmbedBuilder.EmbedFooter footer = null)
        {
            var embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor("#00a8ff"),
                ImageUrl = imageLink,
                Description = description,
                Title = title,
                Footer = footer
            };
            await ctx.RespondAsync(null, false, embed);
        }

        public static DiscordEmbedBuilder.EmbedFooter GetSchoolidoluFotter()
        {
            return new DiscordEmbedBuilder.EmbedFooter { Text = "Powered by schoolido.lu", IconUrl = "https://i.schoolido.lu/android/icon.png" };
        }
    }
}   
