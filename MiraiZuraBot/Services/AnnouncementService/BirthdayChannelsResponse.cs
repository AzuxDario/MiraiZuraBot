using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Services.AnnouncementService
{
    class BirthdayChannelsResponse
    {
        public ulong ChannelId { get; }
        public ulong ServerId { get; }
        public string Content { get; }
        public List<ulong> BirthdayRoles { get; }
        public bool MentionEveryone { get; }
        public int BirthdayChannelId { get; }
        public int BirthdayId { get; }
        public string Filename { get; }
        public BirthdayChannelsResponse(ulong channelId, ulong serverId, string content, List<ulong> birthdayRoles, bool mentionEveryone, int birthdayChannelId, int birthdayId, string filename)
        {
            ChannelId = channelId;
            ServerId = serverId;
            Content = content;
            BirthdayRoles = birthdayRoles;
            MentionEveryone = mentionEveryone;
            BirthdayChannelId = birthdayChannelId;
            BirthdayId = birthdayId;
            Filename = filename;
        }
    }
}
