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
                totalDistance += CarStatistics.TotalDistance(carId);
            }

            return totalDistance / carIds.Count;
        }

        public static TimeSpan AverageTotalTime() {
            TimeSpan totalTime = new TimeSpan(0, 0, 0);

            DBController dbc = new DBController();
            List<Int16> carIds = dbc.GetCarIds();
            dbc.Close();

            foreach (Int16 carId in carIds) {
                totalTime += CarStatistics.TotalTime(carId);
            }

            return new TimeSpan(totalTime.Ticks / carIds.Count);
        }
    }
}
