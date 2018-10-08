using DSharpPlus.CommandsNext;
using MiraiZuraBot.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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

        private async void RefreshCurrentSongMessages(object state)
        {
            return;
        }
    }
}
