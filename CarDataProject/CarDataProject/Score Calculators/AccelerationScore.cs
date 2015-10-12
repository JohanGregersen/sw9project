using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    class AccelerationScore {

        public AccelerationScore(Int16 carId, double accUpperThreshold, double accLowerThreshold) {
            this.carId = carId;
            this.numberOfTrips = PerCarCalculator.GetTripsTaken(this.carId);

            accelerationData = new Dictionary<int, List<double>>();
            GetAccelerationData();

            this.accUpperThreshold = accUpperThreshold;
            this.accLowerThreshold = accLowerThreshold;
            intAccData = FindValidDataPoints();

        }

        private void GetAccelerationData() {
            for (int i = 1; i <= numberOfTrips; i++) {
                accelerationData.Add(i, PerTripCalculator.GetAccelerationCalcultions(carId, i));
            }
        }
        
        public Dictionary<int, List<double>> FindValidDataPoints() {
            Dictionary<int, List<double>> ValidDataPoints = new Dictionary<int, List<double>>();
            foreach (KeyValuePair<int, List<double>> trip in accelerationData) {
                List<double> accPoints = trip.Value.Where(acc => acc >= accUpperThreshold || acc <= accLowerThreshold).ToList();
                ValidDataPoints.Add(trip.Key, accPoints);
            }
            return ValidDataPoints;
        }

        private Dictionary<int, List<double>> accelerationData { get; set; }
        private Dictionary<int, List<double>> intAccData { get; set; }
        private Int16 carId { get; set; }
        private int numberOfTrips { get; set; }
        private double accUpperThreshold { get; set; }
        private double accLowerThreshold { get; set; }
    }
}
