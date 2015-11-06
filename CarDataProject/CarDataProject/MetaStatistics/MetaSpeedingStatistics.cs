using System;
using System.Collections.Generic;

namespace CarDataProject {
    class MetaSpeedingStatistics {
        public static double AveragePercentageTimeAbove(double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            TimeSpan totalTimeAbove = new TimeSpan(0, 0, 0);
            TimeSpan totalTimeBelow = new TimeSpan(0, 0, 0);
            DBController dbc = new DBController();
            List<Int16> carIds = dbc.GetCarIds();

            foreach (Int16 carId in carIds) {
                List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);
                foreach (Int64 tripId in tripIds) {
                    totalTimeAbove += SpeedingStatistics.TimeAbove(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
                    totalTimeBelow += SpeedingStatistics.TimeBelow(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
                }
            }

            dbc.Close();
            return totalTimeAbove.Ticks / (totalTimeAbove.Ticks + totalTimeBelow.Ticks) * 100;
        }

        public static double AveragePercentageTimeBelow(double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            TimeSpan totalTimeAbove = new TimeSpan(0, 0, 0);
            TimeSpan totalTimeBelow = new TimeSpan(0, 0, 0);
            DBController dbc = new DBController();
            List<Int16> carIds = dbc.GetCarIds();

            foreach (Int16 carId in carIds) {
                List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);
                foreach (Int64 tripId in tripIds) {
                    totalTimeAbove += SpeedingStatistics.TimeAbove(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
                    totalTimeBelow += SpeedingStatistics.TimeBelow(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
                }
            }

            dbc.Close();
            return totalTimeBelow.Ticks / (totalTimeAbove.Ticks + totalTimeBelow.Ticks) * 100;
        }

        public static double AveragePercentageDistanceAbove(double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            double totalDistanceAbove = 0;
            double totalDistanceBelow = 0;
            DBController dbc = new DBController();
            List<Int16> carIds = dbc.GetCarIds();

            foreach (Int16 carId in carIds) {
                List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);
                foreach (Int64 tripId in tripIds) {
                    totalDistanceAbove += SpeedingStatistics.DistanceAbove(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
                    totalDistanceBelow += SpeedingStatistics.DistanceBelow(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
                }
            }

            dbc.Close();
            return totalDistanceAbove / (totalDistanceAbove + totalDistanceBelow) * 100;
        }

        public static double AveragePercentageDistanceBelow(double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            double totalDistanceAbove = 0;
            double totalDistanceBelow = 0;
            DBController dbc = new DBController();
            List<Int16> carIds = dbc.GetCarIds();

            foreach (Int16 carId in carIds) {
                List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);
                foreach (Int64 tripId in tripIds) {
                    totalDistanceAbove += SpeedingStatistics.DistanceAbove(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
                    totalDistanceBelow += SpeedingStatistics.DistanceBelow(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
                }
            }

            dbc.Close();
            return totalDistanceBelow / (totalDistanceAbove + totalDistanceBelow) * 100;
        }

        public static double AveragePercentageTimeAboveInThreshold(double lowerPercentage, double upperPercentage, double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            List<double> percentages = new List<double>();

            DBController dbc = new DBController();
            List<Int16> carIds = dbc.GetCarIds();

            foreach (Int16 carId in carIds) {
                List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);
                foreach (Int64 tripId in tripIds) {
                    percentages.Add(SpeedingStatistics.PercentageTimeAboveInThreshold(carId, tripId, lowerPercentage, upperPercentage, ignorableSpeed, ignorableTime, ignorableDistance));
                }
            }

            double totalPercentage = 0;
            foreach (double percentage in percentages) {
                totalPercentage += percentage;
            }

            dbc.Close();
            return totalPercentage / percentages.Count;
        }

        public static double AveragePercentageDistanceAboveInThreshold(double lowerPercentage, double upperPercentage, double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            List<double> percentages = new List<double>();

            DBController dbc = new DBController();
            List<Int16> carIds = dbc.GetCarIds();

            foreach (Int16 carId in carIds) {
                List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);
                foreach (Int64 tripId in tripIds) {
                    percentages.Add(SpeedingStatistics.PercentageDistanceAboveInThreshold(carId, tripId, lowerPercentage, upperPercentage, ignorableSpeed, ignorableTime, ignorableDistance));
                }
            }

            double totalPercentage = 0;
            foreach (double percentage in percentages) {
                totalPercentage += percentage;
            }

            dbc.Close();
            return totalPercentage / percentages.Count;
        }
    }
}
