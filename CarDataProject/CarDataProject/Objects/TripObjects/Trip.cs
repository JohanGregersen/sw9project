using System;

namespace CarDataProject {
    public class Trip {
        public Int64 TripId { get; }
        public int CarId { get; }
        public ScoreInformation Score { get; }
        public TimeSpan SecondsDriven { get; }
        public double MetersDriven { get; }
        public double Price { get; }
        public double BaselineScore { get; }
        public double OptimalityScore { get; }

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
