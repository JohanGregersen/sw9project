using System;
using System.Collections.Generic;

namespace CarDataProject {
    class MetaCarStatistics {
        public static double AverageTotalDistance() {
            double totalDistance = 0;

            DBController dbc = new DBController();
            List<Int16> carIds = dbc.GetCarIds();
            dbc.Close();
            
            foreach (Int16 carId in carIds) {
                totalDistance = CarStatistics.TotalDistance(carId);
            }

            return totalDistance / carIds.Count;
        }

        public static double AverageTripDistance() {
            double totalDistance = 0;

            DBController dbc = new DBController();
            List<Int16> carIds = dbc.GetCarIds();
            Int64 tripCount = dbc.GetTripCount();
            dbc.Close();

            foreach (Int16 carId in carIds) {
                List<double> distancePerTrip = CarStatistics.DistancePerTrip(carId);
                foreach (double distance in distancePerTrip) {
                    totalDistance += distance;
                }
            }

            return totalDistance / tripCount;
        }

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
    }
}
