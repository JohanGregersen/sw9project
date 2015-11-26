using System;

namespace CarDataProject {
    public class Trip {
        public Int64 TripId { get; }
        public Int64 PreviousTripId { get; }
        public int CarId { get; }
        public TemporalInformation StartTemporal { get; }
        public TemporalInformation EndTemporal { get; }
        public TimeSpan SecondsDriven { get; }
        public double MetersDriven { get; }
        public double Price { get; }
        public double BaselineScore { get; }
        public double OptimalityScore { get; }
        public Int16 JerkCount { get; }
        public Int16 BrakeCount { get; }
        public Int16 AccelerationCount { get; }
        public double MetersSped { get; }
        public double TimeSped { get; }
        public double SteadySpeedDistance { get; }
        public double SteadySpeedTime { get; }
        public TimeSpan SecondsToLag { get; }
        public double DataQuality { get; }

        /*

              tripid bigserial NOT NULL,
              previoustripid bigint,
              carid integer,
              startdate integer,
              enddate integer,
              starttime integer,
              endtime integer,
              secondsdriven integer,
              metersdriven real,
              price real,
              optimalscore real,
              tripscore real,
              jerkcount smallint,
              brakecount smallint,
              accelerationcount smallint,
              meterssped real,
              timesped real,
              steadyspeeddistance real,
              steadyspeedtime real,
              secondstolag integer,
              dataquality real,

        */
        public Trip (Int64 TripId, int CarId, TimeSpan SecondsDriven, double MetersDriven, double Price, double BaselineScore, double OptimalityScore) {
            this.TripId = TripId;
            this.CarId = CarId;
            this.SecondsDriven = SecondsDriven;
            this.MetersDriven = MetersDriven;
            this.Price = Price;
            this.BaselineScore = BaselineScore;
            this.OptimalityScore = OptimalityScore;
        }
    }
}
