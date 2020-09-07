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
        public List<string> BirthdayRoles { get; }
        public int BirthdayChannelId { get; }
        public int BirthdayId { get; }
        public string Filename { get; }
        public BirthdayChannelsResponse(ulong channelId, ulong serverId, string content, List<string> birthdayRoles, int birthdayChannelId, int birthdayId, string filename)
        {
            ChannelId = channelId;
            ServerId = serverId;
            Content = content;
            BirthdayRoles = birthdayRoles;
            BirthdayChannelId = birthdayChannelId;
            BirthdayId = birthdayId;
            Filename = filename;
        }
    }
}
