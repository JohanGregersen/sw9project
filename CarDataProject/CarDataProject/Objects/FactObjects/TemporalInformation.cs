using System;
using System.Runtime.Serialization;

namespace CarDataProject {
    [DataContract]
    public class TemporalInformation {
        public Int64 TripId { get; }
        public Int64 EntryId { get; }

        public DateTime Timestamp {
            get {
                return _Timestamp;
            }
        }
        [DataMember(Name = "timestamp")]
        private DateTime _Timestamp;
        
        public TimeSpan SecondsToLag { get; }

        /*
        [DataMember(Name = "secondstolag")]
        private double SecondsToLagInt {
            get {
                return SecondsToLag.TotalSeconds;
            }
            set { }
        }
        */

        public TemporalInformation(Int64 TripId, Int64 EntryId, DateTime Timestamp, TimeSpan SecondsToLag) {
            this.TripId = TripId;
            this.EntryId = EntryId;
            this._Timestamp = Timestamp;
            this.SecondsToLag = SecondsToLag;
        }

        public TemporalInformation(Int64 EntryId, DateTime Timestamp) {
            this.EntryId = EntryId;
            this._Timestamp = Timestamp;
        }

        public TemporalInformation(DateTime Timestamp, TimeSpan SecondsToLag) {
            this._Timestamp = Timestamp;
            this.SecondsToLag = SecondsToLag;
        }

        public TemporalInformation(DateTime Timestamp) {
            this._Timestamp = Timestamp;
        }
    }
}
