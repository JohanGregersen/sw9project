using System;

namespace CarDataProject {
    class TimeInformation {
        public Int64 TripId { get; }
        public Int64 EntryId { get; }
        public DateTime Timestamp { get; }
        public TimeSpan TimeToLag { get; }

        public TimeInformation(Int64 TripId, Int64 EntryId, DateTime Timestamp, TimeSpan TimeToLag) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this.Timestamp = Timestamp;
            this.TimeToLag = TimeToLag;
        }

        public TimeInformation(DateTime Timestamp) {
            this.Timestamp = Timestamp;
        }
    }
}
