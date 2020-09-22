using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Services.EmojiService;
using MiraiZuraBot.Services.LanguageService;
using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.EmojiCommands
{
    [GroupLang("Emoji", "Emoji")]
    class EmojiCounterCommand : BaseCommandModule
    {
        private EmojiCounterService _emojiCounterService;
        private LanguageService _languageService;
        private Translator _translator;

        public EmojiCounterCommand(EmojiCounterService emojiCounterService, LanguageService languageService, Translator translator)
        {
            _emojiCounterService = emojiCounterService;
            _languageService = languageService;
            _translator = translator;
        }

        [Command("policzEmoji")]
        [Description("Zlicza emoji użyte do tej pory na serwerze podczas działania bota.")]
        public async Task CountEmoji(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

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

                emojiHolderList = emojiHolderList.OrderByDescending(p => p.UsageCount).ToList();

                await PostLongMessageHelper.PostLongMessage(ctx, emojiHolderList.Select(p => p.GetEmojiToSend()).ToList(), _translator.GetString(lang, "emojiUsage"));
                return;
            }

            await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "emojiUsage"), _translator.GetString(lang, "emojiNotUsage"));
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
            return string.Format("<:{0}:{1}> - {2}\n", EmojiName, EmojiID, UsageCount);
        }
    }
}
