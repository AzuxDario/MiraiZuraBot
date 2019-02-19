using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB
{
    class Topic
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual List<BirthdayChannel> BirthdayChannels { get; set; }
        public virtual List<Birthday> Birthdays { get; set; }
    }
}
