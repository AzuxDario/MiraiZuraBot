﻿using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiraiZuraBot.Handlers.EmojiHandlers
{
    class EmojiAddHandler
    {
        private Random random;

        public EmojiAddHandler()
        {
            random = new Random();
        }

        public async Task AddEmojiOnServer(DiscordGuild guild, DiscordMessage message)
        {
            if (message.Author.IsCurrent == true)
            {
                return;
            }
            if(guild.Id == 414480832317751297)
            {
                if (Regex.Matches(message.Content, " (nusz|nóż)[.,\\- ]|^(nusz|nóż)[.,\\- ]|^(nusz|nóż)$| (nusz|nóż)$", RegexOptions.IgnoreCase).Count > 0)
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
                if (Regex.Matches(message.Content, " (nusz|nóż)[.,\\- ]|^(nusz|nóż)[.,\\- ]|^(nusz|nóż)$| (nusz|nóż)$", RegexOptions.IgnoreCase).Count > 0)
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
            try
            {
                await message.CreateReactionAsync(emoji);
            }
            catch (Exception ie)
            {
                Console.WriteLine("Error: Cannot add emoji.");
                Console.WriteLine("Exception: " + ie.Message);
                Console.WriteLine("Inner Exception: " + ie?.InnerException?.Message);
                Console.WriteLine("Stack trace: " + ie.StackTrace);
            }
        }
    }
}
