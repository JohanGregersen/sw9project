using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    public class INFATIEntry {
        public DateTime Timestamp;
        public int CarId;
        public int UTMx;
        public int UTMy;
        public int UTMmx;
        public int UTMmy;
        public Int16 Sat;
        public Int16 Hdop;
        public Int16 Speed;
        public Int16 MaxSpeed;
        public Int64 SegmentId;
        public int QualityId;

        public INFATIEntry(DateTime timestamp, int UTMx, int UTMy, int UTMmx, int UTMmy, Int16 Sat, Int16 Hdop, Int16 Speed, Int16 MaxSpeed, Int64 SegmentId) {
            this.Timestamp = timestamp;
            this.UTMx = UTMx;
            this.UTMy = UTMy;
            this.UTMmx = UTMmx;
            this.UTMmy = UTMmy;
            this.Sat = Sat;
            this.Hdop = Hdop;
            this.Speed = Speed;
            this.MaxSpeed = MaxSpeed;
            this.SegmentId = SegmentId;
        }

        public INFATIEntry(DateTime timestamp, int UTMx, int UTMy, int UTMmx, int UTMmy, Int16 Sat, Int16 Hdop, Int16 Speed, Int16 MaxSpeed) {
            this.Timestamp = timestamp;
            this.UTMx = UTMx;
            this.UTMy = UTMy;
            this.UTMmx = UTMmx;
            this.UTMmy = UTMmy;
            this.Sat = Sat;
            this.Hdop = Hdop;
            this.Speed = Speed;
            this.MaxSpeed = MaxSpeed;
        }
    }
}
