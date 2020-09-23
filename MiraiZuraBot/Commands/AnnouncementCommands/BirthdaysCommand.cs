using DSharpPlus.CommandsNext;
using MiraiZuraBot.Attributes;
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
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Services.AnnouncementService;
using MiraiZuraBot.Translators;
using MiraiZuraBot.Services.LanguageService;

namespace MiraiZuraBot.Commands.AnnouncementCommands
{
    [GroupLang("Powiadamianie", "Announcing")]
    class BirthdaysCommand : BaseCommandModule
    {
        private BirthdaysService _birthdaysService;
        private LanguageService _languageService;
        private Translator _translator;
        private Timer checkMessagesTimer;
        private int checkMessagesInterval;
        private const string imageDirectory = "birthdays/";

        public BirthdaysCommand(BirthdaysService birthdaysService, LanguageService languageService, Translator translator)
        {
            _birthdaysService = birthdaysService;
            _languageService = languageService;
            _translator = translator;            
            checkMessagesInterval = 1000 * 60 * 1;    // every 1 minutes;
            checkMessagesTimer = new Timer(PostBirthdayMessage, null, checkMessagesInterval, Timeout.Infinite);
        }

        [Command("tematyUrodzin")]
        [Aliases("birthdayTopics")]
        [CommandLang("tematyUrodzin", "birthdayTopics")]
        [DescriptionLang("Wyświetla możliwe tematy urodzin.", "Shows available bitrhday topics")]
        public async Task BirthdayTopics(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            var topics = _birthdaysService.GetBirthdayTopics();

            await PostLongMessageHelper.PostLongMessage(ctx, topics, _translator.GetString(lang, "birthdaysAvailableTopics"), ", ");
        }

        [Command("aktywneTematyUrodzin")]
        [Aliases("activeBirthdayTopics")]
        [CommandLang("aktywneTematyUrodzin", "activeBirthdayTopics")]
        [DescriptionLang("Wyświetla aktywne tematy urodzin dla danego kanału.", "Shows active bitrhday topics on given channel.")]
        [RequireBotPermissions(Permissions.ManageGuild)]
        [RequireUserPermissions(Permissions.ManageGuild)]
        public async Task ActiveBirthdayTopics(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            string channelId;
            channelId = ctx.Channel.Id.ToString();

            var topics = _birthdaysService.GetActiveBirthdayTopicsForChannel(Convert.ToUInt64(channelId));
            if (topics.Count > 0)
            {
                await PostLongMessageHelper.PostLongMessage(ctx, topics, _translator.GetString(lang, "birthdaysActiveTopics"), ", ");
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "birthdays"), _translator.GetString(lang, "birthdaysNoActiveTopics"));
            }
        }

        private async void PostBirthdayMessage(object state)
        {
            var channels = _birthdaysService.GetChannelsForPostingBirthdays();
            foreach (var channel in channels)
            {
                try
                {
                    // There was no such posted information  
                    DiscordChannel discordChannel = await Bot.DiscordClient.GetChannelAsync(channel.ChannelId);
                    DiscordMessage discordMessage = null;

                    string brithdayRolesMention = GetRolesMention(channel.ServerId, channel.BirthdayRoles, channel.MentionEveryone);

                    if (channel.Filename != null)
                    {
                        try
                        {
                            using (StreamReader reader = new StreamReader(imageDirectory + channel.Filename))
                            {
                                string name = channel.Filename.Split('/').Last();
                                var buffer = new byte[reader.BaseStream.Length];
                                reader.BaseStream.Read(buffer, 0, (int)reader.BaseStream.Length);
                                var memStream = new MemoryStream(buffer);

                                discordMessage = await discordChannel.SendFileAsync(name, memStream, brithdayRolesMention + channel.Content);
                            }
                        }
                        catch (FileNotFoundException)
                        {
                            discordMessage = await discordChannel.SendMessageAsync(brithdayRolesMention + channel.Content);
                        }
                        catch (DirectoryNotFoundException)
                        {
                            discordMessage = await discordChannel.SendMessageAsync(brithdayRolesMention + channel.Content);
                        }
                    }
                    else
                    {
                        discordMessage = await discordChannel.SendMessageAsync(brithdayRolesMention + channel.Content);
                    }

                    // If message was sent add info to database
                    // For now just add everytime
                    // INFO: check error with task cancel during image posting
                    // This was about size of image. Problalby bigger image caused timeout
                    //if (discordMessage != null)
                    {
                        _birthdaysService.AcknowledgementForPostingBirthdays(channel.BirthdayId, channel.BirthdayChannelId);
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

            checkMessagesTimer.Change(checkMessagesInterval, Timeout.Infinite);
            return;
        }

        [Command("wlaczTematUrodzin")]
        [Aliases("turnOnBirthdayTopic")]
        [CommandLang("wlaczTematUrodzin", "turnOnBirthdayTopic")]
        [DescriptionLang("Włącza temat urodzin dla danego kanału.", "Turn on bitrhday topic on given channel.")]
        [RequireBotPermissions(Permissions.ManageGuild)]
        [RequireUserPermissions(Permissions.ManageGuild)]
        public async Task TurnOnBirthdayTopic(CommandContext ctx, [DescriptionLang("Temat", "Topic"), RemainingText] string topicName)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            var result = _birthdaysService.TurnOnBirthdayTopic(ctx.Guild.Id, ctx.Channel.Id, topicName);

            switch(result)
            {
                case BirthdaysService.TurnOnStatus.TurnedOn:
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "birthdays"), _translator.GetString(lang, "birthdaysTurnedOn"));
                    return;
                case BirthdaysService.TurnOnStatus.AlreadyTurnedOn:
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "birthdays"), _translator.GetString(lang, "birthdaysAlreadyTurnedOn"));
                    return;
                case BirthdaysService.TurnOnStatus.TopicDoesntExist:
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "birthdays"), _translator.GetString(lang, "birthdaysTopicDoesntExist"));
                    return;
            }
        }

        [Command("wylaczTematUrodzin")]
        [Aliases("turnOffBirthdayTopic")]
        [CommandLang("wylaczTematUrodzin", "turnOffBirthdayTopic")]
        [DescriptionLang("Wyłącza temat urodzin dla danego kanału.", "Turn off bitrhday topic on given channel.")]
        [RequireBotPermissions(Permissions.ManageGuild)]
        [RequireUserPermissions(Permissions.ManageGuild)]
        public async Task TurnOffBirthdayTopic(CommandContext ctx, [DescriptionLang("Temat", "Topic"), RemainingText] string topicName)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            var result = _birthdaysService.TurnOffBirthdayTopic(ctx.Guild.Id, ctx.Channel.Id, topicName);

            switch (result)
            {
                case BirthdaysService.TurnOffStatus.TurnedOff:
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "birthdays"), _translator.GetString(lang, "birthdaysTurnedOff"));
                    return;
                case BirthdaysService.TurnOffStatus.AlreadyTurnedOff:
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "birthdays"), _translator.GetString(lang, "birthdaysAlreadyTurnedOff"));
                    return;
                case BirthdaysService.TurnOffStatus.TopicDoesntExist:
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "birthdays"), _translator.GetString(lang, "birthdaysTopicDoesntExist"));
                    return;
            }
        }

        private string GetRolesMention(ulong serverID, List<ulong> birthdayRoles, bool mentionEveryone)
        {
            StringBuilder roles = new StringBuilder();
            DiscordGuild server = Bot.DiscordClient.GetGuildAsync(serverID).Result;
            var serverRoles = server.Roles;
            if (mentionEveryone == true)
            {
                roles.Append("@everyone ");
            }
            foreach (var birthdayRole in birthdayRoles)
            {
                DiscordRole role = serverRoles.FirstOrDefault(p => p.Value.Id == birthdayRole).Value;
                if (role != null)
                {
                    roles.Append(role.Mention);
                    roles.Append(" ");
                }
            }

            return roles.ToString();
        }
    }

}
