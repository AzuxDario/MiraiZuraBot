﻿using Microsoft.EntityFrameworkCore;
using MiraiZuraBot.Database;
using MiraiZuraBot.Database.Models.DynamicDB;
using MiraiZuraBot.Helpers.TimeHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MiraiZuraBot.Services.AnnouncementService
{
    class BirthdaysService
    {
        public enum TurnOnStatus { TurnedOn, AlreadyTurnedOn, TopicDoesntExist };
        public enum TurnOffStatus { TurnedOff, AlreadyTurnedOff, TopicDoesntExist };

        private TimeHelper _timeHelper;

        public BirthdaysService(TimeHelper timeHelper)
        {
            _timeHelper = timeHelper;
        }

        public List<string> GetBirthdayTopics()
        {
            using (var databaseContext = new DynamicDBContext())
            {
                List<Topic> dbTopics = databaseContext.Topics.ToList();
                return dbTopics.Select(p => p.Name).ToList();
            }
        }

        public List<string> GetActiveBirthdayTopicsForChannel(ulong channelId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                List<Topic> dbTopics = databaseContext.Topics.Where(p => p.BirthdayChannels.Any(m => m.ChannelID == channelId.ToString() && m.IsEnabled == true)).ToList();
                return dbTopics.Select(p => p.Name).ToList();
            }
        }

        public TurnOnStatus TurnOnBirthdayTopic(ulong serverId, ulong channelId, string topicName)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                // Check if topic exist
                Topic topic = databaseContext.Topics.Where(p => p.Name == topicName).FirstOrDefault();
                if (topic == null)
                {
                    return TurnOnStatus.TopicDoesntExist;
                }

                // Check if this channel and topic has enter in database
                BirthdayChannel birthdayChannel = databaseContext.BirthdayChannels.Include(p => p.Topic).Where(p => p.ChannelID == channelId.ToString() && p.Topic.Name == topicName).FirstOrDefault();
                if (birthdayChannel != null)
                {
                    // Entry exist
                    if (birthdayChannel.IsEnabled == true)
                    {
                        return TurnOnStatus.AlreadyTurnedOn;
                    }
                    else
                    {
                        birthdayChannel.IsEnabled = true;
                        databaseContext.SaveChanges();
                        return TurnOnStatus.TurnedOn;
                    }
                }
                else
                {
                    //Check if server exist
                    Server server = databaseContext.Servers.Where(p => p.ServerID == serverId.ToString()).FirstOrDefault();
                    if (server == null)
                    {
                        server = new Server
                        {
                            ServerID = serverId.ToString()
                        };
                    }

                    // Create new entry
                    BirthdayChannel newChannel = new BirthdayChannel
                    {
                        Server = server,
                        ChannelID = channelId.ToString(),
                        IsEnabled = true,
                        Topic = topic
                    };
                    databaseContext.Add(newChannel);
                    databaseContext.SaveChanges();
                    return TurnOnStatus.TurnedOn;
                }

            }
        }

        public TurnOffStatus TurnOffBirthdayTopic(ulong serverId, ulong channelId, string topicName)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                // Check if topic exist
                Topic topic = databaseContext.Topics.Where(p => p.Name == topicName).FirstOrDefault();
                if (topic == null)
                {
                    return TurnOffStatus.TopicDoesntExist;
                }

                // Check if this channel and topic has enter in database
                BirthdayChannel birthdayChannel = databaseContext.BirthdayChannels.Include(p => p.Topic).Where(p => p.ChannelID == channelId.ToString() && p.Topic.Name == topicName).FirstOrDefault();
                if (birthdayChannel != null)
                {
                    // Entry exist
                    if (birthdayChannel.IsEnabled == false)
                    {
                        return TurnOffStatus.AlreadyTurnedOff;
                    }
                    else
                    {
                        birthdayChannel.IsEnabled = false;
                        databaseContext.SaveChanges();
                        return TurnOffStatus.TurnedOff;
                    }
                }
                else
                {
                    // Entry doesn't exist
                    return TurnOffStatus.AlreadyTurnedOff;
                }

            }
        }

        public List<BirthdayChannelsResponse> GetChannelsForPostingBirthdays()
        {
            List<BirthdayChannelsResponse> channelsForMessage = new List<BirthdayChannelsResponse>();
            using (var databaseContext = new DynamicDBContext())
            {
                // Get all channels to post message
                List<BirthdayChannel> dbChannels = databaseContext.BirthdayChannels.Include(p => p.BirthdayRoles).Include(q => q.Server).ToList();

                foreach (BirthdayChannel channel in dbChannels)
                {
                    // If channel is disabled go to next one
                    if (channel.IsEnabled == false)
                    {
                        continue;
                    }

                    DateTime todayJapan = _timeHelper.GetCurrentJapanTime();
                    bool mentionEveryone = false;

                    // Get topic id for this channel
                    int topicId = channel.TopicID;

                    // Get all roles for this topic in this channel
                    List<string> dbRoles = channel.BirthdayRoles.Select(p => p.RoleID).ToList();
                    List<ulong> rolesMention = new List<ulong>();
                    foreach(string dbRole in dbRoles)
                    {
                        if(dbRole == "everyone")
                        {
                            mentionEveryone = true;
                        }
                        else
                        {
                            rolesMention.Add(Convert.ToUInt64(dbRole));
                        }
                    }

                    // Get all messages for today for this topic
                    List<Birthday> dbBirthdays = databaseContext.Birthdays.Where(p => p.TopicID == topicId && p.Day == todayJapan.Day && p.Month == todayJapan.Month).ToList();

                    foreach (Birthday birthday in dbBirthdays)
                    {
                        // If information was already posted there will be data
                        List<PostedBirthday> dbPostedInformations = databaseContext.PostedBirthdays.Where(p => p.Birthdays.ID == birthday.ID && p.Day == birthday.Day &&
                                                                                                                p.Month == birthday.Month && p.Year == todayJapan.Year
                                                                                                                && p.BirthdayChannel.ID == channel.ID).ToList();

                        if (dbPostedInformations.Count == 0)
                        {
                            // There was no such posted information  
                            BirthdayChannelsResponse channelForMessage = new BirthdayChannelsResponse(Convert.ToUInt64(channel.ChannelID), Convert.ToUInt64(channel.Server.ServerID),
                                                                                                      birthday.Content, rolesMention, mentionEveryone, channel.ID, birthday.ID, birthday.FileName);
                            channelsForMessage.Add(channelForMessage);
                        }
                    }
                }
            }
            return channelsForMessage;
        }

        public void AcknowledgementForPostingBirthdays(int birthdayId, int birthdayChannelId)
        {
            DateTime todayJapan = _timeHelper.GetCurrentJapanTime();
            using (var databaseContext = new DynamicDBContext())
            {
                PostedBirthday postedInformation = new PostedBirthday
                {
                    Day = todayJapan.Day,
                    Month = todayJapan.Month,
                    Year = todayJapan.Year,
                    BirthdayID = birthdayId,
                    BirthdayChannelID = birthdayChannelId
                };

                databaseContext.PostedBirthdays.Add(postedInformation);
                databaseContext.SaveChanges();
            }
        }
    }
}
