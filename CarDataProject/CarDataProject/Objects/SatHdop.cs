using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CarDataProject {
    public class SatHdop {
        public SatHdop(DataRow row) {
            this.Id = row.Field<int>("id");
            this.Hdop = row.Field<Int16>("hdop");
            this.Sat= row.Field<Int16>("sat");
        }

        public int Id { get; set; }
        public Int16 Hdop { get; set; }
        public Int16 Sat { get; set; }
    }
}
