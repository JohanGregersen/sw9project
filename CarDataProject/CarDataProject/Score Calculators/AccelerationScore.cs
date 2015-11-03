using System;
using System.Collections.Generic;
using System.Linq;

namespace CarDataProject {
    class AccelerationScore {
        List<List<Fact>> accelerationData { get; set; }
        private Dictionary<int, List<Tuple<int, double>>> dataOutsideThreshold { get; set; }
        private Int16 carId { get; set; }
        private List<Int64> tripIds { get; set; }
        private double accUpperThreshold { get; set; }
        private double accLowerThreshold { get; set; }

        public AccelerationScore(Int16 carId, double accUpperThreshold, double accLowerThreshold) {
            this.carId = carId;
            this.accUpperThreshold = accUpperThreshold;
            this.accLowerThreshold = accLowerThreshold;

            DBController dbc = new DBController();
            this.tripIds = dbc.GetTripIdsByCarId(carId);
            dbc.Close();

            accelerationData = new List<List<Fact>>();

            foreach (Int64 tripId in tripIds) {
                accelerationData.Add(TripStatistics.Acceleration(carId, tripId));
            }

            dataOutsideThreshold = FindValidDataPoints();   
        }

        //Finds all newtripid as key, Tuple<entryId, accelerations as value which exceeds the threshold values.
        public Dictionary<int, List<Tuple<int, double>>> FindValidDataPoints() {
            Dictionary<int, List<Tuple<int, double>>> ValidDataPoints = new Dictionary<int, List<Tuple<int, double>>>();
            foreach (List<Fact> trip in accelerationData) {
                    List<Tuple<int, double>> accPoints = trip.Value.Where(acc => acc.Item2 >= accUpperThreshold || acc.Item2 <= accLowerThreshold).ToList();
                    ValidDataPoints.Add(trip.Key, accPoints);
            }

            return ValidDataPoints;
        }
    }
}
