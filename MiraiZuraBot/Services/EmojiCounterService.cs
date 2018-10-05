using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MiraiZuraBot.Services
{
    class EmojiCounterService
    {
        public async void countEmojiInMessage(MessageCreateEventArgs e)
        {
            if (e.Author.IsCurrent == false)
            {
                if (e.Message.Content.Contains(":") == true)
                {
                    IReadOnlyList<DiscordGuildEmoji> emojiList;
                    emojiList = await e.Guild.GetEmojisAsync();

                    string wholeMessage = "";
                    foreach (DiscordGuildEmoji emoji in emojiList)
                    {
                        int emojiCount = 0;                        
                        string emojiName = ":" + emoji.Name + ":";

                        if (e.Message.Content.Contains(emojiName) == true)
                        {
                            emojiCount = Regex.Matches(e.Message.Content, emojiName).Count;
                            string message = "<" + emojiName + emoji.Id + ">" + " użyto: " + emojiCount + " id: " + emoji.Id + "\n";
                            wholeMessage += message;
                        }       
                    }
                    if (wholeMessage.Length != 0)
                    {
                        await e.Channel.SendMessageAsync(wholeMessage);
                    }
                }
            }
        }
    }
}
