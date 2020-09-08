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
                builder.Append(description).Append(" (").Append(field).Append(")").AppendLine();
            }
        }

        public static void AddTitledLineToStringBuilder<T>(StringBuilder builder, string description, T field)
        {
            if (field != null && field.ToString() != "")
            {
                builder.Append(description).AppendLine().Append(field).AppendLine();
            }
        }

        public static void AddTitledLineToStringBuilder<T, U>(StringBuilder builder, string description, T field, U field2)
        {
            if (field != null && field.ToString() != "")
            {
                builder.Append(description).AppendLine().Append(field);
            }
            if (field2 != null && field2.ToString() != "")
            {
                builder.Append(" (").Append(field2).Append(")");
            }
            builder.AppendLine();
        }

        public static void AddDataTimeToStringBuilder(StringBuilder builder, string description, DateTime? field)
        {
            if (field != null)
            {
                builder.Append(description).AppendLine().Append(field.Value.ToString("HH:mm dd.MM.yyyy"));
            }
        }

        public static void AddDateTimeToStringBuilder(StringBuilder builder, string description, DateTime? field, DateTime? field2)
        {
            if (field != null)
            {
                builder.Append(description).AppendLine().Append(field.Value.ToString("HH:mm dd.MM.yyyy"));
            }
            if (field2 != null)
            {
                builder.Append(" - ").Append(field2.Value.ToString("HH:mm dd.MM.yyyy"));
            }
            builder.AppendLine();
        }

        public static void AddUrlToStringBuilder(StringBuilder builder, string description, string linkName, string url)
        {
            if (url != null && url.ToString() != "")
            {
                builder.Append(description).AppendLine().Append("[").Append(linkName).Append("](").Append(url).Append(")").AppendLine();
            }
        }
    }
}
