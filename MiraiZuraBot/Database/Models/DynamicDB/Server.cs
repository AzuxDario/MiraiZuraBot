using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB
{
    class Server
    {
        public int ID { get; set; }
        public string ServerID { get; set; }

        public virtual List<Emoji> Emojis { get; set; }
    }
}
