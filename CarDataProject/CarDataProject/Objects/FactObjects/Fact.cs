using System;
using System.Data;
using System.Device.Location;
using NpgsqlTypes;

namespace CarDataProject {
    public class Fact {
        public Int64 EntryId { get; }
        public int CarId { get; set; }
        public Int64 TripId { get; }
        public QualityInformation Quality { get; }
        public SegmentInformation Segment { get; }
        public TemporalInformation Temporal { get; set; }
        public SpatialInformation Spatial { get; set; }
        public MeasureInformation Measure { get; set; }
        public FlagInformation Flag { get; set; }
        public DimDate Date { get; }
        public DimTime Time { get; }

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

        public Fact(Int64 EntryId, SegmentInformation Segment, TemporalInformation Temporal, SpatialInformation Spatial, MeasureInformation Measure) {
            this.Segment = Segment;
            this.Temporal = Temporal;
            this.Spatial = Spatial;
            this.Measure = Measure;
        }

        public Fact(Int64 EntryId, SegmentInformation Segment, TemporalInformation Temporal, MeasureInformation Measure) {
            this.EntryId = EntryId;
            this.Segment = Segment;
            this.Temporal = Temporal;
            this.Measure = Measure;
        }

        public Fact(Int64 EntryId, TemporalInformation Temporal, SpatialInformation Spatial) {
            this.EntryId = EntryId;
            this.Temporal = Temporal;
            this.Spatial = Spatial;
        }

        public Fact(DataRow row) {
            this.EntryId = row.Field<Int64>("entryid");
            this.CarId = row.Field<int>("carid");
            this.TripId = row.Field<Int64>("tripid");

            //Spatial Information
            this.Spatial = new SpatialInformation(new GeoCoordinate(row.Field<double>("latitude"), row.Field<double>("longitude")));

            //Temporal Information
            this.Temporal = new TemporalInformation(DateTimeHelper.ConvertToDateTime(row.Field<int>("dateid"), row.Field<int>("timeid")));

            //Measure Information
            this.Measure = new MeasureInformation((double)row.Field<Single>("speed"));

            //Segment Information
            if(row["segmentid"] is DBNull) {
                row["segmentid"] = 0;
            }
            this.Segment = new SegmentInformation(row.Field<int>("segmentid"), row.Field<Int16>("maxspeed"));

        }
    }
}
