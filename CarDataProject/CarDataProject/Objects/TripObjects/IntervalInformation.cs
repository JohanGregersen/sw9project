using System;
using System.Runtime.Serialization;
namespace CarDataProject {
    [DataContract]
    public class IntervalInformation {
        public int CarId { get; }
        public Int64 TripId { get; }
        [DataMember(Name = "roadtype")]
        public Int64 RoadTypesInterval { get; set; }
        [DataMember(Name = "criticaltime")]
        public Int64 CriticalTimeInterval { get; set; }
        [DataMember(Name = "speed")]
        public Int64 SpeedInterval { get; set; }
        [DataMember(Name = "acceleration")]
        public Int64 AccelerationInterval { get; set; }
        [DataMember(Name = "jerk")]
        public Int64 JerkInterval { get; set; }
        [DataMember(Name = "braking")]
        public Int64 BrakingInterval { get; set; }

        public IntervalInformation(int CarId, Int64 TripId) {
            this.CarId = CarId;
            this.TripId = TripId;
        }

        public IntervalInformation(int CarId, Int64 TripId, Int64 RoadTypesInterval, Int64 CriticalTimeInterval, Int64 SpeedInterval, Int64 AccelerationInterval, Int64 JerkInterval, Int64 BrakingInterval) {
            this.CarId = CarId;
            this.TripId = TripId;
            this.RoadTypesInterval = RoadTypesInterval;
            this.CriticalTimeInterval = CriticalTimeInterval;
            this.SpeedInterval = SpeedInterval;
            this.AccelerationInterval = AccelerationInterval;
            this.JerkInterval = JerkInterval;
            this.BrakingInterval = BrakingInterval;
        }
    }
}
