using System;
using System.Device.Location;
using NpgsqlTypes;

namespace CarDataProject {
    public class SpatialInformation {
        public Int64 TripId { get; }
        public Int64 EntryId { get; }
        public GeoCoordinate Point { get; }
        public GeoCoordinate MPoint { get; }
        public double DistanceToLag { get; }
        public PostgisLineString PathLine { get; }

        public SpatialInformation(Int64 TripId, Int64 EntryId, GeoCoordinate Point, GeoCoordinate MPoint, double DistanceToLag, PostgisLineString PathLine) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this.Point = Point;
            this.MPoint = MPoint;
            this.DistanceToLag = DistanceToLag;
            this.PathLine = PathLine;
        }

        public SpatialInformation(GeoCoordinate Point, GeoCoordinate MPoint) {
            this.Point = Point;
            this.MPoint = MPoint;
        }

        public SpatialInformation(Int64 EntryId, GeoCoordinate MPoint) {
            this.EntryId = EntryId;
            this.MPoint = MPoint;
        }
    }
}
