using System;
using System.Collections.Generic;
using System.Text;


namespace CarDataProject.DataManipulators {
    public static class WeekdayCalculator {
        public static Dictionary<DayOfWeek, int> CalculateWeekdays(Int64 carid) {
            Dictionary<DayOfWeek, int> entriesPerDay = new Dictionary<DayOfWeek, int>();
            entriesPerDay.Add(DayOfWeek.Monday, 0);
            entriesPerDay.Add(DayOfWeek.Tuesday, 0);
            entriesPerDay.Add(DayOfWeek.Wednesday, 0);
            entriesPerDay.Add(DayOfWeek.Thursday, 0);
            entriesPerDay.Add(DayOfWeek.Friday, 0);
            entriesPerDay.Add(DayOfWeek.Saturday, 0);
            entriesPerDay.Add(DayOfWeek.Sunday, 0);

            //Fetch all date entries for a given car
            List<Int64> allDates = FetchAllDatesByCarId(carid);

            foreach (Int64 rawDate in allDates) {

                //Stolen from DBController
                String formattedDate = DateAndTimeConverter(rawDate);
                StringBuilder sb = new StringBuilder();
                int year = Convert.ToInt32(sb.Append("20").Append(formattedDate[4]).Append(formattedDate[5]).ToString());
                sb.Clear();
                int month = Convert.ToInt32(sb.Append(formattedDate[2]).Append(formattedDate[3]).ToString());
                sb.Clear();
                int day = Convert.ToInt32(sb.Append(formattedDate[0]).Append(formattedDate[1]).ToString());
                DateTime date = new DateTime(year, month, day);
                entriesPerDay[date.DayOfWeek] += 1;
                }

            return entriesPerDay;


        }
        //Duplicated from TripCalculator - Maybe put in utility or DBhandler?
        private static List<Int64> FetchAllDatesByCarId(Int64 carid) {
            DBController dbc = new DBController();
            List<Int64> allDates = dbc.GetAllDatesByCarId(carid, false, false);
            dbc.Close();
            return allDates;
        }

        //Duplicated
        private static string DateAndTimeConverter(Int64 dt) {
            string strDT = dt.ToString();

            if (strDT.Length == 5) {
                strDT = "0" + strDT;
            }
            return strDT;
        }
    }
}
