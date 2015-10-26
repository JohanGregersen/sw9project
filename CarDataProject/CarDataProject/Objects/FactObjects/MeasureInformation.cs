using System;

namespace CarDataProject {
    class MeasureInformation {
        public Int64 TripId { get; }
        public Int64 EntryId { get; }
        public double Speed { get; }
        public double Acceleration { get; }
        public double Jerk { get; }

        public MeasureInformation (Int64 TripId, Int64 EntryId, double Speed, double Acceleration, double Jerk) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this.Speed = Speed;
            this.Acceleration = Acceleration;
            this.Jerk = Jerk;
        }

        public MeasureInformation(double Speed) {
            this.Speed = Speed;
        }
    }
}
