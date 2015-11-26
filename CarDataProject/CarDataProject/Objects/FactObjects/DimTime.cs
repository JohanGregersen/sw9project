using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CarDataProject {
    public class DimTime {
        public int TimeId { get; set; }
        public Int16 Hour { get; set; }
        public Int16 Minute { get; set; }
        public Int16 Second { get; set; }

        public DimTime(int TimeId, Int16 Hour, Int16 Minute, Int16 Second) {
            this.TimeId = TimeId;
            this.Hour = Hour;
            this.Minute = Minute;
            this.Second = Second;
        }

        public DimTime(DataRow row) {
            this.TimeId = row.Field<int>("timeid");
            this.Hour = row.Field<Int16>("hour");
            this.Minute = row.Field<Int16>("minute");
            this.Second = row.Field<Int16>("second");
        }

    }
}
