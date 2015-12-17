using System;


namespace CarDataProject {
    public class IntervalInformation {
        public int CarId { get; }
        public Int64 TripId { get; }
        public Int64 RoadTypesInterval { get; set; }
        public Int64 CriticalTimeInterval { get; set; }
        public Int64 SpeedInterval { get; set; }
        public Int64 AccelerationInterval { get; set; }
        public Int64 JerkInterval { get; set; }
        public Int64 BrakingInterval { get; set; }

        public IntervalInformation(int CarId, Int64 TripId) {
            this.CarId = CarId;
            this.TripId = TripId;
        }

        public IntervalInformation(int CarId, Int64 TripId, Int64 RoadTypesInterval, Int64 CriticalTimeInterval, Int64 SpeedInterval, Int64 AccelerationInterval, Int64 JerkInvertal, Int64 BrakingInterval) {
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
