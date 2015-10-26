using System;
using System.Collections.Generic;

namespace CarDataProject.Data_Validation {
    public static class TripHoles {
        public static Dictionary<Point, Point> ByTime(Int16 carId, int tripId, TimeSpan HoleSizeMinimumTime) {
            Dictionary<Point, Point> tripHoles = new Dictionary<Point, Point>();

            DBController dbc = new DBController();
            List<Timestamp> timestamps = dbc.GetTimestampsByCarAndTripId(carId, tripId);
            List<Point> mpoints = dbc.GetMPointByCarAndTripId(carId, tripId);
            dbc.Close();

            for (int i = 1; i < timestamps.Count; i++) {
                if (timestamps[i].timestamp - timestamps[i - 1].timestamp > HoleSizeMinimumTime) {
                    tripHoles.Add(mpoints.Find(x => x.Id.Equals(timestamps[i - 1].Id)), mpoints.Find(x => x.Id.Equals(timestamps[i].Id)));
                }
            }

            return tripHoles;
        }

        public static Dictionary<Point, Point> ByDistance(Int16 carId, int tripId, double HoleSizeMinimumDistance) {
            Dictionary<Point, Point> tripHoles = new Dictionary<Point, Point>();

            DBController dbc = new DBController();
            List<Point> mpoints = dbc.GetMPointByCarAndTripId(carId, tripId);
            dbc.Close();

            for (int i = 1; i < mpoints.Count; i++) {
                if (mpoints[i].Mpoint.GetDistanceTo(mpoints[i - 1].Mpoint) > HoleSizeMinimumDistance) {
                    tripHoles.Add(mpoints[i-1], mpoints[i]);
                }
            }

            return tripHoles;
        }

        public static Dictionary<Point, Point> ByTimeAndDistance(Int16 carId, int tripId, TimeSpan HoleSizeMinimumTime, double HoleSizeMinimumDistance) {
            Dictionary<Point, Point> tripHoles = new Dictionary<Point, Point>();

            DBController dbc = new DBController();
            List<Timestamp> timestamps = dbc.GetTimestampsByCarAndTripId(carId, tripId);
            List<Point> mpoints = dbc.GetMPointByCarAndTripId(carId, tripId);
            dbc.Close();

            for (int i = 1; i < mpoints.Count; i++) {
                if (timestamps[i].timestamp - timestamps[i - 1].timestamp > HoleSizeMinimumTime && mpoints[i].Mpoint.GetDistanceTo(mpoints[i - 1].Mpoint) > HoleSizeMinimumDistance) {
                    tripHoles.Add(mpoints[i - 1], mpoints[i]);
                }
            }

            return tripHoles;
        }
    }
}
