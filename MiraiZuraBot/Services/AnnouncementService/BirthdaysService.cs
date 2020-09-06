using Microsoft.EntityFrameworkCore;
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
        public enum TurnOnStatus { turnedOn, alreadyTurnedOn, topicDoesntExist };
        public enum TurnOffStatus { turnedOff, alreadyTurnedOff, topicDoesntExist };

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

        public TurnOnStatus TurnOnBirthdayTopic(string serverId, string channelId, string topicName)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                // Check if topic exist
                Topic topic = databaseContext.Topics.Where(p => p.Name == topicName).FirstOrDefault();
                if (topic == null)
                {
                    return TurnOnStatus.topicDoesntExist;
                }

                // Check if this channel and topic has enter in database
                BirthdayChannel birthdayChannel = databaseContext.BirthdayChannels.Include(p => p.Topic).Where(p => p.ChannelID == channelId && p.Topic.Name == topicName).FirstOrDefault();
                if (birthdayChannel != null)
                {
                    // Entry exist
                    if (birthdayChannel.IsEnabled == true)
                    {
                        return TurnOnStatus.alreadyTurnedOn;
                    }
                    else
                    {
                        birthdayChannel.IsEnabled = true;
                        databaseContext.SaveChanges();
                        return TurnOnStatus.turnedOn;
                    }
                }
                else
                {
                    //Check if server exist
                    Server server = databaseContext.Servers.Where(p => p.ServerID == serverId).FirstOrDefault();
                    if (server == null)
                    {
                        server = new Server
                        {
                            ServerID = serverId
                        };
                    }

                    // Create new entry
                    BirthdayChannel newChannel = new BirthdayChannel
                    {
                        Server = server,
                        ChannelID = channelId,
                        IsEnabled = true,
                        Topic = topic
                    };
                    databaseContext.Add(newChannel);
                    databaseContext.SaveChanges();
                    return TurnOnStatus.turnedOn;
                }

            }
        }

        public TurnOffStatus TurnOffBirthdayTopic(string serverId, string channelId, string topicName)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                // Check if topic exist
                Topic topic = databaseContext.Topics.Where(p => p.Name == topicName).FirstOrDefault();
                if (topic == null)
                {
                    return TurnOffStatus.topicDoesntExist;
                }

                // Check if this channel and topic has enter in database
                BirthdayChannel birthdayChannel = databaseContext.BirthdayChannels.Include(p => p.Topic).Where(p => p.ChannelID == channelId && p.Topic.Name == topicName).FirstOrDefault();
                if (birthdayChannel != null)
                {
                    // Entry exist
                    if (birthdayChannel.IsEnabled == false)
                    {
                        return TurnOffStatus.alreadyTurnedOff;
                    }
                    else
                    {
                        birthdayChannel.IsEnabled = false;
                        databaseContext.SaveChanges();
                        return TurnOffStatus.turnedOff;
                    }
                }
                else
                {
                    // Entry doesn't exist
                    return TurnOffStatus.alreadyTurnedOff;
                }

            }
        }
    }
}
