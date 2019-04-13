using DSharpPlus.CommandsNext;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Database;
using MiraiZuraBot.Database.Models.DynamicDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using MiraiZuraBot.Core;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using DSharpPlus;
using Microsoft.EntityFrameworkCore;

namespace MiraiZuraBot.Commands.AnnouncementCommands
{
    [CommandsGroup("Powiadamianie")]
    class BirthdaysCommand : BaseCommandModule
    {
        private Timer checkMessagesTimer;
        private int checkMessagesInterval;
        private const string imageDirectory = "birthdays/";

        public BirthdaysCommand()
        {
            checkMessagesInterval = 1000 * 60 * 1;    // every 1 minutes;
            checkMessagesTimer = new Timer(PostBirthdayMessage, null, checkMessagesInterval, Timeout.Infinite);
        }

        [Command("tematyUrodzin")]
        [Description("Wyświetla możliwe tematy urodzin.")]
        public async Task BirthdayTopics(CommandContext ctx)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                List<Topic> dbTopics = databaseContext.Topics.ToList();
                string response = "Dostępne tematy urodzin:\n";
                foreach(Topic topic in dbTopics)
                {
                    response += topic.Name;
                    response += "\n";
                    if (response.Length > 1800)
                    {
                        await ctx.RespondAsync(response);
                        response = "";
                    }   
                }
                if (response != string.Empty)
                {
                    await ctx.RespondAsync(response);
                    return;
                }
            }
        }

        [Command("aktywneTematyUrodzin")]
        [Description("Wyświetla aktywne tematy urodzin dla danego kanału.")]
        [RequirePermissions(Permissions.ManageGuild)]
        public async Task ActiveBirthdayTopics(CommandContext ctx, DiscordChannel channel = null)
        {
            string channelId;
            if(channel == null)
            {
                channelId = ctx.Channel.Id.ToString();
            }
            else
            {
                channelId = channel.Id.ToString();
            }

            using (var databaseContext = new DynamicDBContext())
            {
                List<Topic> dbTopics = databaseContext.Topics.Where(p => p.BirthdayChannels.Any(m => m.ChannelID == channelId && m.IsEnabled == true )).ToList();
                if(dbTopics.Count > 0)
                {
                    string response = "Tematy urodzin włączone na tym kanale:\n";
                    foreach (Topic topic in dbTopics)
                    {
                        response += topic.Name;
                        response += "\n";
                        if (response.Length > 1800)
                        {
                            await ctx.RespondAsync(response);
                            response = "";
                        }
                    }
                    if (response != string.Empty)
                    {
                        await ctx.RespondAsync(response);
                        return;
                    }
                }
                else
                {
                    await ctx.RespondAsync("Dla tego kanału nie ma włączonych żadnych tematów urodzin.");
                }
            }
        }

        private async void PostBirthdayMessage(object state)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                // Get all channels to post message
                List<BirthdayChannel> dbChannels = databaseContext.BirthdayChannels.Include(p => p.BirthdayRoles).Include(q => q.Server).ToList();

                foreach(BirthdayChannel channel in dbChannels)
                {
                    // If channel is disabled go to next one
                    if (channel.IsEnabled == false)
                    {
                        continue;
                    }

                    DateTime todayJapan = GetCurrentJapanTime();

                    // Get topic id for this channel
                    int topicId = channel.TopicID;

                    // Get all roles for this topic in this channel
                    string rolesMention = GetRolesMention(channel.Server.ServerID, channel.BirthdayRoles);

                    // Get all messages for today for this topic
                    List<Birthday> dbBirthdays = databaseContext.Birthdays.Where(p => p.TopicID == topicId && p.Day == todayJapan.Day && p.Month == todayJapan.Month).ToList();

                    foreach(Birthday birthday in dbBirthdays)
                    {
                        // If information was already posted there will be data
                        List<PostedBirthday> dbPostedInformations = databaseContext.PostedBirthdays.Where(p => p.Birthdays.ID == birthday.ID && p.Day == birthday.Day &&
                                                                                                                p.Month == birthday.Month && p.Year == todayJapan.Year 
                                                                                                                && p.BirthdayChannel.ID == channel.ID).ToList();

                        if (dbPostedInformations.Count == 0)
                        {
                            try
                            {
                                // There was no such posted information  
                                ulong id;
                                ulong.TryParse(channel.ChannelID, out id);
                                DiscordChannel discordChannel = await Bot.DiscordClient.GetChannelAsync(id);
                                DiscordMessage discordMessage = null;


                                if (birthday.FileName != null)
                                {
                                    using (StreamReader reader = new StreamReader(imageDirectory + birthday.FileName))
                                    {
                                        string name = birthday.FileName.Split('/').Last();

                                        discordMessage = await discordChannel.SendFileAsync(name, reader.BaseStream, rolesMention + birthday.Content);
                                        
                                    }                                    
                                }
                                else
                                {
                                    discordMessage = await discordChannel.SendMessageAsync(rolesMention + birthday.Content);
                                }

                                // If message was sent add info to database
                                if (discordMessage != null)
                                {
                                    PostedBirthday postedInformation = new PostedBirthday();
                                    postedInformation.Day = todayJapan.Day;
                                    postedInformation.Month = todayJapan.Month;
                                    postedInformation.Year = todayJapan.Year;
                                    postedInformation.BirthdayID = birthday.ID;
                                    postedInformation.BirthdayChannelID = channel.ID;

                                    databaseContext.PostedBirthdays.Add(postedInformation);
                                    databaseContext.SaveChanges();
                                }

                            }
                            catch (Exception ie)
                            {
                                Console.WriteLine("Error: Cannot post planned message.");
                                Console.WriteLine("Exception: " + ie.Message);
                                Console.WriteLine("Inner Exception: " + ie?.InnerException?.Message);
                                Console.WriteLine("Stack trace: " + ie.StackTrace);
                            }
                        }
                    }
                }
            }

            checkMessagesTimer.Change(checkMessagesInterval, Timeout.Infinite);
            return;
        }

        private DateTime GetCurrentJapanTime()
        {
            DateTime todayUTC = DateTime.UtcNow;
            TimeZoneInfo japanTimeZone;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                japanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            }
            else
            {
                japanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Japan");
            }
            return TimeZoneInfo.ConvertTimeFromUtc(todayUTC, japanTimeZone);
        }

        private string GetRolesMention(string serverID, List<BirthdayRole> birthdayRoles)
        {
            StringBuilder roles = new StringBuilder();
            DiscordGuild server = Bot.DiscordClient.GetGuildAsync(ulong.Parse(serverID)).Result;
            var serverRoles = server.Roles;
            foreach(var birthdayRole in birthdayRoles)
            {
                if(birthdayRole.RoleID == "everyone")
                {
                    roles.Append("@everyone ");
                }
                else
                {
                    DiscordRole role = serverRoles.FirstOrDefault(p => p.Id.ToString() == birthdayRole.RoleID);
                    if (role != null)
                    {
                        roles.Append(role.Mention);
                        roles.Append(" ");
                    }
                }
            }

            return roles.ToString();
        }
    }

}
