using System;
using System.Collections.Generic;

namespace CarDataProject {
    public static class TripGaps {
        public static Dictionary<SpatialInformation, SpatialInformation> ByTime(Int16 carId, Int64 tripId, TimeSpan MinimumGapTime) {
            Dictionary<SpatialInformation, SpatialInformation> tripGaps = new Dictionary<SpatialInformation, SpatialInformation>();

            DBController dbc = new DBController();
            List<Fact> facts = new List<Fact>(dbc.GetSpatioTemporalByCarIdAndTripId(carId, tripId));
            dbc.Close();

            for (int i = 1; i < facts.Count; i++) {
                if (facts[i].Temporal.Timestamp - facts[i - 1].Temporal.Timestamp > MinimumGapTime) {
                    tripGaps.Add(facts[i-1].Spatial, facts[i].Spatial);
                }
            }

            return tripGaps;
        }

        public static Dictionary<SpatialInformation, SpatialInformation> ByDistance(Int16 carId, Int64 tripId, double MinimumGapDistance) {
            Dictionary<SpatialInformation, SpatialInformation> tripGaps = new Dictionary<SpatialInformation, SpatialInformation>();

            DBController dbc = new DBController();
            List<Fact> facts = new List<Fact>(dbc.GetSpatioTemporalByCarIdAndTripId(carId, tripId));
            dbc.Close();

            for (int i = 1; i < facts.Count; i++) {
                if (facts[i].Spatial.Point.GetDistanceTo(facts[i - 1].Spatial.Point) > MinimumGapDistance) {
                    tripGaps.Add(facts[i - 1].Spatial, facts[i].Spatial);
                }
            }

            return tripGaps;
        }

        public static Dictionary<SpatialInformation, SpatialInformation> ByTimeAndDistance(Int16 carId, Int64 tripId, TimeSpan MinimumGapTime, double MinimumGapDistance) {
            Dictionary<SpatialInformation, SpatialInformation> tripGaps = new Dictionary<SpatialInformation, SpatialInformation>();

            DBController dbc = new DBController();
            List<Fact> facts = new List<Fact>(dbc.GetSpatioTemporalByCarIdAndTripId(carId, tripId));
            dbc.Close();

            for (int i = 1; i < facts.Count; i++) {
                if (facts[i].Temporal.Timestamp - facts[i - 1].Temporal.Timestamp > MinimumGapTime && facts[i].Spatial.MPoint.GetDistanceTo(facts[i - 1].Spatial.MPoint) > MinimumGapDistance) {
                    tripGaps.Add(facts[i - 1].Spatial, facts[i].Spatial);
                }
            }

            return tripGaps;
        }
    }
}
