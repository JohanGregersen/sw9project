using System;

namespace CarDataProject {
    class QualityInformation {
        public Int64 TripId { get; }
        public Int64 EntryId { get; }
        public Int16 Sat { get; }
        public Int16 Hdop { get; }

        public QualityInformation(Int64 TripId, Int64 EntryId, Int16 Sat, Int16 Hdop) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this.Sat = Sat;
            this.Hdop = Hdop;
        }

        public QualityInformation(Int16 Sat, Int16 Hdop) {
            this.Sat = Sat;
            this.Hdop = Hdop;
        }
    }
}
