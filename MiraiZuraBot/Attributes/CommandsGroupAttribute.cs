using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    class CommandsGroupAttribute : Attribute
    {
        public string Group { get; }

        public CommandsGroupAttribute(string group)
        {
            Group = group;
        }
    }
}
