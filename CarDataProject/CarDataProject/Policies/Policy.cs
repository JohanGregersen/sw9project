using System;
using System.Collections.Generic;

namespace CarDataProject {
    public class Policy {
        List<Global.Enums.RoadType> RoadTypes;
        public static List<TimeInterval> TimeIntervals;
        public static List<Interval> SpeedingIntervals;
        public static List<Interval> AccelerationIntervals;
        public static List<Interval> BrakeIntervals;
        public static List<Interval> JerkIntervals;

        public Policy(List<Global.Enums.RoadType> roadTypes,
                      List<TimeInterval> timeIntervals,
                      List<Interval> speedingIntervals,
                      List<Interval> accelerationIntervals,
                      List<Interval> brakeIntervals,
                      List<Interval> jerkIntervals) {

            if (roadTypes.Count > 8 ||
                timeIntervals.Count > 8 ||
                speedingIntervals.Count > 8 ||
                accelerationIntervals.Count > 8 ||
                brakeIntervals.Count > 8 ||
                jerkIntervals.Count > 8) {
                throw new ArgumentOutOfRangeException("Only up to 8 intervals are supported");
            }

            RoadTypes = roadTypes;
            TimeIntervals = timeIntervals;
            SpeedingIntervals = speedingIntervals;
            AccelerationIntervals = accelerationIntervals;
            BrakeIntervals = brakeIntervals;
            JerkIntervals = jerkIntervals;
        }
    }
}
