using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MiraiZuraBot.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using MiraiZuraBot.Database.Models.DynamicDB;

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
                    using (var databaseContext = new DynamicDBContext())
                    {
                        if (!databaseContext.Servers.Any(p => p.ServerID == e.Guild.Id.ToString()))
                        {
                            Server dbServer = new Server();
                            dbServer.ServerID = e.Guild.Id.ToString();
                            databaseContext.Servers.Add(dbServer);
                            databaseContext.SaveChanges();
                        }
                    }

                    IReadOnlyList<DiscordGuildEmoji> emojiList;
                    emojiList = await e.Guild.GetEmojisAsync();

                    foreach (DiscordGuildEmoji emoji in emojiList)
                    {
                        int emojiCount = 0;
                        string emojiName = ":" + emoji.Name + ":";

                        if (e.Message.Content.Contains(emojiName) == true)
                        {
                            emojiCount = Regex.Matches(e.Message.Content, emojiName).Count;

                            using (var databaseContext = new DynamicDBContext())
                            {
                                Server dbServer = databaseContext.Servers.Where(p => p.ServerID == e.Guild.Id.ToString()).FirstOrDefault();
                                if (!databaseContext.Emojis.Any(p => p.EmojiID == emoji.Id.ToString()))
                                {
                                    Emoji dbEmoji = new Emoji();
                                    dbEmoji.EmojiID = emoji.Id.ToString();
                                    dbEmoji.UsageCount = emojiCount;
                                    dbEmoji.ServerID = dbServer.ID;
                                    databaseContext.Emojis.Add(dbEmoji);
                                    databaseContext.SaveChanges();
                                }
                                else
                                {
                                    Emoji dbEmoji = databaseContext.Emojis.Where(p => p.EmojiID == emoji.Id.ToString()).FirstOrDefault();
                                    dbEmoji.UsageCount += emojiCount;
                                    databaseContext.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
