using System;

namespace CarDataProject {
    public class TemporalInformation {
        public Int64 TripId { get; }
        public Int64 EntryId { get; }
        public DateTime Timestamp { get; }
        public TimeSpan SecondsToLag { get; }

        public TemporalInformation(Int64 TripId, Int64 EntryId, DateTime Timestamp, TimeSpan SecondsToLag) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this.Timestamp = Timestamp;
            this.SecondsToLag = SecondsToLag;
        }

        public TemporalInformation(Int64 EntryId, DateTime Timestamp) {
            this.EntryId = EntryId;
            this.Timestamp = Timestamp;
        }

        public TemporalInformation(DateTime Timestamp, TimeSpan SecondsToLag) {
            this.Timestamp = Timestamp;
            this.SecondsToLag = SecondsToLag;
        }

        public TemporalInformation(DateTime Timestamp) {
            this.Timestamp = Timestamp;
        }
    }
}
