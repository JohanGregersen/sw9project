using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    public class Trip {
        public Trip() {

        }
        public List<Tuple<int, DateTime>> allTimestamps {
            get {
                return _allTS;
            }
            set {
                _allTS = value;
            }

        }
        private List<Tuple<int, DateTime>> _allTS = new List<Tuple<int, DateTime>>();

        public DateTime firstTimestamp { get; set; }
        public DateTime lastTimestamp { get; set; }
    }
}
