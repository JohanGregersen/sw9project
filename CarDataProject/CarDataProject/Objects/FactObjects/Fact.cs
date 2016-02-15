using System;
using System.Data;
using System.Device.Location;
using NpgsqlTypes;
using System.Runtime.Serialization;
using System.Text;

namespace CarDataProject {
    [DataContract]
    public class Fact {
        //There is a workaround for get-only properties, that allows them to be part of the automatic serialization. 
        //It contains a private instance of the variable, and also is declared as _var-name, in the methods below

        public Int64 EntryId {
            get {
                return _EntryId;
            }
        }
        [DataMember(Name = "entryid")]
        private Int64 _EntryId;

        [DataMember(Name = "carid")]
        public int CarId { get; set; }

        public Int64 TripId {
            get {
                return _TripId;
            }
        }
        [DataMember(Name = "tripid")]
        private Int64 _TripId;

        public QualityInformation Quality {
            get {
                return _Quality;
            }
        }
        //[DataMember(Name = "quality")]
        private QualityInformation _Quality;

        public SegmentInformation Segment {
            get {
                return _Segment;
            }
        }
        //[DataMember (Name = "segment")]
        private SegmentInformation _Segment;

        [DataMember(Name = "temporal")]
        public TemporalInformation Temporal { get; set; }
        [DataMember(Name = "spatial")]
        public SpatialInformation Spatial { get; set; }
        [DataMember(Name = "measure")]
        public MeasureInformation Measure { get; set; }

        [DataMember(Name = "secondstolag")]
        private double SecondsToLag {
            get {
                return Temporal.SecondsToLag.TotalSeconds;
            }
            set { }
        }

        [DataMember(Name = "flag")]
        public FlagInformation Flag { get; set; }

        public Fact(Int64 EntryId, int CarId, Int64 TripId, QualityInformation Quality, SegmentInformation Segment, TemporalInformation Temporal, SpatialInformation Spatial, MeasureInformation Measure, FlagInformation Flag) {
            this._EntryId = EntryId;
            this.CarId = CarId;
            this._TripId = TripId;
            this._Quality = Quality;
            this._Segment = Segment;
            this.Temporal = Temporal;
            this.Spatial = Spatial;
            this.Measure = Measure;
            this.Flag = Flag;
        }

        public Fact(QualityInformation Quality, SegmentInformation Segment, TemporalInformation Temporal, SpatialInformation Spatial, MeasureInformation Measure) {
            this._Quality = Quality;
            this._Segment = Segment;
            this.Temporal = Temporal;
            this.Spatial = Spatial;
            this.Measure = Measure;
        }

        public Fact(Int64 EntryId, SegmentInformation Segment, TemporalInformation Temporal, SpatialInformation Spatial, MeasureInformation Measure) {
            this._Segment = Segment;
            this.Temporal = Temporal;
            this.Spatial = Spatial;
            this.Measure = Measure;
        }

        public Fact(Int64 EntryId, SegmentInformation Segment, TemporalInformation Temporal, MeasureInformation Measure) {
            this._EntryId = EntryId;
            this._Segment = Segment;
            this.Temporal = Temporal;
            this.Measure = Measure;
        }

        public Fact(Int64 EntryId, TemporalInformation Temporal, SpatialInformation Spatial) {
            this._EntryId = EntryId;
            this.Temporal = Temporal;
            this.Spatial = Spatial;
        }

        public Fact(DataRow row) {
            this._EntryId = row.Field<Int64>("entryid");
            this.CarId = row.Field<int>("carid");
            this._TripId = row.Field<Int64>("tripid");

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
            row["accelerating"] = row["accelerating"] is DBNull ? false : row["accelerating"];
            row["jerking"] = row["jerking"] is DBNull ? false : row["jerking"];
            row["braking"] = row["braking"] is DBNull ? false : row["braking"];
            row["steadyspeed"] = row["steadyspeed"] is DBNull ? false : row["steadyspeed"];
            this.Flag = new FlagInformation(row.Field<bool>("speeding"), row.Field<bool>("accelerating"), row.Field<bool>("jerking"), row.Field<bool>("braking"), row.Field<bool>("steadyspeed"));

            //Segment Information
            row["segmentid"] = row["segmentid"] is DBNull ? -1 : row["segmentid"];
            this._Segment = new SegmentInformation(row.Field<int>("segmentid"), row.Field<Int16>("maxspeed"));

            //Quality Information
            row["qualityid"] = row["qualityid"] is DBNull ? -1 : row["qualityid"];
            row["satellites"] = row["satellites"] is DBNull ? -1 : row["satellites"];
            row["hdop"] = row["hdop"] is DBNull ? -1 : row["hdop"];
            this._Quality = new QualityInformation(row.Field<Int16>("qualityid"), row.Field<Int16>("satellites"), (double)row.Field<Single>("hdop"));
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(EntryId);
            sb.Append(" ");
            sb.Append(TripId);

            return sb.ToString();

        }


    }
}
