using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Services.RandomMessagesService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.RandomMessagesCommands
{
    [GroupLang("Tekst", "Text")]
    class RandomMessageCommand : BaseCommandModule
    {
        private RandomMessageService _randomMessageService;

        public RandomMessageCommand(RandomMessageService randomMessageService)
        {
            _randomMessageService = randomMessageService;
        }

        [Command("AqoursNews")]
        [CommandLang("AqoursNews", "AqoursNews")]
        [DescriptionLang("Pokazuje zmyśloną informacje o Aqours.", "It shows fake news about Aqours.")]
        public async Task AqoursNews(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync(_randomMessageService.GetAqoursNews());
        }
    }
}
