using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB
{
    class Birthday
    {
        public int ID { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }

        public virtual List<PostedBirthday> PostedBirthdays { get; set; }

        public int TopicID { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
