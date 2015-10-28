using System;

namespace CarDataProject {
    public class FlagInformation {
        public Int64 TripId { get; }
        public Int64 EntryId { get; }
        public bool Speeding { get; }
        public bool Braking { get; }

        public FlagInformation(Int64 TripId, Int64 EntryId, bool Speeding, bool Braking) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this.Speeding = Speeding;
            this.Braking = Braking;
        }
    }
}
