using System;

namespace CarDataProject {
    public class Interval : IComparable<Interval> {
        double Start;
        double? End = null;

        public Interval(double start, double end) {
            this.Start = start;
            this.End = end;
        }

        public Interval(double start) {
            this.Start = start;
        }

        public int CompareTo(Interval that) {
            return this.Start.CompareTo(that.Start);
        }

        public bool Contains(double value) {
            //Special case - If two timestamps is logged on same second, value can be infinity
            if(double.IsInfinity(value)) {
                if (End.HasValue) {
                    return false;
                } else { return true; }
            }

            if (Start <= value) {
                if (End.HasValue) {
                    if (End > value) {
                        return true;
                    } else { return false; }
                } else { return true; }
            } else { return false; }
        }
    }
}
