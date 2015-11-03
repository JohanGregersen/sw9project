using System;
using System.Collections.Generic;

namespace CarDataProject {
    public static class WeekdayStatistics {
        public static void WriteAll(Int16 carId) {
            FileWriter.PlotsPerWeekday(carId, PlotsPerWeekday(carId));
            FileWriter.TimePerWeekday(carId, TimePerWeekday(carId)); 
        }

        public static void PlotAll(Int16 carId) {
            GnuplotHelper.Plot(Global.CarStatistics.PlotsPerWeekdayFile(carId), Global.CarStatistics.PlotsPerWeekdayGraph(carId), true, 1, 2, "Points per weekday");
        }

        public static Dictionary<DayOfWeek, int> PlotsPerWeekday(Int16 carId) {
            Dictionary<DayOfWeek, int> plotsPerDay = new Dictionary<DayOfWeek, int>();
            plotsPerDay.Add(DayOfWeek.Monday, 0);
            plotsPerDay.Add(DayOfWeek.Tuesday, 0);
            plotsPerDay.Add(DayOfWeek.Wednesday, 0);
            plotsPerDay.Add(DayOfWeek.Thursday, 0);
            plotsPerDay.Add(DayOfWeek.Friday, 0);
            plotsPerDay.Add(DayOfWeek.Saturday, 0);
            plotsPerDay.Add(DayOfWeek.Sunday, 0);

            //Fetch all timestamps for all trips on car
            DBController dbc = new DBController();
            List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);
            List<TemporalInformation> timestamps = new List<TemporalInformation>();
            foreach (Int64 tripId in tripIds) {
                timestamps = dbc.GetTimestampsByCarIdAndTripId(carId, tripId);
            }

            dbc.Close();

            foreach (TemporalInformation timestamp in timestamps) {
                plotsPerDay[timestamp.Timestamp.DayOfWeek] += 1;
            }

            return plotsPerDay;
        }

        public static Dictionary<DayOfWeek, TimeSpan> TimePerWeekday(Int16 carId) {
            Dictionary<DayOfWeek, TimeSpan> timePerDay = new Dictionary<DayOfWeek, TimeSpan>();
            timePerDay.Add(DayOfWeek.Monday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Tuesday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Wednesday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Thursday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Friday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Saturday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Sunday, new TimeSpan(0, 0, 0));

            TimeSpan day = new TimeSpan(24, 0, 0);

            DBController dbc = new DBController();
            List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);   

            //For all trips
            foreach (Int64 tripId in tripIds) {
                List<TemporalInformation> entries = dbc.GetTimestampsByCarIdAndTripId(carId, tripId);

                //If last timestamp is another date than first timestamp
                if (entries[entries.Count - 1].Timestamp.Date > entries[0].Timestamp.Date) {

                    //Add all time from first timestamp until midnight to total time
                    timePerDay[entries[0].Timestamp.DayOfWeek] += day - entries[0].Timestamp.TimeOfDay;

                    //For next days where the trip does not end, add 24 hours to total time
                    DateTime date = entries[0].Timestamp.AddDays(1);

                    while (date < entries[entries.Count - 1].Timestamp.Date) {
                        timePerDay[date.DayOfWeek] += day;
                        date.AddDays(1);
                    }

                    //On the last day, add all time from midnight until the last timestamp
                    timePerDay[date.DayOfWeek] += entries[entries.Count - 1].Timestamp.TimeOfDay;
                } else {
                    //If trip ends same day as it starts, just subtract starttime from endtime to get the timespan
                    timePerDay[entries[0].Timestamp.DayOfWeek] += entries[entries.Count - 1].Timestamp.TimeOfDay - entries[0].Timestamp.TimeOfDay;
                }
            }

            dbc.Close();
            return timePerDay;
        }

        public static Dictionary<DayOfWeek, Dictionary<int, TimeSpan>> TimePerHourPerWeekday(Int16 carId) {
            Dictionary<DayOfWeek, Dictionary<int, TimeSpan>> timePerHourPerWeekday = new Dictionary<DayOfWeek, Dictionary<int, TimeSpan>>();
            setupDay(timePerHourPerWeekday, DayOfWeek.Monday);
            setupDay(timePerHourPerWeekday, DayOfWeek.Tuesday);
            setupDay(timePerHourPerWeekday, DayOfWeek.Wednesday);
            setupDay(timePerHourPerWeekday, DayOfWeek.Thursday);
            setupDay(timePerHourPerWeekday, DayOfWeek.Friday);
            setupDay(timePerHourPerWeekday, DayOfWeek.Saturday);
            setupDay(timePerHourPerWeekday, DayOfWeek.Sunday);

            int hour = 60;
            int minute = 60;

            DBController dbc = new DBController();
            List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);

            //For all trips
            foreach (Int64 tripId in tripIds) {
                List<TemporalInformation> entries = dbc.GetTimestampsByCarIdAndTripId(carId, tripId);

                //If last timestamp has later date or hour than the first timestamp
                if (entries[entries.Count - 1].Timestamp.Date > entries[0].Timestamp.Date || entries[entries.Count - 1].Timestamp.Hour > entries[0].Timestamp.Hour) {

                    //Add all time from first timestamp until the beginning of the next hour
                    timePerHourPerWeekday[entries[0].Timestamp.DayOfWeek][entries[0].Timestamp.Hour] += new TimeSpan(0, hour - entries[0].Timestamp.Minute, minute - entries[0].Timestamp.Second);

                    //Keep track of the hour and date of the day
                    int currentHour = entries[0].Timestamp.Hour + 1;
                    DateTime currentDate = entries[0].Timestamp;

                    //If past midnight, add a day to currentDate and reset currentHour
                    if (currentHour == 24) {
                        currentHour = 0;
                        currentDate.AddDays(1);
                    }

                    //For the next hours where the trip does not end, add an hour to total time.
                    while (currentHour < entries[entries.Count - 1].Timestamp.Hour || currentDate.Date < entries[entries.Count - 1].Timestamp.Date) {
                        timePerHourPerWeekday[currentDate.DayOfWeek][currentHour] += new TimeSpan(1, 0, 0);
                        currentHour++;

                        if (currentHour == 24) {
                            currentHour = 0;
                            currentDate.AddDays(1);
                        }
                    }

                    //On the last hour where the trip ends, add all remaining minutes and seconds
                    timePerHourPerWeekday[currentDate.DayOfWeek][currentHour] += new TimeSpan(0, entries[entries.Count - 1].Timestamp.Minute, entries[entries.Count - 1].Timestamp.Second);
                } else {
                    //If trip ends on the same hour as it starts, just subtract starttime from endtime to get the timespan
                    timePerHourPerWeekday[entries[0].Timestamp.DayOfWeek][entries[0].Timestamp.Hour] += entries[entries.Count - 1].Timestamp.TimeOfDay - entries[entries.Count - 1].Timestamp.TimeOfDay;
                }
            }

            return timePerHourPerWeekday;
        }

        private static Dictionary<DayOfWeek, Dictionary<int, TimeSpan>> setupDay(Dictionary<DayOfWeek, Dictionary<int, TimeSpan>> dictionary, DayOfWeek day) {
            dictionary.Add(day, new Dictionary<int, TimeSpan>());

            for (int i = 0; i < 24; i++) {
                dictionary[day].Add(i, new TimeSpan(0, 0, 0));
            }

            return dictionary;
        }
    }
}