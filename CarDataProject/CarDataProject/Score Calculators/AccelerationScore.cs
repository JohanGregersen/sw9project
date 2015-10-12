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

            accelerationData = new Dictionary<int, List<Tuple<int, double>>>();
            GetAccelerationData();

            this.accUpperThreshold = accUpperThreshold;
            this.accLowerThreshold = accLowerThreshold;
            dataOutsideThreshold = FindValidDataPoints();
                
        }

        /*
        public double CalculateScore(double weight) {

        }
        */

        

        //Get all AccelerationCalculations for each tripId and store them
        //newtripid, List<Tuple<entryid, acceleration>>
        private void GetAccelerationData() {
            for (int i = 1; i <= numberOfTrips; i++) {
                accelerationData.Add(i, PerTripCalculator.GetAccelerationCalcultions(carId, i));
            }
        }

        //Finds all newtripid as key, Tuple<entryId, accelerations as value which exceeds the threshold values.
        public Dictionary<int, List<Tuple<int, double>>> FindValidDataPoints() {
            Dictionary<int, List<Tuple<int, double>>> ValidDataPoints = new Dictionary<int, List<Tuple<int, double>>>();
            foreach (KeyValuePair<int, List<Tuple<int, double>>> trip in accelerationData) {
                    List<Tuple<int, double>> accPoints = trip.Value.Where(acc => acc.Item2 >= accUpperThreshold || acc.Item2 <= accLowerThreshold).ToList();
                    ValidDataPoints.Add(trip.Key, accPoints);
            }
            return ValidDataPoints;
        }

        //Dictionary as newtripid and Tuple<entryId, acceleration>
        private Dictionary<int, List<Tuple<int, double>>> accelerationData { get; set; }
        private Dictionary<int, List<Tuple<int, double>>> dataOutsideThreshold { get; set; }
        private Int16 carId { get; set; }
        private int numberOfTrips { get; set; }
        private double accUpperThreshold { get; set; }
        private double accLowerThreshold { get; set; }
    }
}
