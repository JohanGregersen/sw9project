using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CarDataProject {
    public class Competition {
        public Int16 CompetitionId { get; set; }
        public int StartDateId { get; set; }
        public int StartTimeId { get; set; }
        public int StopDateId { get; set; }
        public int StopTimeId { get; set; }
        public string CompetitionDescription { get; set; }

        public Competition(Int16 CompetitionId, int StartDateId, int StartTimeId, int StopDateId, int StopTimeId, string CompetitionDescription) {
            this.CompetitionId = CompetitionId;
            this.StartDateId = StartDateId;
            this.StartTimeId = StartTimeId;
            this.StopDateId = StopDateId;
            this.StopTimeId = StopTimeId;
            this.CompetitionDescription = CompetitionDescription;
        }

        public Competition(DataRow row) {
            this.CompetitionId = row.Field<Int16>("competitionid");
            this.StartDateId = row.Field<int>("startdateid");
            this.StartTimeId = row.Field<int>("starttimeid");
            this.StopDateId = row.Field<int>("stopdateid");
            this.StopTimeId = row.Field<int>("stoptimeid");
            this.CompetitionDescription = row.Field<string>("competitiondescription");
        }
    }
}
