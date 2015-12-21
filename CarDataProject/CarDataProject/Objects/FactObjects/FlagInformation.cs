using System;

namespace CarDataProject {
    public class FlagInformation {
        public Int64 TripId { get; }
        public Int64 EntryId { get; }
        public bool Speeding { get; }
        public bool Accelerating { get; }
        public bool Jerking { get; }
        public bool Braking { get; }
        public bool SteadySpeed { get; }

        public FlagInformation(Int64 TripId, Int64 EntryId, bool Speeding, bool Accelerating, bool Jerking, bool Braking) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this.Speeding = Speeding;
            this.Accelerating = Accelerating;
            this.Jerking = Jerking;
            this.Braking = Braking;
        }

        public FlagInformation(bool Speeding, bool Accelerating, bool Jerking, bool Braking, bool SteadySpeed) {
            this.Speeding = Speeding;
            this.Accelerating = Accelerating;
            this.Jerking = Jerking;
            this.Braking = Braking;
            this.SteadySpeed = SteadySpeed;
        }
    }
}
