using System;
using System.Runtime.Serialization;

namespace CarDataProject {
    [DataContract]
    public class MeasureInformation {
        public Int64 TripId { get; }
        public Int64 EntryId { get; }

        [DataMember (Name = "speed")]
        public double Speed { get; set; }
        [DataMember(Name = "acceleration")]
        public double Acceleration { get; set; }
        [DataMember(Name = "jerk")]
        public double Jerk { get; set; }

        public MeasureInformation (Int64 TripId, Int64 EntryId, double Speed, double Acceleration, double Jerk) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this.Speed = Speed;
            this.Acceleration = Acceleration;
            this.Jerk = Jerk;
        }

        public MeasureInformation(double Speed, double Acceleration) {
            this.Speed = Speed;
            this.Acceleration = Acceleration;
        }

        public MeasureInformation(double Speed) {
            this.Speed = Speed;
        }

        public MeasureInformation(double Speed, double Acceleration, double Jerk) {
            this.Speed = Speed;
            this.Acceleration = Acceleration;
            this.Jerk = Jerk;
        }
    }
}
