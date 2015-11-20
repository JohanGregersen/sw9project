using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    public class INFATITrip {
        public Int64 TripId { get; set; }
        public int CarId { get; }
        public List<TemporalInformation> Timestamps { get; set; }
        public INFATITrip(int CarId) {
            this.CarId = CarId;
            Timestamps = new List<TemporalInformation>();
        }
    }
}
