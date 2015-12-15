using System;
using System.Collections.Generic;

namespace CarDataProject {
    public class INFATITrip {
        public Int64 TripId { get; set; }
        public int CarId { get; }
        public List<TemporalInformation> Timestamps { get; set; }

        public INFATITrip(int CarId) {
            this.CarId = CarId;
            this.Timestamps = new List<TemporalInformation>();
        }
    }
}
