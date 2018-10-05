using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB
{
    class Emoji
    {
        public int ID { get; set; }
        public string EmojiID { get; set; }
        public int UsageCount { get; set; }

        public int ServerID { get; set; }
        public virtual Server Server { get; set; }
    }
}
