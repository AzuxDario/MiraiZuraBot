using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB
{
    class RandomMessage
    {
        public int ID { get; set; }
        public string MessageGroup { get; set; }
        public int MessagePart { get; set; }
        public string Message { get; set; }
    }
}
