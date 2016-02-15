using System;
using System.Runtime.Serialization;

namespace CarDataProject {
    [DataContract]    
    public class FlagInformation {
        public Int64 TripId { get; }
        public Int64 EntryId { get; }

        [DataMember (Name= "speeding")]
        public bool Speeding { get; set; }
        [DataMember(Name = "accelerating")]
        public bool Accelerating { get; set; }
        [DataMember(Name = "jerking")]
        public bool Jerking { get; set; }
        [DataMember(Name = "braking")]
        public bool Braking { get; set; }
        public bool SteadySpeed { get; set; }

        public FlagInformation(Int64 TripId, Int64 EntryId, bool Speeding, bool Accelerating, bool Jerking, bool Braking) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this.Speeding = Speeding;
            this.Accelerating = Accelerating;
            this.Jerking = Jerking;
            this.Braking = Braking;
        }

        public FlagInformation(bool Speeding, bool Accelerating, bool Jerking, bool Braking, bool SteadySpeed) {
            this.Speeding = Speeding;
            this.Accelerating = Accelerating;
            this.Jerking = Jerking;
            this.Braking = Braking;
            this.SteadySpeed = SteadySpeed;
        }
    }
}
