using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using System.Data;

namespace CarDataProject {
    public class Point {
        public Point(DataRow row) {
            this.Id = row.Field<Int64>("id");
            this.Mpoint = row.Field<GeoCoordinate>("hdop");
        }

        public Point(Int64 Id, GeoCoordinate Mpoint) {
            this.Id = Id;
            this.Mpoint = Mpoint;
        }

        public Int64 Id { get; set; }
        public GeoCoordinate Mpoint { get; set; }
    }
}
