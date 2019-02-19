using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB
{
    class PostedBirthday
    {
        public int ID { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public int BirthdayChannelID { get; set; }
        public virtual BirthdayChannel BirthdayChannel { get; set; }

        public int BirthdayID { get; set; }
        public virtual Birthday Birthdays { get; set; }
    }
}
