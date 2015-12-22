using System;
using System.Collections.Generic;

namespace CarDataProject {
    public static class DefaultPolicy {
        //Helper data
        private static List<DayOfWeek> weekdays = new List<DayOfWeek> { DayOfWeek.Monday,
                                                                 DayOfWeek.Tuesday,
                                                                 DayOfWeek.Wednesday,
                                                                 DayOfWeek.Thursday,
                                                                 DayOfWeek.Friday };

        private static List<DayOfWeek> weekends = new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday };

        //Policies
        public static List<Global.Enums.RoadType> RoadTypes = new List<Global.Enums.RoadType> { Global.Enums.RoadType.motorway,
                                                                                                Global.Enums.RoadType.trunk,
                                                                                                Global.Enums.RoadType.primary,
                                                                                                Global.Enums.RoadType.secondary,
                                                                                                Global.Enums.RoadType.tertiary,
                                                                                                Global.Enums.RoadType.unclassified,
                                                                                                Global.Enums.RoadType.residential,
                                                                                                Global.Enums.RoadType.service };

        public static List<TimeInterval> TimeIntervals = new List<TimeInterval> { new TimeInterval(weekdays, new TimeSpan(7, 0, 0), new TimeSpan(9, 30, 0)),
                                                                                  new TimeInterval(weekdays, new TimeSpan(14, 0, 0), new TimeSpan(17, 0, 0)),
                                                                                  new TimeInterval(weekends, new TimeSpan(9, 0, 0), new TimeSpan(13, 0, 0)),
                                                                                  new TimeInterval(weekends, new TimeSpan(20, 0, 0), new TimeSpan(23, 30, 0)),
        };
        public static List<Interval> SpeedingIntervals = new List<Interval> { new Interval(0.001, 10),
                                                                              new Interval(10, 20),
                                                                              new Interval(20, 30),
                                                                              new Interval(30, 40),
                                                                              new Interval(40, 50),
                                                                              new Interval(50, 60),
                                                                              new Interval(60, 70),
                                                                              new Interval(70)
        };

        public static List<Interval> AccelerationIntervals = new List<Interval> { new Interval(0.001, 3),
                                                                                  new Interval(3, 5),
                                                                                  new Interval(5, 7),
                                                                                  new Interval(7, 8),
                                                                                  new Interval(8, 9),
                                                                                  new Interval(9, 10),
                                                                                  new Interval(10, 11),
                                                                                  new Interval(11)
        };

        public static List<Interval> BrakeIntervals = new List<Interval> { new Interval(0.001, 3),
                                                                           new Interval(3, 5),
                                                                           new Interval(5, 7),
                                                                           new Interval(7, 8),
                                                                           new Interval(8, 9),
                                                                           new Interval(9, 10),
                                                                           new Interval(10, 11),
                                                                           new Interval(11)
        };

        public static List<Interval> JerkIntervals = new List<Interval> { new Interval(0.001, 1),
                                                                          new Interval(1, 2),
                                                                          new Interval(2, 3),
                                                                          new Interval(3, 4),
                                                                          new Interval(4, 5),
                                                                          new Interval(5, 6),
                                                                          new Interval(6, 7),
                                                                          new Interval(7)
        };

        public static List<double> RoadTypeWeights = new List<double> { 1, 1, 1, 1.05, 1.1, 1.1, 1.3, 1.3 };
        public static List<double> CriticalTimeWeights = new List<double> { 1.2, 1.15, 1.05, 1.1 };
        public static List<double> SpeedingWeights = new List<double> { 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2 };
        public static List<double> AccelerationWeights = new List<double> { 1, 1, 1, 1.1, 1.2, 1.4, 1.6, 1.9 };
        public static List<double> BrakeWeights = new List<double> { 1, 1, 1, 1.1, 1.2, 1.4, 1.6, 1.9 };
        public static List<double> JerkWeights = new List<double> { 1, 1, 1, 1.1, 1.2, 1.4, 1.6, 1.9 };

        public static double BrakePrice = 1.5;
        public static double AccelerationPrice = 1;
        public static double JerkPrice = 1.2;

        public static double A = 1.02;
        public static double B = 1.05;
        public static double C = 0;
        public static double Poly = 1.08;
    }
}
