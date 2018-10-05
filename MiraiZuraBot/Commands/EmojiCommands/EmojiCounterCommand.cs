using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Database;
using MiraiZuraBot.Database.Models.DynamicDB;
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
        [Command("policzEmoji")]
        [Description("Zlicza emoji użyte do tej pory na serwerze.")]
        public async Task CountEmoji(CommandContext ctx)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = databaseContext.Servers.Where(p => p.ServerID == ctx.Guild.Id.ToString()).Include(p => p.Emojis).FirstOrDefault();
                if (dbServer != null)
                {
                    if (dbServer.Emojis != null)
                    {
                        List<Emoji> emojis = dbServer.Emojis.OrderBy(p => p.UsageCount).ToList();
                        string wholeMessage = "";

                        foreach (Emoji emoji in emojis)
                        {
                            try
                            {
                                DiscordGuildEmoji discordEmoji = await ctx.Guild.GetEmojiAsync(ulong.Parse(emoji.EmojiID));
                                string message = "<:" + discordEmoji.Name + ":" + discordEmoji.Id + ">" + " użyto: " + emoji.UsageCount + " razy \n";
                                wholeMessage += message;
                            }
                            catch
                            {
                                //dupa
                            }
                        }
                        if (wholeMessage != null)
                        {
                            await ctx.RespondAsync(wholeMessage);
                        }

                        return;
                    }
                }

                await ctx.RespondAsync("Na tym serwerze jeszcze nie użyto emoji.");
            }
        }
    }
}
