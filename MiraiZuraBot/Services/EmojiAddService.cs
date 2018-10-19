﻿using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MiraiZuraBot.Services
{
    class EmojiAddService
    {
        private Random random;

        public EmojiAddService()
        {
            random = new Random();
        }

        public async void AddEmojiOnServer(DiscordGuild guild, DiscordMessage message)
        {
            if (message.Author.IsCurrent == true)
            {
                return;
            }
            if(guild.Id == 414480832317751297)
            {
                if (Regex.Matches(message.Content, "nusz").Count + Regex.Matches(message.Content, "noż").Count > 0)
                {
                    IReadOnlyList<DiscordGuildEmoji> serverEmojiList;
                    serverEmojiList = await message.Channel.Guild.GetEmojisAsync();

                    foreach (DiscordEmoji serverEmoji in serverEmojiList)
                    {
                        if (serverEmoji.Name == "yousoro")
                        {
                            AddEmoji(message, serverEmoji);
                        }
                    }
                }
            }
            else if(guild.Id == 291868072531329024)
            {
                if (Regex.Matches(message.Content, "nusz", RegexOptions.IgnoreCase).Count + Regex.Matches(message.Content, "nóż", RegexOptions.IgnoreCase).Count > 0)
                {
                    IReadOnlyList<DiscordGuildEmoji> serverEmojiList;
                    serverEmojiList = await message.Channel.Guild.GetEmojisAsync();

                    List<DiscordEmoji> emojiList = new List<DiscordEmoji>();

                    foreach (DiscordEmoji serverEmoji in serverEmojiList)
                    {
                        if (Regex.Matches(serverEmoji.Name, "nusz").Count != 0)
                        {
                            emojiList.Add(serverEmoji);
                        }
                    }

                    var emojiIndex = random.Next(0, emojiList.Count);
                    AddEmoji(message, emojiList[emojiIndex]);
                    return;
                }
            }
        }

        private async void AddEmoji(DiscordMessage message, DiscordEmoji emoji)
        {
            await message.CreateReactionAsync(emoji);  
        }
    }
}
