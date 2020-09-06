using MiraiZuraBot.Database;
using MiraiZuraBot.Database.Models.DynamicDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiraiZuraBot.Services.AnnouncementService
{
    class BirthdaysService
    {
        public List<string> GetBirthdayTopics()
        {
            using (var databaseContext = new DynamicDBContext())
            {
                List<Topic> dbTopics = databaseContext.Topics.ToList();
                return dbTopics.Select(p => p.Name).ToList();
            }
        }

        public List<string> GetActiveBirthdayTopicsForChannel(string channelId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                List<Topic> dbTopics = databaseContext.Topics.Where(p => p.BirthdayChannels.Any(m => m.ChannelID == channelId && m.IsEnabled == true)).ToList();
                return dbTopics.Select(p => p.Name).ToList();
            }
        }
    }
}
