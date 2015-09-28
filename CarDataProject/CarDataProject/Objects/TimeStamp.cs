using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CarDataProject {
    public class Timestamp {
        public Timestamp(DataRow row) {
            this.Id = row.Field<Int64>("id");
            this.rdate = row.Field<Int64>("rdate");
            this.rtime = row.Field<Int64>("rtime");
        }

        public Int64 Id { get; set; }
        public Int64 rdate { get; set; }
        public Int64 rtime { get; set; }
        public DateTime timestamp { get; set; }


        public void StoreAsDateTimeFormat() {
            this.timestamp = DateTimeHelper.ConvertToDateTime(this.rdate, this.rtime);
        }



    }
}
