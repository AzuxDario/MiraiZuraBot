using MiraiZuraBot.Database;
using MiraiZuraBot.Database.Models.DynamicDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiraiZuraBot.Services.RandomMessages
{
    class RandomMessageService
    {
        private readonly string AqoursNewsString = "AqoursNews";

        public string GetAqoursNews()
        {
            using (var databaseContext = new DynamicDBContext())
            {
                Random rnd = new Random();
                string message = "";
                for (int i = 1; ; i++)
                {
                    List<RandomMessage> messages = databaseContext.RandomMessages.Where(p => p.MessageGroup == AqoursNewsString && p.MessagePart == i).ToList();
                    if (messages.Count == 0)
                    {
                        break;
                    }

                    int index = rnd.Next(0, messages.Count);
                    message += messages[index].Message;
                    message += " ";
                }
                return message;
            }
        }
    }
}
