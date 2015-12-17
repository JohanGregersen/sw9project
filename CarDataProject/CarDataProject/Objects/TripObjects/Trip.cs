using System;
using System.Collections.Generic;
using System.Data;

namespace CarDataProject {
    public class Trip {
        public Int64 TripId { get; }
        public Int64 PreviousTripId { get; set; }
        public int CarId { get; }
        public TemporalInformation StartTemporal { get; set; }
        public TemporalInformation EndTemporal { get; set; }
        public TimeSpan SecondsDriven { get; set; }
        public double MetersDriven { get; set; }
        public double Price { get; set; }
        public double OptimalScore { get; set; }
        public double TripScore { get; set; }
        public Int16 JerkCount { get; set; }
        public Int16 BrakeCount { get; set; }
        public Int16 AccelerationCount { get; set; }
        public double MetersSped { get; set; }
        public TimeSpan TimeSped { get; set; }
        public double SteadySpeedDistance { get; set; }
        public TimeSpan SteadySpeedTime { get; set; }
        public TimeSpan SecondsToLag { get; set; }
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

            row["previoustripid"] = row["previoustripid"] is DBNull ? 0 : row["previoustripid"];
            this.PreviousTripId = row.Field<Int64>("previoustripid");

            //Temporal Information
            row["startdate"] = row["startdate"] is DBNull ? "19700101" : row["startdate"];
            row["starttime"] = row["starttime"] is DBNull ? "0" : row["starttime"];
            this.StartTemporal = new TemporalInformation(DateTimeHelper.ConvertToDateTime(row.Field<int>("startdate"), row.Field<int>("starttime")));
            row["enddate"] = row["enddate"] is DBNull ? "19700101" : row["enddate"];
            row["endtime"] = row["endtime"] is DBNull ? "0" : row["endtime"];
            this.EndTemporal = new TemporalInformation(DateTimeHelper.ConvertToDateTime(row.Field<int>("enddate"), row.Field<int>("endtime")));

            row["secondsdriven"] = row["secondsdriven"] is DBNull ? -1 : row["secondsdriven"];
            this.SecondsDriven = new TimeSpan(0, 0, row.Field<int>("secondsdriven"));

            row["secondstolag"] = row["secondstolag"] is DBNull ? -1 : row["secondstolag"];
            this.SecondsDriven = new TimeSpan(0, 0, row.Field<int>("secondstolag"));

            //Spatial
            row["metersdriven"] = row["metersdriven"] is DBNull ? -1 : row["metersdriven"];
            this.MetersDriven = (double)row.Field<Single>("metersdriven");

            //Score Information
            row["price"] = row["price"] is DBNull ? -1 : row["price"];
            this.Price = (double)row.Field<Single>("price");

            row["optimalscore"] = row["optimalscore"] is DBNull ? -1 : row["optimalscore"];
            this.OptimalScore= (double)row.Field<Single>("optimalscore");

            row["tripscore"] = row["tripscore"] is DBNull ? -1 : row["tripscore"];
            this.TripScore = (double)row.Field<Single>("tripscore");

            //Score Measures
            row["jerkcount"] = row["jerkcount"] is DBNull ? -1 : row["jerkcount"];
            this.JerkCount = row.Field<Int16>("jerkcount"); 

            row["brakecount"] = row["brakecount"] is DBNull ? -1 : row["brakecount"];
            this.BrakeCount = row.Field<Int16>("brakecount");

            row["accelerationcount"] = row["accelerationcount"] is DBNull ? -1 : row["accelerationcount"];
            this.AccelerationCount = row.Field<Int16>("accelerationcount");

            row["meterssped"] = row["meterssped"] is DBNull ? -1 : row["meterssped"];
            this.MetersSped = (double)row.Field<Single>("meterssped");

            row["timesped"] = row["timesped"] is DBNull ? 0 : row["timesped"];
            this.TimeSped = new TimeSpan(0, 0, (int)row.Field<Single>("timesped"));

            row["steadyspeeddistance"] = row["steadyspeeddistance"] is DBNull ? -1 : row["steadyspeeddistance"];
            this.TripScore = (double)row.Field<Single>("steadyspeeddistance");

            row["steadyspeedtime"] = row["steadyspeedtime"] is DBNull ? 0 : row["steadyspeedtime"];
            this.TimeSped = new TimeSpan(0, 0, (int)row.Field<Single>("steadyspeedtime"));

            //IntervalInformation
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

            //Data Quality
            row["dataquality"] = row["dataquality"] is DBNull ? -1 : row["dataquality"];
            this.TripScore = (double)row.Field<Single>("dataquality");

        }
    }
}
