using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB
{
    class PostedInformation
    {
        public int ID { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }

        public int ChannelID { get; set; }
        public virtual Channel Channel { get; set; }

        public int InformationID { get; set; }
        public virtual Information Information { get; set; }
    }
}
