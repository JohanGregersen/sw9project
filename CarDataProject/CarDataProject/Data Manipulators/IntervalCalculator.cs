using System;
using System.Collections.Generic;

namespace CarDataProject {
    public static class IntervalCalculator {
        public static Dictionary<Global.Enums.RoadType, double> RoadType(Trip trip, List<Fact> facts) {
            DBController dbc = new DBController();
            List<Global.Enums.RoadType> roadTypes = dbc.GetRoadTypesByTripId(trip.TripId);
            dbc.Close();

            Dictionary<Global.Enums.RoadType, double> metersDistribution = new Dictionary<Global.Enums.RoadType, double>();

            //Populate dictionary with all policy-required roadtypes
            foreach (Global.Enums.RoadType roadType in DefaultPolicy.RoadTypes) {
                metersDistribution.Add(roadType, 0);
            }

            //Calculate meters driven on each roadtype
            for (int i = 1; i < facts.Count; i++) {
                metersDistribution[roadTypes[i]] += facts[i].Spatial.DistanceToLag;
            }

            //Calculate the distribution in percentages of whole trip
            foreach (Global.Enums.RoadType roadType in DefaultPolicy.RoadTypes) {
                metersDistribution[roadType] /= trip.MetersDriven * 100;
            }

            return metersDistribution;
        }

        public static Dictionary<TimeInterval , double> CriticalTime(Trip trip, List<Fact> facts) {
            Dictionary<TimeInterval, double> criticalTimes = new Dictionary<TimeInterval, double>();

            //Populate dictionary with all policy-required time-intervals
            foreach (TimeInterval interval in DefaultPolicy.TimeIntervals) {
                criticalTimes.Add(interval, 0);
            }

            //Calculate meters driven in every interval
            for (int i = 1; i < facts.Count; i++) {
                foreach(TimeInterval interval in DefaultPolicy.TimeIntervals) {
                    if(interval.ActiveDays.Contains(facts[i].Temporal.Timestamp.DayOfWeek) && interval.StartTime <= facts[i].Temporal.Timestamp.TimeOfDay && interval.EndTime >= facts[i].Temporal.Timestamp.TimeOfDay) {
                        criticalTimes[interval] += facts[i].Spatial.DistanceToLag;
                    }
                }
            }

            //Calculate the distribution in percentages of whole trip
            for(int i = 0; i < DefaultPolicy.TimeIntervals.Count; i++) {
                criticalTimes[DefaultPolicy.TimeIntervals[i]] /= trip.MetersDriven * 100;
            }

            return criticalTimes;
        }

        public static List<double> Speeding() {
            List<double> intervals = new List<double>();

            return intervals;
        }

        public static List<double> Acceleration() {
            List<double> intervals = new List<double>();

            return intervals;
        }

        public static List<double> Brake() {
            List<double> intervals = new List<double>();

            return intervals;
        }

        public static List<double> Jerk() {
            List<double> intervals = new List<double>();

            return intervals;
        }
    }
}
