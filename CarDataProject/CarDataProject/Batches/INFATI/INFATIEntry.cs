﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    class INFATIEntry {
        public DateTime Timestamp;
        public int UTMx;
        public int UTMy;
        public int UTMmx;
        public int UTMmy;
        public Int16 Sat;
        public Int16 Hdop;
        public Int16 Speed;
        public Int64 SegmentId;

        public INFATIEntry(DateTime timestamp, int UTMx, int UTMy, int UTMmx, int UTMmy, Int16 Sat, Int16 Hdop, Int16 Speed, Int64 SegmentId) {
            this.Timestamp = timestamp;
            this.UTMx = UTMx;
            this.UTMy = UTMy;
            this.UTMmx = UTMmx;
            this.UTMmy = UTMmy;
            this.Sat = Sat;
            this.Hdop = Hdop;
            this.Speed = Speed;
            this.SegmentId = SegmentId;
        }

        public INFATIEntry(DateTime timestamp, int UTMx, int UTMy, int UTMmx, int UTMmy, Int16 Sat, Int16 Hdop, Int16 Speed) {
            this.Timestamp = timestamp;
            this.UTMx = UTMx;
            this.UTMy = UTMy;
            this.UTMmx = UTMmx;
            this.UTMmy = UTMmy;
            this.Sat = Sat;
            this.Hdop = Hdop;
            this.Speed = Speed;
        }
    }
}