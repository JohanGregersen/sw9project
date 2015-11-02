using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace CarDataProject {
    class TripStatistics {
        public static void WriteAll(Int16 carId, Int64 tripId) {
            FileWriter.DefaultTripStatistics(carId, tripId);
        }

        public static TimeSpan Duration (Int16 carId, Int64 tripId) {
            DBController dbc = new DBController();
            List<TemporalInformation> timestamps = dbc.GetTimestampsByCarIdAndTripId(carId, tripId);
            dbc.Close();

            return timestamps[timestamps.Count - 1].Timestamp - timestamps[0].Timestamp;
        }
        
        public static double Distance(Int16 carId, Int64 tripId) {
            DBController dbc = new DBController();
            List<SpatialInformation> entries = dbc.GetMPointsByCarIdAndTripId(carId, tripId);
            dbc.Close();

            double distance = 0;

            for (int i = 1; i < entries.Count - 1; i++) {
                distance += entries[i].MPoint.GetDistanceTo(entries[i - 1].MPoint);
            }

            return distance /= 1000;
        }

        public static List<Fact> Acceleration(Int16 carId, Int64 tripId) {
            DBController dbc = new DBController();
            List<Fact> accelerationData = dbc.GetSpeedInformationByCarIdAndTripId(carId, tripId);
            dbc.Close();

            for(int i = 1; i < accelerationData.Count(); i++) {
                //Acceleration = Velocity change / Time
                double velocityChange = accelerationData[i].Measure.Speed - accelerationData[i - 1].Measure.Speed;       
                TimeSpan time = accelerationData[i].Temporal.Timestamp - accelerationData[i - 1].Temporal.Timestamp;
                double acceleration = velocityChange / time.Seconds;

                accelerationData[i].Measure.Acceleration = acceleration;
            }

            return accelerationData;
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                 