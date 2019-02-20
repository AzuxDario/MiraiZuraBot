using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB
{
    class BirthdayRole
    {
        public BirthdayRole() { }
        public BirthdayRole(ulong id)
        {
            RoleID = id.ToString();
        }
        public int ID { get; set; }
        public string RoleID { get; set; }

        public int BirthdayChannelID { get; set; }
        public virtual BirthdayChannel BirthdayChannel { get; set; }
    }
}
