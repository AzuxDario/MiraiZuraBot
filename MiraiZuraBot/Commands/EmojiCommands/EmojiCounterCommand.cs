using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Services.EmojiService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.EmojiCommands
{
    [CommandsGroup("Emoji")]
    class EmojiCounterCommand : BaseCommandModule
    {
        private EmojiCounterService _emojiCounterService;

        public EmojiCounterCommand(EmojiCounterService emojiCounterService)
        {
            _emojiCounterService = emojiCounterService;
        }

        [Command("policzEmoji")]
        [Description("Zlicza emoji użyte do tej pory na serwerze podczas działania bota.")]
        public async Task CountEmoji(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            List<EmojiData> emojiData = _emojiCounterService.GetEmojiData(ctx.Guild.Id);

            if (emojiData.Count != 0)
            {
                IReadOnlyList<DiscordGuildEmoji> serverEmojiList;
                // List for emojis to print
                List<EmojiHolder> emojiHolderList = new List<EmojiHolder>();
                // Get server emoji
                serverEmojiList = await ctx.Guild.GetEmojisAsync();

                foreach (DiscordGuildEmoji emoji in serverEmojiList)
                {
                    if(emoji.IsAnimated == true)
                    {
                        continue;
                    }

                    EmojiData tempEmoji = emojiData.Where(p => p.EmojiID == emoji.Id).FirstOrDefault();
                            
                    if(tempEmoji != null)
                    {
                        EmojiHolder emojiHolder = new EmojiHolder(emoji.Name, emoji.Id, tempEmoji.UsageCount);
                        emojiHolderList.Add(emojiHolder);
                    }
                    else
                    {
                        EmojiHolder emojiHolder = new EmojiHolder(emoji.Name, emoji.Id, 0);
                        emojiHolderList.Add(emojiHolder);
                    }
                }

                string wholeMessage = "";
                emojiHolderList = emojiHolderList.OrderByDescending(p => p.UsageCount).ToList();

                foreach (EmojiHolder emoji in emojiHolderList)
                {
                    wholeMessage += emoji.GetEmojiToSend() + "\n";
                    if(wholeMessage.Length > 1800)
                    {
                        await ctx.RespondAsync(wholeMessage);
                        wholeMessage = "";
                    }
                }

                if (wholeMessage != null)
                {
                    await ctx.RespondAsync(wholeMessage);
                    return;
                }

            }

            await ctx.RespondAsync("Na tym serwerze jeszcze nie użyto emoji.");
        }
    }

    public class EmojiHolder
    {
        public string EmojiName { get; set; }
        public ulong EmojiID { get; set; }
        public int UsageCount { get; set; }

        public EmojiHolder(string name, ulong id, int count)
        {
            EmojiName = name;
            EmojiID = id;
            UsageCount = count;
        }

        public string GetEmojiToSend()
        {
            return "<:" + EmojiName + ":" + EmojiID + "> użyto: " + UsageCount;
        }
    }
}
