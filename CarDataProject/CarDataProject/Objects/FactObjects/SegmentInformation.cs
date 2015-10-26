using System;
using NpgsqlTypes;

namespace CarDataProject {
    class SegmentInformation {
        public Int64 SegmentId { get; }
        public Int64 OSMId { get; }
        public string RoadName { get; }
        public Int16 RoadType { get; }
        public Int16 Oneway { get; }
        public Int16 Bridge { get; }
        public Int16 Tunnel { get; }
        public Int16 SpeedForward { get; }
        public Int16 SpeedBackward { get; }
        public bool Direction { get; }
        public PostgisLineString RoadLine { get; }

        public SegmentInformation(Int64 SegmentId, Int64 OSMId, string RoadName, Int16 RoadType, Int16 Oneway, Int16 Bridge, Int16 Tunnel, Int16 SpeedForward, Int16 SpeedBackward, bool Direction, PostgisLineString RoadLine) {
            this.SegmentId = SegmentId;
            this.OSMId = OSMId;
            this.RoadName = RoadName;
            this.RoadType = RoadType;
            this.Oneway = Oneway;
            this.Bridge = Bridge;
            this.Tunnel = Tunnel;
            this.SpeedForward = SpeedForward;
            this.SpeedBackward = SpeedBackward;
            this.Direction = Direction;
            this.RoadLine = RoadLine;
        }

        public SegmentInformation(Int64 SegmentId) {
            this.SegmentId = SegmentId;
        }
    }
}
