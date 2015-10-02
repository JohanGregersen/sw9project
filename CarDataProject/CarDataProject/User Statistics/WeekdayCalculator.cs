using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;


namespace CarDataProject {
    public static class WeekdayCalculator {
        public static Dictionary<DayOfWeek, int> PlotsPerWeekday(Int16 carid) {
            Dictionary<DayOfWeek, int> entriesPerDay = new Dictionary<DayOfWeek, int>();

            entriesPerDay.Add(DayOfWeek.Monday, 0);
            entriesPerDay.Add(DayOfWeek.Tuesday, 0);
            entriesPerDay.Add(DayOfWeek.Wednesday, 0);
            entriesPerDay.Add(DayOfWeek.Thursday, 0);
            entriesPerDay.Add(DayOfWeek.Friday, 0);
            entriesPerDay.Add(DayOfWeek.Saturday, 0);
            entriesPerDay.Add(DayOfWeek.Sunday, 0);

            //Fetch all date entries for a given car
            List<int> allDates = FetchAllDatesByCarId(carid);

            foreach (int rawDate in allDates) {

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
            FileWriter.PlotsPerWeekday(entriesPerDay);
            GnuplotHelper.PlotGraph(4);
            return entriesPerDay;
        }

        public static Dictionary<DayOfWeek, TimeSpan> TimePerWeekday(Int16 carid, List<Trip> trips) {
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
            FileWriter.TimePerWeekday(timePerDay);
            GnuplotHelper.PlotGraph(5);
            return timePerDay;
        }

        private static Dictionary<DayOfWeek, Dictionary<int, TimeSpan>> setupDay(Dictionary<DayOfWeek, Dictionary<int, TimeSpan>> dictionary, DayOfWeek day) {

            dictionary.Add(day, new Dictionary<int, TimeSpan>());

            for (int i = 0; i < 24; i++) {
                dictionary[day].Add(i, new TimeSpan(0, 0, 0));
            }

            return dictionary;
        }

        public static Dictionary<DayOfWeek, Dictionary<int, TimeSpan>> TimePerHourPerWeekday(Int16 carid, List<Trip> trips) {
            Dictionary<DayOfWeek, Dictionary<int, TimeSpan>> timePerHour = new Dictionary<DayOfWeek, Dictionary<int, TimeSpan>>();
            setupDay(timePerHour, DayOfWeek.Monday);
            setupDay(timePerHour, DayOfWeek.Tuesday);
            setupDay(timePerHour, DayOfWeek.Wednesday);
            setupDay(timePerHour, DayOfWeek.Thursday);
            setupDay(timePerHour, DayOfWeek.Friday);
            setupDay(timePerHour, DayOfWeek.Saturday);
            setupDay(timePerHour, DayOfWeek.Sunday);
            
            int hour = 60;
            int minute = 60;

            //For all trips
            foreach (Trip trip in trips) {

                //If last timestamp has later date or hour than the first timestamp
                if (trip.allTimestamps[trip.allTimestamps.Count - 1].Item2.Date > trip.allTimestamps[0].Item2.Date || trip.allTimestamps[trip.allTimestamps.Count - 1].Item2.Hour > trip.allTimestamps[0].Item2.Hour) {

                    //Add all time from first timestamp until the beginning of the next hour
                    timePerHour[trip.allTimestamps[0].Item2.DayOfWeek][trip.allTimestamps[0].Item2.Hour] += new TimeSpan(0, hour - trip.allTimestamps[0].Item2.Minute, minute - trip.allTimestamps[0].Item2.Second);

                    //Keep track of the hour and date of the day
                    int currentHour = trip.allTimestamps[0].Item2.Hour + 1;
                    DateTime currentDate = trip.allTimestamps[0].Item2;

                    //If past midnight, add a day to currentDate and reset currentHour
                    if (currentHour == 24) {
                        currentHour = 0;
                        currentDate.AddDays(1);
                    }

                    //For the next hours where the trip does not end, add an hour to total time.
                    while (currentHour < trip.allTimestamps[trip.allTimestamps.Count - 1].Item2.Hour || currentDate.Date < trip.allTimestamps[trip.allTimestamps.Count - 1].Item2.Date) {
                        timePerHour[currentDate.DayOfWeek][currentHour] += new TimeSpan(1, 0, 0);
                        currentHour++;

                        if (currentHour == 24) {
                            currentHour = 0;
                            currentDate.AddDays(1);
                        }
                    }

                    //On the last hour where the trip ends, add all remaining minutes and seconds
                    timePerHour[currentDate.DayOfWeek][currentHour] += new TimeSpan(0, trip.allTimestamps[trip.allTimestamps.Count - 1].Item2.Minute, trip.allTimestamps[trip.allTimestamps.Count - 1].Item2.Second);
                } else {
                    //If trip ends on the same hour as it starts, just subtract starttime from endtime to get the timespan
                    timePerHour[trip.allTimestamps[0].Item2.DayOfWeek][trip.allTimestamps[0].Item2.Hour] += trip.allTimestamps[trip.allTimestamps.Count - 1].Item2.TimeOfDay - trip.allTimestamps[0].Item2.TimeOfDay;
                }
            }
            FileWriter.TimePerHourPerWeekday(timePerHour);
            GnuplotHelper.PlotGraph(6);
            return timePerHour;
        }

        //Duplicated from TripCalculator - Maybe put in utility or DBhandler?
        private static List<int> FetchAllDatesByCarId(Int16 carid) {
            DBController dbc = new DBController();
            List<int> allDates = dbc.GetAllDatesByCarId(carid, false, false);
            dbc.Close();
            return allDates;
        }

        private static string DateAndTimeConverter(int dt) {
            string strDT = dt.ToString();

            if (strDT.Length == 5) {
                strDT = "0" + strDT;
            }
            return strDT;
        }
    }
}
