using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Device.Location;

namespace CarDataProject {
    public class CarLogEntry {
        public int id { get; set; }
        public Int64 entryid { get; set; }
        public int carid { get; set; }
        public int driverid { get; set; }
        public int rdate { get; set; }
        public int rtime { get; set; }
        public int xcoord { get; set; }
        public int ycoord { get; set; }
        public int mpx { get; set; }
        public int mpy { get; set; }
        public Int16 sat { get; set; }
        public Int16 hdop { get; set; }
        public Int16 maxspd { get; set; }
        public Int16 spd { get; set; }
        public Int16 strtcod { get; set; }
        public Int64? segmentkey { get; set; }
        public int? tripid { get; set; }
        public int? tripsegmentno { get; set; }
        public GeoCoordinate point { get; set; }
        public GeoCoordinate mpoint { get; set; }
        public int newtripid { get; set; }

        public CarLogEntry(DataRow row) {
            this.id = row.Field<int>("id");
            this.entryid = row.Field<Int64>("entryid");
            this.carid = row.Field<int>("carid");
            this.driverid = row.Field<int>("driverid");
            this.rdate = row.Field<int>("rdate");
            this.rtime = row.Field<int>("rtime");
            this.xcoord = row.Field<int>("xcoord");
            this.ycoord = row.Field<int>("ycoord");
            this.mpx = row.Field<int>("mpx");
            this.mpy = row.Field<int>("mpy");
            this.sat = row.Field<Int16>("sat");
            this.hdop = row.Field<Int16>("hdop");
            this.maxspd = row.Field<Int16>("maxspd");
            this.spd = row.Field<Int16>("spd");
            this.strtcod = row.Field<Int16>("strtcod");
            this.segmentkey = row.Field<Int64>("segmentkey");
            this.tripid = row.Field<Int16>("tripid");
            this.tripsegmentno = row.Field<int>("tripsegmentno");
            this.newtripid = row.Field<int>("newtripid");



            //this.point = new GeoCoordinate(row.Field<double>("xcoord"), row.Field<double>("ycoord"));
            //this.mpoint = new GeoCoordinate(row.Field<double>("mpx"), row.Field<double>("mpy"));
        }

        public CarLogEntry(List<string> row) {
            //Extract values from list into class fields
            this.id = Convert.ToInt32(row.ElementAt(0).Trim());
            this.entryid = Convert.ToInt64(row.ElementAt(1).Trim());
            this.carid = row.ElementAt(2);
            this.driverid = row.ElementAt(3);
            this.rdate = row.ElementAt(4);
            this.rtime = row.ElementAt(5);
            //this.point = CoordinateHelper.UtmToLatLng(row.ElementAt(6), row.ElementAt(7), "32N");
            //this.mpoint = CoordinateHelper.UtmToLatLng(row.ElementAt(8), row.ElementAt(9), "32N");
            this.sat = row.ElementAt(10);
            this.hdop = row.ElementAt(11);
            this.maxspd = row.ElementAt(12);
            this.spd = row.ElementAt(13);
            this.strtcod = row.ElementAt(14);
            this.segmentkey = row.ElementAtOrDefault(15);
            this.tripid = row.ElementAtOrDefault(16);
            this.tripsegmentno = row.ElementAtOrDefault(17);
        }
    }
}
