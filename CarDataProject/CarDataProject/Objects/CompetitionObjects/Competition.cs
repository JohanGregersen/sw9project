using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.Serialization;
using System.Text;
using CarDataProject;

namespace CarDataProject {
    [DataContract]
    public class Competition {

        [DataMember(Name = "competitionid")]
        public Int16 CompetitionId { get; set; }
        [DataMember(Name = "competitionname")]
        public string CompetitionName { get; set; }
        [DataMember(Name = "starttemporal")]
        public TemporalInformation StartTemporal { get; set; }
        [DataMember(Name = "stoptemporal")]
        public TemporalInformation StopTemporal { get; set; }
        [DataMember(Name = "competitiondescription")]
        public string CompetitionDescription { get; set; }

        public Competition(Int16 CompetitionId, string CompetitionName, TemporalInformation StartTemporal, TemporalInformation StopTemporal, string CompetitionDescription) {
            this.CompetitionId = CompetitionId;
            this.CompetitionName = CompetitionName;
            this.StartTemporal = StartTemporal;
            this.StopTemporal = StopTemporal;
            this.CompetitionDescription = CompetitionDescription;
        }

        public Competition(DataRow row) {
            this.CompetitionId = row.Field<Int16>("competitionid");
            this.CompetitionName = row.Field<string>("competitionname");

            //Temporal Information
            row["startdateid"] = row["startdateid"] is DBNull ? "19700101" : row["startdateid"];
            row["starttimeid"] = row["starttimeid"] is DBNull ? "0" : row["starttimeid"];
            this.StartTemporal = new TemporalInformation(DateTimeHelper.ConvertToDateTime(row.Field<int>("startdateid"), row.Field<int>("starttimeid")));
            row["stopdateid"] = row["stopdateid"] is DBNull ? "19700101" : row["stopdateid"];
            row["stoptimeid"] = row["stoptimeid"] is DBNull ? "0" : row["stoptimeid"];
            this.StopTemporal = new TemporalInformation(DateTimeHelper.ConvertToDateTime(row.Field<int>("stopdateid"), row.Field<int>("stoptimeid")));

            if(row.Table.Columns.Contains("competitiondescription")) {
                row["competitiondescription"] = row["competitiondescription"] is DBNull ? "No Information" : row["competitiondescription"];
                this.CompetitionDescription = row.Field<string>("competitiondescription");
            }
        }
    }
}
