using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB
{
    class Server
    {
        public Server()
        {
            Language = Translator.Language.Polish;
        }
        public Server(ulong id)
        {
            ServerID = id.ToString();
            Language = Translator.Language.Polish;
        }
        public Server(ulong id, Translator.Language language)
        {
            ServerID = id.ToString();
            Language = language;
        }

        public int ID { get; set; }
        public string ServerID { get; set; }
        public Translator.Language Language { get; set; }

        public virtual List<Emoji> Emojis { get; set; }
        public virtual List<BirthdayChannel> BirthdayChannels { get; set; }
        public virtual List<AssignRole> AssignRoles { get; set; }
    }
}
