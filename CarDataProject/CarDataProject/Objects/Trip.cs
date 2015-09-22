using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    public class Trip {
        public Trip() {

        }
        public List<Tuple<Int64, DateTime>> allTimestamps {
            get {
                return _allTS;
            }
            set {
                _allTS = value;
            }

        }
        private List<Tuple<Int64, DateTime>> _allTS = new List<Tuple<Int64, DateTime>>();

        public DateTime firstTimestamp { get; set; }
        public DateTime lastTimestamp { get; set; }
    }
}
