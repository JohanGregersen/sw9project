using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace CarDataProject {
    class TripStatistics {
        public static void WriteAll(Int16 carId, Int64 tripId) {
            FileWriter.DefaultTripStatistics(carId, tripId);
        }

        public static TimeSpan Duration (Int16 carId, int tripId) {
            DBController dbc = new DBController();
            List<TemporalInformation> timestamps = dbc.GetTimestampsByCarIdAndTripId(carId, tripId);
            dbc.Close();

            return timestamps[timestamps.Count - 1].Timestamp - timestamps[0].Timestamp;
        }

        //Hvad skal der ske her? Eller måske er det "Hvad skal der ske i GetMpointPlot?"
        public static double Distance(Int16 carId, Int64 tripId) {
            double distance = 0;
            List<SpatialInformation> entries = ValidationPlots.GetMpointPlot(carId, tripId);

            for (int i = 1; i < entries.Count - 1; i++) {
                distance += entries[i].MPoint.GetDistanceTo(entries[i - 1].MPoint);
            }

            return distance /= 1000;
        }

        public static Dictionary<int, double> AccelerationCalcultions(Int16 carId, Int64 tripId) {
            DBController dbc = new DBController();
            List<Tuple<int, Timestamp, int>> accelerationData = dbc.GetAccelerationDataByTrip(carId, tripId);
            dbc.Close();

            List<Tuple<int, double>> accelerationCalculations = new List<Tuple<int, double>>();

            for(int i = 0; i < accelerationData.Count() - 1; i++) {
                //Item1 = TimeStamp (DateTime object)
                //Item2 = speed
                // a = VELCOCITY CHANGE / TIME TAKEN;
                double velocityChange = accelerationData[i + 1].Item3 - accelerationData[i].Item3;
                    
                double timeTaken = Convert.ToDouble((DateTimeHelper.ToUnixTime(accelerationData[i + 1].Item2.timestamp) - DateTimeHelper.ToUnixTime(accelerationData[i].Item2.timestamp)));

                double acceleration = velocityChange / timeTaken;

                accelerationCalculations.Add(new Tuple<int, double>(accelerationData[i].Item1, acceleration));
            }

            // FileWriter.Acceleration(accelerationCalculations);
            // GnuplotHelper.PlotGraph(10, "acceleration");

            return accelerationCalculations;
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                 