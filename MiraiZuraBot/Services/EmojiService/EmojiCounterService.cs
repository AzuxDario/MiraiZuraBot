using Microsoft.EntityFrameworkCore;
using MiraiZuraBot.Database;
using MiraiZuraBot.Database.Models.DynamicDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiraiZuraBot.Services.EmojiService
{
    class EmojiCounterService
    {
        public List<EmojiData> GetEmojiData(ulong serverId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                List<EmojiData> emojis = new List<EmojiData>();
                Server dbServer = databaseContext.Servers.Where(p => p.ServerID == serverId.ToString()).Include(p => p.Emojis).FirstOrDefault();
                if (dbServer != null)
                {
                    // Get emoji from database
                    List<Emoji> dbEmojis = dbServer.Emojis.OrderBy(p => p.UsageCount).ToList();

                    foreach(Emoji emoji in dbEmojis)
                    {
                        emojis.Add(new EmojiData(Convert.ToUInt64(emoji.EmojiID), emoji.UsageCount));
                    }
                }
                return emojis;
            }
        }
    }
}
