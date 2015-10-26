using System;

namespace CarDataProject {
    class SegmentInformation {
        public Int64 SegmentId { get; }
        public string RoadName { get; }
        public Int16 Oneway { get; }
        public Int16 Bridge { get; }
        public Int16 Tunnel { get; }
        public Int16 MaxSpeed { get; }
        //TODO RoadLines

        public SegmentInformation(Int64 SegmentId, string RoadName, Int16 Oneway, Int16 Bridge, Int16 Tunnel, Int16 MaxSpeed) {
            this.SegmentId = SegmentId;
            this.RoadName = RoadName;
            this.Oneway = Oneway;
            this.Bridge = Bridge;
            this.Tunnel = Tunnel;
            this.MaxSpeed = MaxSpeed;
        }

        public SegmentInformation(Int64 SegmentId) {
            this.SegmentId = SegmentId;
        }
    }
}
