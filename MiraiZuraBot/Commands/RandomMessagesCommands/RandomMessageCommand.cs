using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Database;
using MiraiZuraBot.Database.Models.DynamicDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.RandomMessagesCommands
{
    [CommandsGroup("Tekst")]
    class RandomMessageCommand : BaseCommandModule
    {
        private string AqoursNewsString = "AqoursNews";

        [Command("AqoursNews")]
        [Description("Pokazuje zmyśloną informacje o Aqours.")]
        public async Task AqoursNews(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            using (var databaseContext = new DynamicDBContext())
            {
                Random rnd = new Random();
                string message = "";
                for(int i = 1; ; i++)
                {
                    List<RandomMessage> messages = databaseContext.RandomMessages.Where(p => p.MessageGroup == AqoursNewsString && p.MessagePart == i).ToList();
                    if(messages.Count == 0)
                    {
                        break;
                    }

                    int index = rnd.Next(0, messages.Count);
                    message += messages[index].Message;
                    message += " ";
                }
                await ctx.RespondAsync(message);
            }
        }
    }
}
