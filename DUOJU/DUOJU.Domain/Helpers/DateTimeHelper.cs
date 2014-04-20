using System;

namespace DUOJU.Domain.Helpers
{
    public static class DateTimeHelper
    {
        private static DateTime _startDataTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));


        public static DateTime ConvertDateTime(long secondTimeStamp)
        {
            return _startDataTime.AddSeconds(secondTimeStamp);
        }

        public static long ConvertTimeStamp(DateTime dt)
        {
            return (long)(dt - _startDataTime).TotalSeconds;
        }
    }
}
