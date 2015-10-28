using System;
using NpgsqlTypes;

namespace CarDataProject {
    public class SegmentInformation {
        public Int64 SegmentId { get; set; }
        public Int64 OSMId { get; }
        public string RoadName { get; }
        public Int16 RoadType { get; }
        public Int16 Oneway { get; }
        public Int16 Bridge { get; }
        public Int16 Tunnel { get; }
        public Int16 MaxSpeed { get; set; }
        public bool Direction { get; }
        public PostgisLineString RoadLine { get; }

        public SegmentInformation(Int64 SegmentId, Int64 OSMId, string RoadName, Int16 RoadType, Int16 Oneway, Int16 Bridge, Int16 Tunnel, Int16 MaxSpeed, bool Direction, PostgisLineString RoadLine) {
            this.SegmentId = SegmentId;
            this.OSMId = OSMId;
            this.RoadName = RoadName;
            this.RoadType = RoadType;
            this.Oneway = Oneway;
            this.Bridge = Bridge;
            this.Tunnel = Tunnel;
            this.MaxSpeed = MaxSpeed;
            this.Direction = Direction;
            this.RoadLine = RoadLine;
        }

        public static SegmentInformation WithId(Int64 SegmentId) {
            SegmentInformation segment = new SegmentInformation();
            segment.SegmentId = SegmentId;
            return segment;
        }

        public static SegmentInformation WithMaxSpeed(Int16 MaxSpeed) {
            SegmentInformation segment = new SegmentInformation();
            segment.MaxSpeed = MaxSpeed;
            return segment;
        }

        private SegmentInformation() {
        }
    }
}
