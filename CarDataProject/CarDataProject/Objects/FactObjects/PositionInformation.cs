using System;
using System.Device.Location;

namespace CarDataProject {
    class PositionInformation {
        public Int64 TripId { get; }
        public Int64 EntryId { get; }
        public GeoCoordinate Point { get; }
        public GeoCoordinate MPoint { get; }
        public double DistanceToLag { get; }
        //TODO PathLine

        public PositionInformation(Int64 TripId, Int64 EntryId, GeoCoordinate Point, GeoCoordinate MPoint, double DistanceToLag) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this.Point = Point;
            this.MPoint = MPoint;
            this.DistanceToLag = DistanceToLag;
        }

        public PositionInformation(GeoCoordinate Point, GeoCoordinate MPoint) {
            this.Point = Point;
            this.MPoint = MPoint;
        }
    }
}
