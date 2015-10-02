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


            //Legacy before we learned ST_Transform
            //this.point = new GeoCoordinate(row.Field<double>("xcoord"), row.Field<double>("ycoord"));
            //this.mpoint = new GeoCoordinate(row.Field<double>("mpx"), row.Field<double>("mpy"));
        }

        public CarLogEntry(List<string> row) {
            //Extract values from list into class fields
            this.id = Convert.ToInt32(row.ElementAt(0).Trim());
            this.entryid = Convert.ToInt64(row.ElementAt(1).Trim());
            this.carid = Convert.ToInt32(row.ElementAt(2).Trim());
            this.driverid = Convert.ToInt32(row.ElementAt(3).Trim());
            this.rdate = Convert.ToInt32(row.ElementAt(4).Trim());
            this.rtime = Convert.ToInt32(row.ElementAt(5).Trim());
            this.xcoord = Convert.ToInt32(row.ElementAt(6).Trim());
            this.ycoord = Convert.ToInt32(row.ElementAt(7).Trim());
            this.mpx = Convert.ToInt32(row.ElementAt(8).Trim());
            this.mpy = Convert.ToInt32(row.ElementAt(9).Trim());
            this.sat = Convert.ToInt16(row.ElementAt(10).Trim());
            this.hdop = Convert.ToInt16(row.ElementAt(11).Trim());
            this.maxspd = Convert.ToInt16(row.ElementAt(12).Trim());
            this.spd = Convert.ToInt16(row.ElementAt(13).Trim());
            this.strtcod = Convert.ToInt16(row.ElementAt(14).Trim());
            this.segmentkey = Convert.ToInt32(row.ElementAt(15).Trim());
            this.tripid = Convert.ToInt32(row.ElementAt(16).Trim());
            this.tripsegmentno = Convert.ToInt32(row.ElementAt(17).Trim());

            //Legacy before we learned ST_Transform
            //this.point = CoordinateHelper.UtmToLatLng(row.ElementAt(6), row.ElementAt(7), "32N");
            //this.mpoint = CoordinateHelper.UtmToLatLng(row.ElementAt(8), row.ElementAt(9), "32N");
        }
    }
}
