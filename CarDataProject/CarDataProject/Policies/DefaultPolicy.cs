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
    }
}
