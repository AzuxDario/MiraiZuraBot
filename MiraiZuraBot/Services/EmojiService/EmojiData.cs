using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Services.EmojiService
{
    class EmojiData
    {
        public ulong EmojiID { get; set; }
        public int UsageCount { get; set; }

        public EmojiData( ulong id, int count)
        {
            EmojiID = id;
            UsageCount = count;
        }
    }
}
