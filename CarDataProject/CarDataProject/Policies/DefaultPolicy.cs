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

        public static List<TimeInterval> TimeIntervals = new List<TimeInterval> { new TimeInterval(weekdays, new TimeSpan(7, 0, 0), new TimeSpan(9, 0, 0)),
                                                                                  new TimeInterval(weekdays, new TimeSpan(15, 0, 0), new TimeSpan(17, 0, 0)),
                                                                                  new TimeInterval(weekends, new TimeSpan(9, 0, 0), new TimeSpan(13, 0, 0)),
                                                                                  new TimeInterval(weekends, new TimeSpan(20, 0, 0), new TimeSpan(23, 59, 59)),
                                                                                  new TimeInterval(weekends, new TimeSpan(0, 0, 0), new TimeSpan(0, 4, 0))

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

        public static List<Interval> AccelerationIntervals = new List<Interval> { new Interval(6, 7),
                                                                                  new Interval(7, 8),
                                                                                  new Interval(8, 9),
                                                                                  new Interval(9, 10),
                                                                                  new Interval(10, 11),
                                                                                  new Interval(11, 12),
                                                                                  new Interval(12, 13),
                                                                                  new Interval(13)

        };

        public static List<Interval> BrakeIntervals = new List<Interval> {  new Interval(8, 9),
                                                                            new Interval(9, 10),
                                                                            new Interval(10, 11),
                                                                            new Interval(11, 12),
                                                                            new Interval(12, 13),
                                                                            new Interval(13, 14),
                                                                            new Interval(14, 15),
                                                                            new Interval(15),
        };

        public static List<Interval> JerkIntervals = new List<Interval> { new Interval(8, 9),
                                                                          new Interval(9, 10),
                                                                          new Interval(10, 11),
                                                                          new Interval(11, 12),
                                                                          new Interval(12, 13),
                                                                          new Interval(13, 14),
                                                                          new Interval(14, 15),
                                                                          new Interval(15),
        };

        public static List<double> RoadTypeWeights = new List<double> { 0.8, 0.9, 0.95, 1.05, 1.1, 1.1, 1.2, 1.2 };
        public static List<double> CriticalTimeWeights = new List<double> { 1.2, 1.15, 1.025, 1.15, 1.4 };
        public static List<double> SpeedingWeights = new List<double> { 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2 };
        public static List<double> AccelerationWeights = new List<double> { 1.05, 1.1, 1.175, 1.275, 1.4, 1.55, 1.725, 2 };
        public static List<double> BrakeWeights = new List<double> { 1.05, 1.1, 1.175, 1.275, 1.4, 1.55, 1.725, 2 };
        public static List<double> JerkWeights = new List<double> { 1.05, 1.1, 1.175, 1.275, 1.4, 1.55, 1.725, 2 };

        public static double AccelerationPrice = 40;
        public static double BrakePrice = 65;
        public static double JerkPrice = 15;

        public static double A = 1.02;
        public static double B = 1.05;
        public static double C = 0;
        public static double Poly = 1.08;
    }
}
