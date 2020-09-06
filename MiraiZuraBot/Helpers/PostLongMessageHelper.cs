using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Helpers
{
    class PostLongMessageHelper
    {
        public static async Task PostLongMessage(CommandContext ctx, List<string> strings, string header = null)
        {
            StringBuilder response = new StringBuilder(2000);
            if(header != null)
            {
                response.Append(header);
                response.AppendLine();
            }
            foreach (string s in strings)
            {
                response.Append(s);

                if (response.Length > 1800)
                {
                    await ctx.RespondAsync(response.ToString());
                    response.Clear();
                    if (header != null)
                    {
                        response.Append(header);
                        response.AppendLine();
                    }
                    continue;
                }
                response.Append(", ");
            }
            if (response.Length > 0)
            {
                await ctx.RespondAsync(response.ToString());
                return;
            }
        }
    }
}
