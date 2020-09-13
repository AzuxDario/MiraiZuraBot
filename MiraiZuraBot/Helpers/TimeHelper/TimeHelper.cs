using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MiraiZuraBot.Helpers.TimeHelper
{
    class TimeHelper
    {
        public DateTime GetCurrentJapanTime()
        {
            DateTime todayUTC = DateTime.UtcNow;
            TimeZoneInfo japanTimeZone;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                japanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            }
            else
            {
                japanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Japan");
            }
            return TimeZoneInfo.ConvertTimeFromUtc(todayUTC, japanTimeZone);
        }
    }
}
