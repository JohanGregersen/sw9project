using System;

namespace CarDataProject {
    public class QualityInformation {
        public Int64 TripId { get; }
        public Int64 EntryId { get; }
        public Int16 QualityId { get; }
        public Int16 Sat { get; }
        public double Hdop { get; }

        public QualityInformation(Int16 QualityId) {
            this.QualityId = QualityId;
        }

        public QualityInformation(Int64 TripId, Int64 EntryId, Int16 Sat, double Hdop) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this.Sat = Sat;
            this.Hdop = Hdop;
        }

        public QualityInformation(Int64 EntryId, Int16 Sat, double Hdop) {
            this.EntryId = EntryId;
            this.Sat = Sat;
            this.Hdop = Hdop;
        }

        public QualityInformation(Int16 Sat, double Hdop) {
            this.Sat = Sat;
            this.Hdop = Hdop;
        }
    }
}
