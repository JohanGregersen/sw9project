using System;
using System.Collections.Generic;
using System.Linq;

namespace CarDataProject {
    class AccelerationScore {
        List<List<Fact>> accelerationData { get; set; }
        private List<List<Fact>> dataOutsideThreshold { get; set; }
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
                accelerationData.Add(TripStatistics.Accelerations(carId, tripId));
            }

            dataOutsideThreshold = FindValidDataPoints();   
        }

        //Finds all newtripid as key, Tuple<entryId, accelerations as value which exceeds the threshold values.
        public List<List<Fact>> FindValidDataPoints() {
            List<List<Fact>> ValidDataPoints = new List<List<Fact>>();
            foreach (List<Fact> trip in accelerationData) {
                    List<Fact> accPoints = trip.Where(acc => acc.Measure.Acceleration >= accUpperThreshold || acc.Measure.Acceleration <= accLowerThreshold).ToList();
                    ValidDataPoints.Add(accPoints);
            }
            return ValidDataPoints;
        }
    }
}
