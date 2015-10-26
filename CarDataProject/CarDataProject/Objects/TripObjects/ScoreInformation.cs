using System;

namespace CarDataProject {
    class ScoreInformation {
        public int ScoreId { get; }
        public Int64 TripId { get; }
        public double TotalScore { get; }
        public double Jerks { get; }
        public double Acceleration{ get; }
        public double Speeding{ get; }
        public double TimeOfDay { get; }
        public double RoadType { get; }

        public ScoreInformation (int ScoreId, Int64 TripId, double TotalScore, double Jerks, double Acceleration, double Speeding, double TimeOfDay, double RoadType) {
            this.ScoreId = ScoreId;
            this.TripId = TripId;
            this.TotalScore = TotalScore;
            this.Jerks = Jerks;
            this.Acceleration = Acceleration;
            this.Speeding = Speeding;
            this.TimeOfDay = TimeOfDay;
            this.RoadType = RoadType;
        }
    }
}
