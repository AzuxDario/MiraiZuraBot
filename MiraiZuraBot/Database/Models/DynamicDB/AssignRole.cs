﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB
{
    class AssignRole
    {
        public AssignRole() { }
        public AssignRole(ulong id)
        {
            RoleID = id.ToString();
        }
        public int ID { get; set; }
        public string RoleID { get; set; }

        public int ServerID { get; set; }
        public virtual Server Server { get; set; }
    }
}
