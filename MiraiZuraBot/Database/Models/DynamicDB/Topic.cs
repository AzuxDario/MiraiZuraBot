using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB
{
    class Topic
    {
        public int ID { get; set; }
        public int Name { get; set; }

        public virtual List<Channel> Channels { get; set; }
        public virtual List<Birthday> Informations { get; set; }
    }
}
