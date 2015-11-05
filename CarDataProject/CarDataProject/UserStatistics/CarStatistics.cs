using System;
using System.Collections.Generic;
using System.IO;

namespace CarDataProject {
    class CarStatistics {
        public static void WriteAll(Int16 carId) {
            FileWriter.KilometersPerTrip(carId, DistancePerTrip(carId));
            FileWriter.MinutesPerTrip(carId, TimePerTrip(carId));
            FileWriter.DefaultCarStatistics(carId);
            }

        public static void PlotAll(Int16 carId) {
            GnuplotHelper.Plot(Global.CarStatistics.KilometersPerTripFile(carId), Global.CarStatistics.KilometersPerTripGraph(carId), true, 1, 2, "Kilometers per trip");
            GnuplotHelper.Plot(Global.CarStatistics.MinutesPerTripFile(carId), Global.CarStatistics.MinutesPerTripGraph(carId), true, 1, 2, "Minutes per trip");

        }

        public static Int64 TripCount(Int16 carId) {
            DBController dbc = new DBController();
            Int64 tripCount = dbc.GetTripCountByCarId(carId);
            dbc.Close();
            return tripCount;
        }
        
        public static List<double> DistancePerTrip(Int16 carId) {
            DBController dbc = new DBController();
            List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);
            dbc.Close();

            List<double> kilometersPerTrip = new List<double>();

            foreach (Int64 tripId in tripIds) {
                kilometersPerTrip.Add(TripStatistics.Distance(carId, tripId));
            }

            return kilometersPerTrip;
        }
        
        public static List<TimeSpan> TimePerTrip(Int16 carId) {
            DBController dbc = new DBController();
            List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);
            dbc.Close();

            List<TimeSpan> minutesPerTrip = new List<TimeSpan>();

            foreach (Int64 tripId in tripIds) {
                minutesPerTrip.Add(TripStatistics.Duration(carId, tripId));
            }

            return minutesPerTrip;
        }

        public static double TotalDistance(Int16 carId) {
            double totalDistance = 0;
            List<double> distancePerTrip = DistancePerTrip(carId);

            foreach (double distance in distancePerTrip) {
                totalDistance += distance;
            }

            return totalDistance;
        }

        public static TimeSpan TotalTime(Int16 carId) {
            TimeSpan totalTime = new TimeSpan(0, 0, 0);
            List<TimeSpan> timePerTrip = TimePerTrip(carId);

            foreach (TimeSpan time in timePerTrip) {
                totalTime += time;
            }

            return totalTime;
        }
    }
}