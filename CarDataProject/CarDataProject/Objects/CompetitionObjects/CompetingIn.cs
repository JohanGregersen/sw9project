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
    public class CompetingIn {

        [DataMember(Name = "competitionid")]
        public Int16 CompetitionId { get; set; }
        [DataMember(Name = "carid")]
        public Int16 CarId { get; set; }
        [DataMember(Name = "username")]
        public string Username { get; set; }
        [DataMember(Name = "score")]
        public double Score { get; set; }
        [DataMember(Name = "attempts")]
        public int Attempts { get; set; }

        public CompetingIn(Int16 CompetitionId, Int16 Carid, string Username, double Score, int Attempts) {
            this.CompetitionId = CompetitionId;
            this.CarId = CarId;
            this.Username = Username;
            this.Score = Score;
            this.Attempts = Attempts;
        }

        public CompetingIn(DataRow row) {
            this.CompetitionId = row.Field<Int16>("competitionid");
            this.CarId = row.Field<Int16>("carid");

            row["username"] = row["username"] is DBNull ? "" : row["username"];
            this.Username = row.Field<string>("username");

            row["score"] = row["score"] is DBNull ? 0.0 : row["score"];
            this.Score = (double)row.Field<Single>("score");

            row["attempts"] = row["attempts"] is DBNull ? 0 : row["attempts"];
            this.Attempts = row.Field<int>("attempts");


        }
    }
}
