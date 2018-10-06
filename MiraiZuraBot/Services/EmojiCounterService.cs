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
        public async void countEmojiInMessage(DiscordMessage message)
        {
            if (message.Author.IsCurrent == false)
            {
                if (message.Content.Contains(":") == true)
                {
                    using (var databaseContext = new DynamicDBContext())
                    {
                        if (!databaseContext.Servers.Any(p => p.ServerID == message.Channel.Guild.Id.ToString()))
                        {
                            Server dbServer = new Server();
                            dbServer.ServerID = message.Channel.Guild.Id.ToString();
                            databaseContext.Servers.Add(dbServer);
                            databaseContext.SaveChanges();
                        }
                    }

                    IReadOnlyList<DiscordGuildEmoji> serverEmojiList;
                    serverEmojiList = await message.Channel.Guild.GetEmojisAsync();

                    foreach (DiscordGuildEmoji serverEmoji in serverEmojiList)
                    {
                        int emojiCount = 0;
                        string emojiName = ":" + serverEmoji.Name + ":";

                        if (message.Content.Contains(emojiName) == true)
                        {
                            emojiCount = Regex.Matches(message.Content, emojiName).Count;

                            using (var databaseContext = new DynamicDBContext())
                            {
                                Server dbServer = databaseContext.Servers.Where(p => p.ServerID == message.Channel.Guild.Id.ToString()).FirstOrDefault();
                                if (!databaseContext.Emojis.Any(p => p.EmojiID == serverEmoji.Id.ToString()))
                                {
                                    Emoji dbEmoji = new Emoji();
                                    dbEmoji.EmojiID = serverEmoji.Id.ToString();
                                    dbEmoji.UsageCount = emojiCount;
                                    dbEmoji.ServerID = dbServer.ID;
                                    databaseContext.Emojis.Add(dbEmoji);
                                    databaseContext.SaveChanges();
                                }
                                else
                                {
                                    Emoji dbEmoji = databaseContext.Emojis.Where(p => p.EmojiID == serverEmoji.Id.ToString()).FirstOrDefault();
                                    dbEmoji.UsageCount += emojiCount;
                                    databaseContext.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
        }

        public async void countEmojiReaction(DiscordEmoji emoji, DiscordChannel channel)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                if (!databaseContext.Servers.Any(p => p.ServerID == channel.Guild.Id.ToString()))
                {
                    Server dbServer = new Server();
                    dbServer.ServerID = channel.Guild.Id.ToString();
                    databaseContext.Servers.Add(dbServer);
                    databaseContext.SaveChanges();
                }
            }

            IReadOnlyList<DiscordGuildEmoji> serverEmojiList;
            serverEmojiList = await channel.Guild.GetEmojisAsync();

            foreach (DiscordGuildEmoji serverEmoji in serverEmojiList)
            {
                if (emoji == serverEmoji)
                {
                    using (var databaseContext = new DynamicDBContext())
                    {
                        Server dbServer = databaseContext.Servers.Where(p => p.ServerID == channel.Guild.Id.ToString()).FirstOrDefault();
                        if (!databaseContext.Emojis.Any(p => p.EmojiID == serverEmoji.Id.ToString()))
                        {
                            Emoji dbEmoji = new Emoji();
                            dbEmoji.EmojiID = serverEmoji.Id.ToString();
                            dbEmoji.UsageCount = 1;
                            dbEmoji.ServerID = dbServer.ID;
                            databaseContext.Emojis.Add(dbEmoji);
                            databaseContext.SaveChanges();
                        }
                        else
                        {
                            Emoji dbEmoji = databaseContext.Emojis.Where(p => p.EmojiID == serverEmoji.Id.ToString()).FirstOrDefault();
                            dbEmoji.UsageCount += 1;
                            databaseContext.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
