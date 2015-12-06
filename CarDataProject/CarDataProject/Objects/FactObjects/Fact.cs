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
            row["distancetolag"] = row["distancetolag"] is DBNull ? -1.0 : row["distancetolag"];
            row["pathline"] = row["pathline"] is DBNull ? null : row["pathline"];
            this.Spatial = new SpatialInformation(new GeoCoordinate(row.Field<double>("latitude"), row.Field<double>("longitude")), (double)row.Field<Single>("distancetolag"), row.Field<PostgisLineString>("pathline"));

            //Temporal Information
            row["secondstolag"] = row["secondstolag"] is DBNull ? 0 : row["secondstolag"];
            this.Temporal = new TemporalInformation(DateTimeHelper.ConvertToDateTime(row.Field<int>("dateid"), row.Field<int>("timeid")), new TimeSpan(0, 0, row.Field<Int16>("secondstolag")));

            //Measure Information
            row["acceleration"] = row["acceleration"] is DBNull ? 0 : row["acceleration"];
            row["jerk"] = row["jerk"] is DBNull ? 0 : row["jerk"];
            this.Measure = new MeasureInformation((double)row.Field<Single>("speed"), (double)row.Field<Single>("acceleration"), (double)row.Field<Single>("jerk"));

            //Flag Information
            row["speeding"] = row["speeding"] is DBNull ? false : row["speeding"];
            row["braking"] = row["braking"] is DBNull ? false : row["braking"];
            row["steadyspeed"] = row["steadyspeed"] is DBNull ? false : row["steadyspeed"];
            this.Flag = new FlagInformation(row.Field<bool>("speeding"), row.Field<bool>("braking"), row.Field<bool>("steadyspeed"));

            //Segment Information
            row["segmentid"] = row["segmentid"] is DBNull ? -1 : row["segmentid"];
            this.Segment = new SegmentInformation(row.Field<int>("segmentid"), row.Field<Int16>("maxspeed"));

            //Quality Information
            row["qualityid"] = row["qualityid"] is DBNull ? -1 : row["qualityid"];
            this.Quality = new QualityInformation(row.Field<Int16>("qualityid"));
        }
    }
}
