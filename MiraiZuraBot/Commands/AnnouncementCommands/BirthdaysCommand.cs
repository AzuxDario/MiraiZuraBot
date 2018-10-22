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

namespace MiraiZuraBot.Commands.AnnouncementCommands
{
    [CommandsGroup("Powiadamianie")]
    class BirthdaysCommand : BaseCommandModule
    {
        private Timer checkMessagesTimer;
        private int checkMessagesInterval;

        public BirthdaysCommand()
        {
            checkMessagesInterval = 1000 * 60 * 1;    // every 1 minutes;
            checkMessagesTimer = new Timer(RefreshCurrentSongMessages, null, checkMessagesInterval, Timeout.Infinite);
        }

        [Command("Dummy")]
        public async Task Dummy(CommandContext ctx)
        {

        }

        private async void RefreshCurrentSongMessages(object state)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                // Get all channels to post message
                List<Channel> dbChannels = databaseContext.Channels.ToList();

                foreach(Channel channel in dbChannels)
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
                    DateTime todayJapan = TimeZoneInfo.ConvertTimeFromUtc(todayUTC, japanTimeZone);

                    // Get topic id for this channel
                    int topicId = channel.TopicID;

                    // Get all messages for today for this topic
                    List<Birthday> dbBirthdays = databaseContext.Birthdays.Where(p => p.TopicID == topicId && p.Day == todayJapan.Day && p.Month == todayJapan.Month).ToList();

                    foreach(Birthday birthday in dbBirthdays)
                    {
                        // If information was already posted there will be data
                        List<PostedBirthday> dbPostedInformations = databaseContext.PostedBirthdays.Where(p => p.Birthdays.ID == birthday.ID && p.Day == birthday.Day &&
                                                                                                                p.Month == birthday.Month && p.Year == todayJapan.Year 
                                                                                                                && p.Channel.ID == channel.ID).ToList();

                        if (dbPostedInformations.Count == 0)
                        {
                            try
                            {
                                // There was no such information
                                ulong id;
                                ulong.TryParse(channel.ChannelID, out id);
                                DiscordChannel discordChannel = await Bot.DiscordClient.GetChannelAsync(id);
                                var embed = new DiscordEmbedBuilder
                                {
                                    Color = new DiscordColor("#5588EE"),
                                    ImageUrl = birthday.ImageLink
                                };
                                DiscordMessage discordMessage = await discordChannel.SendMessageAsync(birthday.Content, false, embed);

                                // If message was sent add info to database
                                if (discordMessage != null)
                                {
                                    PostedBirthday postedInformation = new PostedBirthday();
                                    postedInformation.Day = todayJapan.Day;
                                    postedInformation.Month = todayJapan.Month;
                                    postedInformation.Year = todayJapan.Year;
                                    postedInformation.BirthdayID = birthday.ID;
                                    postedInformation.ChannelID = channel.ID;

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
    }
}
