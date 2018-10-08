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

namespace MiraiZuraBot.Commands.AnnouncementCommands
{
    [CommandsGroup("Powiadamianie")]
    class AnnouncementCommand : BaseCommandModule
    {
        private Timer checkMessagesTimer;
        private int checkMessagesInterval;

        public AnnouncementCommand()
        {
            checkMessagesInterval = 1000 * 60 * 60;    // every hour
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
                    DateTime today = DateTime.Now;

                    // Get topic id for this channel
                    int topicId = channel.Topic.ID;

                    // Get all messages for today for this topic
                    List<Information> dbInformations = databaseContext.Informations.Where(p => p.Topic.ID == topicId && p.Day == today.Day && p.Month == today.Month).ToList();

                    foreach(Information information in dbInformations)
                    {
                        // If information was already posted there will be data
                        List<PostedInformation> dbPostedInformations = databaseContext.PostedInformations.Where(p => p.Information.ID == information.ID && p.Day == information.Day &&
                                                                                                                p.Month == information.Month && p.Year == today.Year 
                                                                                                                && p.Channel.ID == channel.ID).ToList();

                        if (dbPostedInformations.Count == 0)
                        {
                            try
                            {
                                // There was no such information
                                ulong id;
                                ulong.TryParse(channel.ChannelID, out id);
                                DiscordChannel discordChannel = await Bot.DiscordClient.GetChannelAsync(id);
                                DiscordMessage discordMessage = await discordChannel.SendMessageAsync(information.Content);

                                // If message was sent add info to database
                                if (discordMessage.Content != null)
                                {
                                    PostedInformation postedInformation = new PostedInformation();
                                    postedInformation.Day = today.Day;
                                    postedInformation.Month = today.Month;
                                    postedInformation.Year = today.Year;
                                    postedInformation.InformationID = information.ID;
                                    postedInformation.ChannelID = channel.ID;

                                    databaseContext.PostedInformations.Add(postedInformation);
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
