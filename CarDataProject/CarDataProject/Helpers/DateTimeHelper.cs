using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    public static class DateTimeHelper {


        public static DateTime ConvertToDateTime(Int64 rdate, Int64 rtime) {

            string strDate = DateAndTimeChecker(rdate);
            String strTime = DateAndTimeChecker(rtime);

            StringBuilder sb = new StringBuilder();
            int year = Convert.ToInt32(sb.Append("20").Append(strDate[4]).Append(strDate[5]).ToString());
            sb.Clear();
            int month = Convert.ToInt32(sb.Append(strDate[2]).Append(strDate[3]).ToString());
            sb.Clear();
            int day = Convert.ToInt32(sb.Append(strDate[0]).Append(strDate[1]).ToString());
            sb.Clear();
            int hour = Convert.ToInt32(sb.Append(strTime[0]).Append(strTime[1]).ToString());
            sb.Clear();
            int minute = Convert.ToInt32(sb.Append(strTime[2]).Append(strTime[3]).ToString());
            sb.Clear();
            int second = Convert.ToInt32(sb.Append(strTime[4]).Append(strTime[5]).ToString());

            DateTime dt = new DateTime(year, month, day, hour, minute, second);

            return dt;
        }

        private static string DateAndTimeChecker(Int64 dt) {
            string strDT = dt.ToString();

            if (strDT.Length == 5) {
                strDT = "0" + strDT;
            }
            return strDT;
        }

        public static long ToUnixTime(DateTime date) {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
    }
}
