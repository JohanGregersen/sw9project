using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;
using NpgsqlTypes;
using System.Device.Location;
using Newtonsoft.Json;

namespace CarDataProject {
    public class CarLogEntry {
        public Int64 id { get; set; }
        public Int64 entryid { get; set; }
        public Int64 carid { get; set; }
        public Int64 driverid { get; set; }
        public Int64 rdate { get; set; }
        public Int64 rtime { get; set; }
        public GeoCoordinate point { get; set; }
        public GeoCoordinate mpoint { get; set; }
        public Int64 sat { get; set; }
        public Int64 hdop { get; set; }
        public Int64 maxspd { get; set; }
        public Int64 spd { get; set; }
        public Int64 strtcod { get; set; }
        public Int64? segmentkey { get; set; }
        public Int64? tripid { get; set; }
        public Int64? tripsegmentno { get; set; }

        public CarLogEntry(DataRow row) {
            this.id = row.Field<Int64>("id");
            this.entryid = row.Field<Int64>("entryid");
            this.carid = row.Field<Int64>("carid");
            this.driverid = row.Field<Int64>("driverid");
            this.rdate = row.Field<Int64>("rdate");
            this.rtime = row.Field<Int64>("rtime");

            this.point = new GeoCoordinate(row.Field<double>("xcoord"), row.Field<double>("ycoord"));
            this.mpoint = new GeoCoordinate(row.Field<double>("mpx"), row.Field<double>("mpy"));

            this.sat = row.Field<Int64>("sat");
            this.hdop = row.Field<Int64>("hdop");
            this.maxspd = row.Field<Int64>("maxspd");
            this.spd = row.Field<Int64>("spd");
            this.strtcod = row.Field<Int64>("strtcod");
            this.segmentkey = row.Field<Int64>("segmentkey");
            this.tripid = row.Field<Int64>("tripid");
            this.tripsegmentno = row.Field<Int64>("tripsegmentno");
        }

        public CarLogEntry(List<Int64> row) {
            //Extract values from list into class fields
            this.id = row.ElementAt(0);
            this.entryid = row.ElementAt(1);
            this.carid = row.ElementAt(2);
            this.driverid = row.ElementAt(3);
            this.rdate = row.ElementAt(4);
            this.rtime = row.ElementAt(5);
            this.point = Utility.UtmToLatLng(row.ElementAt(6), row.ElementAt(7), "32N");
            this.mpoint = Utility.UtmToLatLng(row.ElementAt(8), row.ElementAt(9), "32N");
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
