﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB
{
    class Channel
    {
        public int ID { get; set; }
        public string ChannelID { get; set; }

        public virtual List<PostedBirthday> PostedInformations { get; set; }

        public int ServerID { get; set; }
        public virtual Server Server { get; set; }

        public int TopicID { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
