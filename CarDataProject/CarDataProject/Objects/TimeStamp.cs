using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CarDataProject {
    public class Timestamp {

        public Timestamp(DataRow row) {
            this.Id = row.Field<int>("id");
            this.rdate = row.Field<int>("rdate");
            this.rtime = row.Field<int>("rtime");
            this.timestamp = DateTimeHelper.ConvertToDateTime(this.rdate, this.rtime);
        }

        public int Id { get; set; }
        public int rdate { get; set; }
        public int rtime { get; set; }
        public DateTime timestamp { get; set; }

    }
}
