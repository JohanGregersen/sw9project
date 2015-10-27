using System;

namespace CarDataProject {
    class TemporalInformation {
        public Int64 TripId { get; }
        public Int64 EntryId { get; }
        public DateTime Timestamp { get; }
        public TimeSpan TimeToLag { get; }

        public TemporalInformation(Int64 TripId, Int64 EntryId, DateTime Timestamp, TimeSpan TimeToLag) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this.Timestamp = Timestamp;
            this.TimeToLag = TimeToLag;
        }

        public TemporalInformation(DateTime Timestamp) {
            this.Timestamp = Timestamp;
        }
    }
}
