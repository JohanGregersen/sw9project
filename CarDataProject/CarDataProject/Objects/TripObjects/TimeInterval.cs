using System;
using System.Collections.Generic;

namespace CarDataProject {
    public class TimeInterval : IComparable<TimeInterval> {
        public List<DayOfWeek> ActiveDays;
        public TimeSpan StartTime;
        public TimeSpan EndTime;

        public TimeInterval(List<DayOfWeek> activeDays, TimeSpan startTime, TimeSpan endTime) {
            this.ActiveDays = activeDays;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        public int CompareTo(TimeInterval that) {
            return this.StartTime.CompareTo(that.StartTime);
        }
    }
}
