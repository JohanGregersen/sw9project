using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CarDataProject {
    public class SatHdop {
        public SatHdop(DataRow row) {
            this.Id = row.Field<Int64>("id");
            this.Hdop = row.Field<Int64>("hdop");
            this.Sat= row.Field<Int64>("sat");
        }

        public Int64 Id { get; set; }
        public Int64 Hdop { get; set; }
        public Int64 Sat { get; set; }
    }
}
