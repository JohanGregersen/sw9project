using System;
using System.Collections.Generic;

namespace CarDataProject {
    public static class WeekdayStatistics {
        public static void WriteAll(Int16 carId) {
            FileWriter.PlotsPerWeekday(carId, Plots(carId));
            FileWriter.TimePerWeekday(carId, Time(carId)); 
        }

        public static void PlotAll(Int16 carId) {
            GnuplotHelper.Plot(Global.CarStatistics.PlotsPerWeekdayFile(carId), Global.CarStatistics.PlotsPerWeekdayGraph(carId), true, 1, 2, "Points per weekday");
        }

        public static Dictionary<DayOfWeek, int> Plots(Int16 carId) {
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

        public static Dictionary<DayOfWeek, TimeSpan> Time(Int16 carId) {
            Dictionary<DayOfWeek, TimeSpan> timePerDay = new Dictionary<DayOfWeek, TimeSpan>();
            timePerDay.Add(DayOfWeek.Monday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Tuesday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Wednesday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Thursday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Friday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Saturday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Sunday, new TimeSpan(0, 0, 0));

            DBController dbc = new DBController();
            List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);
            dbc.Close();

            foreach (Int64 tripId in tripIds) {
                List<TemporalInformation> entries = dbc.GetTimestampsByCarIdAndTripId(carId, tripId);

                for (int i = 1; i < entries.Count; i++) {
                    timePerDay[entries[i].Timestamp.DayOfWeek] += entries[i].TimeToLag;
                }
            }

            return timePerDay;
        }

        public static Dictionary<DayOfWeek, double> Distance(Int16 carId) {
            Dictionary<DayOfWeek, double> distancePerDay = new Dictionary<DayOfWeek, double>();
            distancePerDay.Add(DayOfWeek.Monday, 0);
            distancePerDay.Add(DayOfWeek.Tuesday, 0);
            distancePerDay.Add(DayOfWeek.Wednesday, 0);
            distancePerDay.Add(DayOfWeek.Thursday, 0);
            distancePerDay.Add(DayOfWeek.Friday, 0);
            distancePerDay.Add(DayOfWeek.Saturday, 0);
            distancePerDay.Add(DayOfWeek.Sunday, 0);

            TimeSpan day = new TimeSpan(24, 0, 0);

            DBController dbc = new DBController();
            List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);
            dbc.Close();
            
            foreach (Int64 tripId in tripIds) {
                List<Fact> entries = dbc.GetSpatioTemporalByCarIdAndTripId(carId, tripId);

                for (int i = 1; i < entries.Count; i++) {
                    distancePerDay[entries[i].Temporal.Timestamp.DayOfWeek] += entries[i].Spatial.DistanceToLag;
                }
            }

            return distancePerDay;
        }

        public static Dictionary<DayOfWeek, Dictionary<int, TimeSpan>> TimePerHour(Int16 carId) {
            Dictionary<DayOfWeek, Dictionary<int, TimeSpan>> timePerHourPerWeekday = new Dictionary<DayOfWeek, Dictionary<int, TimeSpan>>();
            SetupDay(timePerHourPerWeekday, DayOfWeek.Monday);
            SetupDay(timePerHourPerWeekday, DayOfWeek.Tuesday);
            SetupDay(timePerHourPerWeekday, DayOfWeek.Wednesday);
            SetupDay(timePerHourPerWeekday, DayOfWeek.Thursday);
            SetupDay(timePerHourPerWeekday, DayOfWeek.Friday);
            SetupDay(timePerHourPerWeekday, DayOfWeek.Saturday);
            SetupDay(timePerHourPerWeekday, DayOfWeek.Sunday);

            DBController dbc = new DBController();
            List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);
            dbc.Close();

            foreach (Int64 tripId in tripIds) {
                List<TemporalInformation> entries = dbc.GetTimestampsByCarIdAndTripId(carId, tripId);

                for (int i = 1; i < entries.Count; i++) {
                    timePerHourPerWeekday[entries[i].Timestamp.DayOfWeek][entries[i].Timestamp.Hour] += entries[i].TimeToLag;
                }
            }
            return timePerHourPerWeekday;
        }

        private static Dictionary<DayOfWeek, Dictionary<int, TimeSpan>> SetupDay(Dictionary<DayOfWeek, Dictionary<int, TimeSpan>> dictionary, DayOfWeek day) {
            dictionary.Add(day, new Dictionary<int, TimeSpan>());

            for (int i = 0; i < 24; i++) {
                dictionary[day].Add(i, new TimeSpan(0, 0, 0));
            }

            return dictionary;
        }
    }
}