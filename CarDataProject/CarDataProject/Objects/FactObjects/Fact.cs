using System;

namespace CarDataProject {
    class Fact {
        public Int64 EntryId { get; }
        public int CarId { get; }
        public Int64 TripId { get; }
        public QualityInformation Quality { get; }
        public SegmentInformation Segment { get; }
        public TemporalInformation Temporal { get; }
        public SpatialInformation Spatial { get; }
        public MeasureInformation Measure { get; }
        public FlagInformation Flag { get; }

        public Fact(Int64 EntryId, int CarId, Int64 TripId, QualityInformation Quality, SegmentInformation Segment, TemporalInformation Temporal, SpatialInformation Spatial, MeasureInformation Measure, FlagInformation Flag) {
            this.EntryId = EntryId;
            this.CarId = CarId;
            this.TripId = TripId;
            this.Quality = Quality;
            this.Segment = Segment;
            this.Temporal = Temporal;
            this.Spatial = Spatial;
            this.Measure = Measure;
            this.Flag = Flag;
        }

        public Fact(QualityInformation Quality, SegmentInformation Segment, TemporalInformation Temporal, SpatialInformation Spatial, MeasureInformation Measure) {
            this.Quality = Quality;
            this.Segment = Segment;
            this.Temporal = Temporal;
            this.Spatial = Spatial;
            this.Measure = Measure;
        }
    }
}
