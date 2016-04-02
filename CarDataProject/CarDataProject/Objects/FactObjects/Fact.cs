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

        public Int64 LocalTripId {
            get {
                return _LocalTripId;
            }
        }
        [DataMember(Name = "localtripid")]
        private Int64 _LocalTripId;
        
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
            if (row.Table.Columns.Contains("entryid")) {
                this._EntryId = row.Field<Int64>("entryid");
            }
            if (row.Table.Columns.Contains("carid")) {
                this.CarId = row.Field<int>("carid");
            }
            if (row.Table.Columns.Contains("tripid")) {
                this._TripId = row.Field<Int64>("tripid");
            }

            if (row.Table.Columns.Contains("localtripid")) {
                this._LocalTripId = row.Field<Int64>("localtripid");
            }

            //Spatial Information
            if (row.Table.Columns.Contains("distancetolag") && row.Table.Columns.Contains("pathline")) {
                row["distancetolag"] = row["distancetolag"] is DBNull ? -1.0 : row["distancetolag"];
                row["pathline"] = row["pathline"] is DBNull ? null : row["pathline"];
                this.Spatial = new SpatialInformation(new GeoCoordinate(row.Field<double>("latitude"), row.Field<double>("longitude")), (double)row.Field<Single>("distancetolag"), row.Field<PostgisLineString>("pathline"));
            } else {
                this.Spatial = new SpatialInformation(new GeoCoordinate(row.Field<double>("latitude"), row.Field<double>("longitude")));
            }

            //Temporal Information
            if (row.Table.Columns.Contains("secondstolag")) {
                row["secondstolag"] = row["secondstolag"] is DBNull ? 0 : row["secondstolag"];
                this.Temporal = new TemporalInformation(DateTimeHelper.ConvertToDateTime(row.Field<int>("dateid"), row.Field<int>("timeid")), new TimeSpan(0, 0, row.Field<Int16>("secondstolag")));
            } else {
                this.Temporal = new TemporalInformation(DateTimeHelper.ConvertToDateTime(row.Field<int>("dateid"), row.Field<int>("timeid")));
            }

            //Measure Information
            if (row.Table.Columns.Contains("speed") && row.Table.Columns.Contains("acceleration") && row.Table.Columns.Contains("jerk")) {
                row["acceleration"] = row["acceleration"] is DBNull ? 0 : row["acceleration"];
                row["jerk"] = row["jerk"] is DBNull ? 0 : row["jerk"];
                this.Measure = new MeasureInformation((double)row.Field<Single>("speed"), (double)row.Field<Single>("acceleration"), (double)row.Field<Single>("jerk"));
            }

            //Flag Information
            if (row.Table.Columns.Contains("speeding")) {
                row["speeding"] = row["speeding"] is DBNull ? false : row["speeding"];
                row["accelerating"] = row["accelerating"] is DBNull ? false : row["accelerating"];
                row["jerking"] = row["jerking"] is DBNull ? false : row["jerking"];
                row["braking"] = row["braking"] is DBNull ? false : row["braking"];
                row["steadyspeed"] = row["steadyspeed"] is DBNull ? false : row["steadyspeed"];
                this.Flag = new FlagInformation(row.Field<bool>("speeding"), row.Field<bool>("accelerating"), row.Field<bool>("jerking"), row.Field<bool>("braking"), row.Field<bool>("steadyspeed"));
            }

            //Segment Information
            if (row.Table.Columns.Contains("segmentid") && row.Table.Columns.Contains("maxspeed")) {
                row["segmentid"] = row["segmentid"] is DBNull ? -1 : row["segmentid"];
                this._Segment = new SegmentInformation(row.Field<int>("segmentid"), row.Field<Int16>("maxspeed"));
            }

            //Quality Information
            if (row.Table.Columns.Contains("qualityid") && row.Table.Columns.Contains("satellites") && row.Table.Columns.Contains("hdop")) {
                row["qualityid"] = row["qualityid"] is DBNull ? -1 : row["qualityid"];
                row["satellites"] = row["satellites"] is DBNull ? -1 : row["satellites"];
                row["hdop"] = row["hdop"] is DBNull ? -1 : row["hdop"];
                this._Quality = new QualityInformation(row.Field<Int16>("qualityid"), row.Field<Int16>("satellites"), (double)row.Field<Single>("hdop"));
            } else if (row.Table.Columns.Contains("qualityid")) {
                this._Quality = new QualityInformation(-1, -1, -1);
            }
        }

        public Fact(dynamic jsonObj) {
            this.CarId = jsonObj.carid;
            this._TripId = jsonObj.tripid;
            this._LocalTripId = jsonObj.localtripid;

            //Spatial Information
            dynamic spatialObj = jsonObj.spatial;
            spatialObj.distancetolag = spatialObj.distancetolag == null ? -1.0 : spatialObj.distancetolag;
            spatialObj.pathline = spatialObj.pathline == null ? -1.0 : spatialObj.pathline;

            spatialObj.mpointlat = spatialObj.mpointlat == null ? 0 : spatialObj.mpointlat;
            spatialObj.mpointlng = spatialObj.mpointlng == null ? 0 : spatialObj.mpointlng;
            //spatialObj.pathline = spatialObj.pathline == null ? "" : spatialObj.pathline
            //No Pathline yet.
            this.Spatial = new SpatialInformation(new GeoCoordinate((double)spatialObj.mpointlat, (double)spatialObj.mpointlng), (double)spatialObj.distancetolag);

            //Temporal Information
            dynamic temporalObj = jsonObj.temporal;
            temporalObj.timestamp = temporalObj.timestamp == null ? 0 : temporalObj.timestamp;
            temporalObj.secondstolag = temporalObj.secondstolag == null ? 0 : temporalObj.secondstolag;
            
            this.Temporal = new TemporalInformation(new DateTime((long)temporalObj.timestamp), new TimeSpan(0, 0, (Int16)temporalObj.secondstolag));

            //Measure Information
            dynamic measureObj = jsonObj.measure;
            measureObj.speed = measureObj.speed == null ? 0 : measureObj.speed;
            measureObj.acceleration = measureObj.acceleration == null ? 0 : measureObj.acceleration;
            measureObj.jerk = measureObj.jerk == null ? 0 : measureObj.jerk;

            this.Measure = new MeasureInformation((double)measureObj.speed, (double)measureObj.acceleration, (double)measureObj.jerk);

            //Flag Information
            dynamic flagObj = jsonObj.flag;
            flagObj.speeding = flagObj.speeding == null ? false : flagObj.speeding;
            flagObj.accelerating = flagObj.accelerating == null ? false : flagObj.accelerating;
            flagObj.jerking = flagObj.jerking == null ? false : flagObj.jerking;
            flagObj.braking = flagObj.braking == null ? false : flagObj.braking;
            //flagObj.steadyspeed = flagObj.steadyspeed == null ? false : flagObj.steadyspeed;
            //this.Flag = new FlagInformation((bool)flagObj.speeding, (bool)flagObj.accelerating, (bool)flagObj.jerking, (bool)flagObj.braking, (bool)flagObj.steadyspeed);

            this.Flag = new FlagInformation((bool)flagObj.speeding, (bool)flagObj.accelerating, (bool)flagObj.jerking, (bool)flagObj.braking);

            //Segment Information
            //Implement using DataRow row as example

            //Quality Information
            //Implement using DataRow row as example
        }


        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("EntryId:" + EntryId);
            sb.AppendLine();
            sb.Append("TripId: " + TripId);
            sb.AppendLine();
            sb.Append("LocalTripId: " + LocalTripId);
            sb.AppendLine();
            sb.Append("CarId: " + CarId);
            sb.AppendLine();

            sb.Append("FlagInformation: " + "\n" + Flag.ToString());
            sb.AppendLine();
            sb.Append("MeasureInformation: " + "\n" + Measure.ToString());
            sb.AppendLine();
            sb.Append("SpatialInformation: " + "\n" + Spatial.ToString());
            sb.AppendLine();
            sb.Append("TemporalInformation: " + "\n" + Temporal.ToString());
            sb.AppendLine();

            return sb.ToString();

        }


    }
}
