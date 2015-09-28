using System;
using System.Collections.Generic;
using System.Text;


namespace CarDataProject.DataManipulators {
    public static class WeekdayCalculator {
        public static Dictionary<DayOfWeek, int> PlotsPerWeekday(Int64 carid) {
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

        public static Dictionary<DayOfWeek, TimeSpan> TimePerWeekday(Int64 carid, List<Trip> trips) {
            Dictionary<DayOfWeek, TimeSpan> timePerDay = new Dictionary<DayOfWeek, TimeSpan>();
            timePerDay.Add(DayOfWeek.Monday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Tuesday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Wednesday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Thursday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Friday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Saturday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Sunday, new TimeSpan(0, 0, 0));

            TimeSpan day = new TimeSpan(24, 0, 0);

            //For all trips
            foreach (Trip trip in trips) {

                //If last timestamp is another date than first timestamp
                if (trip.allTimestamps[trip.allTimestamps.Count - 1].Item2.Date > trip.allTimestamps[0].Item2.Date) {

                    //Add all time from first timestamp until midnight to total time
                    timePerDay[trip.allTimestamps[0].Item2.DayOfWeek] += day - trip.allTimestamps[0].Item2.TimeOfDay;

                    //For next days where the trip does not end, add 24 hours to total time
                    DateTime date = trip.allTimestamps[0].Item2.Date.AddDays(1);

                    while (date < trip.allTimestamps[trip.allTimestamps.Count - 1].Item2.Date) {
                        timePerDay[date.DayOfWeek] += day;
                        date.AddDays(1);
                    }

                    //On the last day, add all time from midnight until the last timestamp
                    timePerDay[date.DayOfWeek] += trip.allTimestamps[trip.allTimestamps.Count - 1].Item2.TimeOfDay;
                } else {
                    //If trip ends same day as it starts, just subtract starttime from endtime to get the timespan
                    timePerDay[trip.allTimestamps[0].Item2.DayOfWeek] += trip.allTimestamps[trip.allTimestamps.Count - 1].Item2.TimeOfDay - trip.allTimestamps[0].Item2.TimeOfDay;
                }
            }
            return timePerDay;
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
