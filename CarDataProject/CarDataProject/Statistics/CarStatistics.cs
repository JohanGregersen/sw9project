using System;
using System.Collections.Generic;
using System.IO;

namespace CarDataProject {
    class CarStatistics {
        public static void WriteAll(Int16 carId) {
            FileWriter.KilometersPerTrip(carId, DistancePerTrip(carId));
            FileWriter.MinutesPerTrip(carId, TimePerTrip(carId));
            FileWriter.DefaultCarStatistics(carId);

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
            Int64 tripCount = TripCount(carId);
            List<double> kilometersPerTrip = new List<double>();

            for (int i = 1; i < tripCount; i++) {
                kilometersPerTrip.Add(TripStatistics.KilometersDriven(carId, i));
            }

            return kilometersPerTrip;
        }
        
        public static List<TimeSpan> TimePerTrip(Int16 carId) {
            Int64 tripCount = TripCount(carId);
            List<TimeSpan> minutesPerTrip = new List<TimeSpan>();

            for (int i = 1; i < tripCount; i++) {
                minutesPerTrip.Add(TripStatistics.Duration(carId, i));
            }

            return minutesPerTrip;
        }
    }
}