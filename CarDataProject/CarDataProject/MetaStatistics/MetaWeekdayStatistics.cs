using System;
using System.Collections.Generic;

namespace CarDataProject {
    class MetaWeekdayStatistics {
        public static Dictionary<DayOfWeek, double> AverageTripDistancePerDay() {
            Dictionary<DayOfWeek, double> distancePerDay = new Dictionary<DayOfWeek, double>();
            distancePerDay.Add(DayOfWeek.Monday, 0);
            distancePerDay.Add(DayOfWeek.Tuesday, 0);
            distancePerDay.Add(DayOfWeek.Wednesday, 0);
            distancePerDay.Add(DayOfWeek.Thursday, 0);
            distancePerDay.Add(DayOfWeek.Friday, 0);
            distancePerDay.Add(DayOfWeek.Saturday, 0);
            distancePerDay.Add(DayOfWeek.Sunday, 0);

            DBController dbc = new DBController();
            List<Int16> carIds = dbc.GetCarIds();
            Int64 tripCount = dbc.GetTripCount();
            dbc.Close();

            foreach (Int16 carId in carIds) {
                Dictionary<DayOfWeek, double> carData = WeekdayStatistics.Distance(carId);
                foreach (KeyValuePair<DayOfWeek, double> entry in carData) {
                    distancePerDay[entry.Key] += entry.Value;
                }
            }

            foreach (KeyValuePair<DayOfWeek, double> day in distancePerDay) {
                distancePerDay[day.Key] /= tripCount;
            }

            return distancePerDay;
        }

        public static Dictionary<DayOfWeek, TimeSpan> AverageTripTimePerDay() {
            Dictionary<DayOfWeek, TimeSpan> timePerDay = new Dictionary<DayOfWeek, TimeSpan>();
            timePerDay.Add(DayOfWeek.Monday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Tuesday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Wednesday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Thursday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Friday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Saturday, new TimeSpan(0, 0, 0));
            timePerDay.Add(DayOfWeek.Sunday, new TimeSpan(0, 0, 0));

            DBController dbc = new DBController();
            List<Int16> carIds = dbc.GetCarIds();
            Int64 tripCount = dbc.GetTripCount();
            dbc.Close();

            foreach (Int16 carId in carIds) {
                Dictionary<DayOfWeek, TimeSpan> carData = WeekdayStatistics.Time(carId);
                foreach (KeyValuePair<DayOfWeek, TimeSpan> entry in carData) {
                    timePerDay[entry.Key] += entry.Value;
                }
            }

            foreach (KeyValuePair<DayOfWeek, TimeSpan> day in timePerDay) {
                timePerDay[day.Key] = new TimeSpan(timePerDay[day.Key].Ticks / carIds.Count);
            }

            return timePerDay;
        }
    }
}
