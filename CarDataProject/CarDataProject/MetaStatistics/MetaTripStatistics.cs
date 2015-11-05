using System;
using System.Collections.Generic;

namespace CarDataProject {
    class MetaTripStatistics {
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

        public static TimeSpan AverageTripTime() {
            TimeSpan totalTime = new TimeSpan(0, 0, 0);

            DBController dbc = new DBController();
            List<Int16> carIds = dbc.GetCarIds();
            Int64 tripCount = dbc.GetTripCount();
            dbc.Close();

            foreach (Int16 carId in carIds) {
                List<TimeSpan> timePerTrip = CarStatistics.TimePerTrip(carId);
                foreach (TimeSpan time in timePerTrip) {
                    totalTime += time;
                }
            }

            return new TimeSpan(totalTime.Ticks / carIds.Count);
        }
    }
}
