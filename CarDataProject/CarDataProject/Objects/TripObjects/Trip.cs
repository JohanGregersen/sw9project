using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace CarDataProject {
    [DataContract]
    public class Trip {
        [DataMember(Name = "tripid")]
        public Int64 TripId { get; set; }
        [DataMember(Name = "prevtripid")]
        public Int64 PreviousTripId { get; set; }
        [DataMember(Name = "carid")]
        public int CarId { get; set; }
        [DataMember(Name = "starttemporal")]
        public TemporalInformation StartTemporal { get; set; }
        [DataMember(Name = "endtemporal")]
        public TemporalInformation EndTemporal { get; set; }
        public TimeSpan SecondsDriven { get; set; }
        [DataMember(Name = "secondsdriven")]
        private double SecondsDrivenInt {
            get {
                return SecondsDriven.TotalSeconds;
            }
            set { }
        }
        [DataMember(Name = "metersdriven")]
        public double MetersDriven { get; set; }
        [DataMember(Name = "price")]
        public double Price { get; set; }
        [DataMember(Name = "optimalscore")]
        public double OptimalScore { get; set; }
        [DataMember(Name = "tripscore")]
        public double TripScore { get; set; }
        [DataMember(Name = "scorepercentage")]
        public double ScorePercentage {
            get {
                return (TripScore / OptimalScore * 100) - 100;
            }
            set { }
        }
        [DataMember(Name = "jerkcount")]
        public Int16 JerkCount { get; set; }
        [DataMember(Name = "brakecount")]
        public Int16 BrakeCount { get; set; }
        [DataMember(Name = "accelerationcount")]
        public Int16 AccelerationCount { get; set; }
        [DataMember(Name = "meterssped")]
        public double MetersSped { get; set; }
        public TimeSpan TimeSped { get; set; }
        [DataMember(Name = "timesped")]
        private double TimeSpedInt {
            get {
                return TimeSped.TotalSeconds;
            }
            set { }
        }

        [DataMember(Name = "roadtypescore")]
        public double RoadTypeScore { get; set; }
        [DataMember(Name = "criticaltimescore")]
        public double CriticalTimeScore { get; set; }
        [DataMember(Name = "speedingscore")]
        public double SpeedingScore { get; set; }
        [DataMember(Name = "accelerationscore")]
        public double AccelerationScore { get; set; }
        [DataMember(Name = "brakescore")]
        public double BrakeScore { get; set; }
        [DataMember(Name = "jerkscore")]
        public double JerkScore { get; set; }

        public double SteadySpeedDistance { get; set; }
        public TimeSpan SteadySpeedTime { get; set; }

        public TimeSpan SecondsToLag { get; set; }
        [DataMember(Name = "secondstolag")]
        private double SecondsToLagInt {
            get {
                return SecondsToLag.TotalSeconds;
            }
            set { }
        }

        public IntervalInformation IntervalInformation {get; set; }
        public double DataQuality { get; set; }

        public Trip(Int64 TripId, int CarId) {
            this.TripId = TripId;
            this.CarId = CarId;
        }

        public Trip(Int64 TripId, int CarId, TimeSpan SecondsDriven, double MetersDriven, double Price, double OptimalScore, double TripScore) {
            this.TripId = TripId;
            this.CarId = CarId;
            this.SecondsDriven = SecondsDriven;
            this.MetersDriven = MetersDriven;
            this.Price = Price;
            this.OptimalScore = OptimalScore;
            this.TripScore = TripScore;
        }

        public Trip(DataRow row) {
            this.TripId = row.Field<Int64>("tripid");
            this.CarId = row.Field<int>("carid");

            if (row.Table.Columns.Contains("previoustripid")) {
                row["previoustripid"] = row["previoustripid"] is DBNull ? 0 : row["previoustripid"];
                this.PreviousTripId = row.Field<Int64>("previoustripid");
            }
            //Temporal Information
            row["startdateid"] = row["startdateid"] is DBNull ? "19700101" : row["startdateid"];
            row["starttimeid"] = row["starttimeid"] is DBNull ? "0" : row["starttimeid"];
            this.StartTemporal = new TemporalInformation(DateTimeHelper.ConvertToDateTime(row.Field<int>("startdateid"), row.Field<int>("starttimeid")));
            row["enddateid"] = row["enddateid"] is DBNull ? "19700101" : row["enddateid"];
            row["endtimeid"] = row["endtimeid"] is DBNull ? "0" : row["endtimeid"];
            this.EndTemporal = new TemporalInformation(DateTimeHelper.ConvertToDateTime(row.Field<int>("enddateid"), row.Field<int>("endtimeid")));

            if (row.Table.Columns.Contains("secondsdriven")) {
                row["secondsdriven"] = row["secondsdriven"] is DBNull ? -1 : row["secondsdriven"];
                this.SecondsDriven = new TimeSpan(0, 0, row.Field<int>("secondsdriven"));
            }

            if (row.Table.Columns.Contains("secondstolag")) {
                row["secondstolag"] = row["secondstolag"] is DBNull ? -1 : row["secondstolag"];
                this.SecondsToLag = new TimeSpan(0, 0, row.Field<int>("secondstolag"));
            }

            //Spatial
            if (row.Table.Columns.Contains("metersdriven")) {
                row["metersdriven"] = row["metersdriven"] is DBNull ? -1 : row["metersdriven"];
                this.MetersDriven = (double)row.Field<Single>("metersdriven");
            }

            //Score Information
            if (row.Table.Columns.Contains("price")) {
                row["price"] = row["price"] is DBNull ? -1 : row["price"];
                this.Price = (double)row.Field<Single>("price");
            }

            if (row.Table.Columns.Contains("optimalscore")) {
                row["optimalscore"] = row["optimalscore"] is DBNull ? -1 : row["optimalscore"];
                this.OptimalScore = (double)row.Field<Single>("optimalscore");
            }

            if (row.Table.Columns.Contains("tripscore")) {
                row["tripscore"] = row["tripscore"] is DBNull ? -1 : row["tripscore"];
                this.TripScore = (double)row.Field<Single>("tripscore");
            }

            //Score Measures
            if (row.Table.Columns.Contains("jerkcount")) {
                row["jerkcount"] = row["jerkcount"] is DBNull ? -1 : row["jerkcount"];
                this.JerkCount = row.Field<Int16>("jerkcount");
            }

            if (row.Table.Columns.Contains("brakecount")) {
                row["brakecount"] = row["brakecount"] is DBNull ? -1 : row["brakecount"];
                this.BrakeCount = row.Field<Int16>("brakecount");
            }

            if (row.Table.Columns.Contains("accelerationcount")) {
                row["accelerationcount"] = row["accelerationcount"] is DBNull ? -1 : row["accelerationcount"];
                this.AccelerationCount = row.Field<Int16>("accelerationcount");
            }

            if (row.Table.Columns.Contains("meterssped")) {
                row["meterssped"] = row["meterssped"] is DBNull ? -1 : row["meterssped"];
                this.MetersSped = (double)row.Field<Single>("meterssped");
            }

            if (row.Table.Columns.Contains("timesped")) {
                row["timesped"] = row["timesped"] is DBNull ? 0 : row["timesped"];
                this.TimeSped = new TimeSpan(0, 0, (int)row.Field<Single>("timesped"));
            }

            if (row.Table.Columns.Contains("steadyspeeddistance")) {
                row["steadyspeeddistance"] = row["steadyspeeddistance"] is DBNull ? -1 : row["steadyspeeddistance"];
                this.SteadySpeedDistance = (double)row.Field<Single>("steadyspeeddistance");
            }

            if (row.Table.Columns.Contains("steadyspeedtime")) {
                row["steadyspeedtime"] = row["steadyspeedtime"] is DBNull ? 0 : row["steadyspeedtime"];
                this.SteadySpeedTime = new TimeSpan(0, 0, (int)row.Field<Single>("steadyspeedtime"));
            }

            //IntervalInformation
            if (row.Table.Columns.Contains("speedinterval")) {
                row["roadtypesinterval"] = row["roadtypesinterval"] is DBNull ? 0 : row["roadtypesinterval"];
                row["criticaltimeinterval"] = row["criticaltimeinterval"] is DBNull ? 0 : row["criticaltimeinterval"];
                row["speedinterval"] = row["speedinterval"] is DBNull ? 0 : row["speedinterval"];
                row["accelerationinterval"] = row["accelerationinterval"] is DBNull ? 0 : row["accelerationinterval"];
                row["jerkinterval"] = row["jerkinterval"] is DBNull ? 0 : row["jerkinterval"];
                row["brakinginterval"] = row["brakinginterval"] is DBNull ? 0 : row["brakinginterval"];
                this.IntervalInformation = new IntervalInformation(CarId,
                                                                   TripId,
                                                                   row.Field<Int64>("roadtypesinterval"),
                                                                   row.Field<Int64>("criticaltimeinterval"),
                                                                   row.Field<Int64>("speedinterval"),
                                                                   row.Field<Int64>("accelerationinterval"),
                                                                   row.Field<Int64>("jerkinterval"),
                                                                   row.Field<Int64>("brakinginterval"));

            }

            //Trip Score Information
            if (row.Table.Columns.Contains("roadtypescore")) {
                row["roadtypescore"] = row["roadtypescore"] is DBNull ? 0 : row["roadtypescore"];
                row["criticaltimescore"] = row["criticaltimescore"] is DBNull ? 0 : row["criticaltimescore"];
                row["speedingscore"] = row["speedingscore"] is DBNull ? 0 : row["speedingscore"];
                row["accelerationscore"] = row["accelerationscore"] is DBNull ? 0 : row["accelerationscore"];
                row["brakescore"] = row["brakescore"] is DBNull ? 0 : row["brakescore"];
                row["jerkscore"] = row["jerkscore"] is DBNull ? 0 : row["jerkscore"];
                this.RoadTypeScore = (double)row.Field<Single>("roadtypescore");
                this.CriticalTimeScore = (double)row.Field<Single>("criticaltimescore");
                this.SpeedingScore = (double)row.Field<Single>("speedingscore");
                this.AccelerationScore = (double)row.Field<Single>("accelerationscore");
                this.BrakeScore = (double)row.Field<Single>("brakescore");
                this.JerkScore = (double)row.Field<Single>("jerkscore");
            }

            //Data Quality
            if (row.Table.Columns.Contains("dataquality")) {
                row["dataquality"] = row["dataquality"] is DBNull ? -1 : row["dataquality"];
                this.DataQuality = (double)row.Field<Single>("dataquality");
            }
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(TripId);
            sb.Append(" ");
            sb.Append(CarId);

            return sb.ToString();

        }


    }
}
