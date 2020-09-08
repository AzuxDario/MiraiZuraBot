using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Helpers.SchoolidoluHelper
{
    class SchoolidoluHelper
    {
        public static DiscordEmbedBuilder.EmbedFooter GetSchoolidoluFotter()
        {
            return new DiscordEmbedBuilder.EmbedFooter { Text = "Powered by schoolido.lu", IconUrl = "https://i.schoolido.lu/android/icon.png" };
        }

        public static void AddLineToStringBuilder<T>(StringBuilder builder, string description, T field)
        {
            if (field != null && field.ToString() != "")
            {
                builder.Append(description).Append(field).AppendLine();
            }
        }

        public static void AddLineToStringBuilder(StringBuilder builder, string description, DateTime? field)
        {
            if (field != null)
            {
                builder.Append(description).Append(field.Value.ToString("HH:mm dd.MM.yyyy")).AppendLine();
            }
        }

        public static void AddUrlToStringBuilder(StringBuilder builder, string description, string linkName, string url)
        {
            if (url != null && url.ToString() != "")
            {
                builder.Append(description).Append("[").Append(linkName).Append("](").Append(url).Append(")").AppendLine();
            }
        }
    }
}
