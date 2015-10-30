using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Skal slettes, men der er måske noget kode der skal genbruges?
 */

namespace CarDataProject {
    class OldCar {
        public OldCar(Int16 carid) {
            this.carId = carid;
        }

        public Int16 carId { get; set; }
        public List<Trip> allTrips { get; set; }

        public void GetQualityMeasurements() {

        }

        public void UpdateCarWithTripIds(Int16 carid) {
            DBController dbc = new DBController();

            List<Trip> myTrips = TripCalculator.CalculateTripsByCarId(1);
            for (int i = 1; i < myTrips.Count() + 1; i++) {
                foreach (Tuple<int, DateTime> entry in myTrips[i - 1].allTimestamps) {
                    dbc.UpdateWithNewId(i, entry.Item1);
                }
            }

            dbc.Close();
        }

        public void UpdateCarWithTripIdsOptimized(Int16 carid) {
            DBController dbc = new DBController();

            List<Trip> myTrips = TripCalculator.CalculateTripsByCarId(1);
            for (int i = 1; i < myTrips.Count() + 1; i++) {
                dbc.UpdateWithNewIdWithMultipleEntries(i, myTrips[i-1].allTimestamps);
            }

            dbc.Close();
        }
    }
}
