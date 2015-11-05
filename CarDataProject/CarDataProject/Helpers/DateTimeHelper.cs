using System;

namespace CarDataProject {
    public static class DateTimeHelper {
        public static DateTime ConvertToDateTime(int date, int time) {
            string dateString = date.ToString("D6");
            string timeString = time.ToString("D6");

            Int16 year = Int16.Parse(dateString.Substring(4, 2));
            Int16 month = Int16.Parse(dateString.Substring(2, 2));
            Int16 day = Int16.Parse(dateString.Substring(0, 2));

            //Year is represented in two digits only
            year += 2000;

            Int16 hour = Int16.Parse(timeString.Substring(0, 2));
            Int16 minute = Int16.Parse(timeString.Substring(2, 2));
            Int16 second = Int16.Parse(timeString.Substring(4, 2));

            return new DateTime(year, month, day, hour, minute, second);
        }

        public static Int64 ToUnixTime(DateTime date) {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
    }
}
