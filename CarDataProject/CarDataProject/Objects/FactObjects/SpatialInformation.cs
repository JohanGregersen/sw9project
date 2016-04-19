using System;
//using System.Device.Location;
using GeoCoordinatePortable;
using NpgsqlTypes;
using System.Runtime.Serialization;
using System.Text;

namespace CarDataProject {
    [DataContract]
    public class SpatialInformation {
        public Int64 TripId { get; }
        public Int64 EntryId { get; }

        public GeoCoordinate Point { get; }


        public GeoCoordinate MPoint { get; }

        [DataMember (Name = "mpointlat")]
        private double GetMPointLat {
            get {
                return MPoint.Latitude;
            }
            set { }
        }

        [DataMember(Name = "mpointlng")]
        private double GetMPointLng {
            get {
                return MPoint.Longitude;
            }
            set { }
        }
        [DataMember(Name = "distancetolag")]
        public double DistanceToLag { get; set; }
        
        public PostgisLineString PathLine { get; }

        public SpatialInformation(Int64 TripId, Int64 EntryId, GeoCoordinate MPoint, double DistanceToLag, PostgisLineString PathLine) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this.MPoint = MPoint;
            this.DistanceToLag = DistanceToLag;
            this.PathLine = PathLine;
        }

        public SpatialInformation(Int64 EntryId, GeoCoordinate MPoint, double DistanceToLag) {
            this.EntryId = EntryId;
            this.MPoint = MPoint;
            this.DistanceToLag = DistanceToLag;
        }

        public SpatialInformation(GeoCoordinate MPoint, double DistanceToLag, PostgisLineString PathLine) {
            this.MPoint = MPoint;
            this.DistanceToLag = DistanceToLag;
            this.PathLine = PathLine;
        }

        public SpatialInformation(GeoCoordinate MPoint) {
            this.MPoint = MPoint;
        }

        public SpatialInformation(GeoCoordinate Point, GeoCoordinate MPoint) {
            this.Point = Point;
            this.MPoint = MPoint;
        }

        public SpatialInformation(Int64 TripId, GeoCoordinate Point) {
            this.TripId = TripId;
            this.Point = Point;
        }

        public SpatialInformation(Int64 TripId, Int64 EntryId, GeoCoordinate MPoint) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this.MPoint = MPoint;
        }

        public SpatialInformation(GeoCoordinate MPoint, double DistanceToLag) {
            this.MPoint = MPoint;
            this.DistanceToLag = DistanceToLag;
        }

        public SpatialInformation(double DistanceToLag) {
            this.DistanceToLag = DistanceToLag;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(" MPoint: " + "Lat: " + MPoint.Latitude + " Lng: " + MPoint.Longitude);
            sb.AppendLine();
            sb.Append(" DistanceToLag: " + DistanceToLag);
            //sb.AppendLine();
            //sb.Append("PathLine: " + PathLine.ToString());
            return sb.ToString();
        }
    }
}
