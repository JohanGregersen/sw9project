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
            this.Id = row.Field<int>("id");
            this.Mpoint = row.Field<GeoCoordinate>("hdop");
        }

        public Point(int Id, GeoCoordinate Mpoint) {
            this.Id = Id;
            this.Mpoint = Mpoint;
        }

        public int Id { get; set; }
        public GeoCoordinate Mpoint { get; set; }
    }
}
