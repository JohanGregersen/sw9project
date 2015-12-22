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

        public static List<double> RoadTypeWeights;
        public static List<double> CriticalTimeWeights;
        public static List<double> SpeedingWeights;
        public static List<double> AccelerationWeights;
        public static List<double> BrakeWeights;
        public static List<double> JerkWeights;

        public static double BrakePrice;
        public static double AccelerationPrice;
        public static double JerkPrice;

        public static double A;
        public static double B;
        public static double C;
        public static double Poly;

        public Policy(List<Global.Enums.RoadType> roadTypes,
                      List<TimeInterval> timeIntervals,
                      List<Interval> speedingIntervals,
                      List<Interval> accelerationIntervals,
                      List<Interval> brakeIntervals,
                      List<Interval> jerkIntervals,
                      List<double> roadTypeWeights,
                      List<double> criticalTimeWeights,
                      List<double> speedingWeights,
                      List<double> accelerationWeights,
                      List<double> brakeWeights,
                      List<double> jerkWeights,
                      double brakePrice,
                      double accelerationPrice,
                      double jerkPrice,
                      double a,
                      double b,
                      double c,
                      double poly) {

            if (ListOutOfRange(roadTypes) ||
                ListOutOfRange(timeIntervals) ||
                ListOutOfRange(speedingIntervals) ||
                ListOutOfRange(accelerationIntervals) ||
                ListOutOfRange(brakeIntervals) ||
                ListOutOfRange(jerkIntervals) ||
                ListOutOfRange(roadTypeWeights) ||
                ListOutOfRange(criticalTimeWeights) ||
                ListOutOfRange(speedingWeights) ||
                ListOutOfRange(accelerationWeights) ||
                ListOutOfRange(brakeWeights) ||
                ListOutOfRange(jerkWeights)) {
                throw new ArgumentOutOfRangeException("List out of range: Only up to 8 intervals are supported");
            }

            RoadTypes = roadTypes;
            TimeIntervals = timeIntervals;
            SpeedingIntervals = speedingIntervals;
            AccelerationIntervals = accelerationIntervals;
            BrakeIntervals = brakeIntervals;
            JerkIntervals = jerkIntervals;
            RoadTypeWeights = roadTypeWeights;
            CriticalTimeWeights = criticalTimeWeights;
            SpeedingWeights = speedingWeights;
            AccelerationWeights = accelerationWeights;
            BrakeWeights = brakeWeights;
            JerkWeights = jerkWeights;
            BrakePrice = brakePrice;
            AccelerationPrice = accelerationPrice;
            JerkPrice = jerkPrice;
            A = a;
            B = b;
            C = c;
            Poly = poly;
        }

        private static bool ListOutOfRange<T>(List<T> list) {
            return (list.Count > 8);
        }
    }
}
