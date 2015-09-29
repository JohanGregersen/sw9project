﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    class Car {
        public Car(Int64 carid) {
            this.carId = carid;
        }

        public Int64 carId { get; set; }
        public List<Trip> allTrips { get; set; }




        public void GetQualityMeasurements() {

        }

        public void UpdateCarWithTripIds(Int64 carid) {
            DBController dbc = new DBController();

            List<Trip> myTrips = TripCalculator.CalculateTripsByCarId(1);
            for (int i = 1; i < myTrips.Count() + 1; i++) {
                foreach (Tuple<Int64, DateTime> entry in myTrips[i - 1].allTimestamps) {
                    dbc.UpdateWithNewId(i, entry.Item1);
                }
            }

            dbc.Close();
        }

        public void UpdateCarWithTripIdsOptimized(Int64 carid) {
            DBController dbc = new DBController();

            List<Trip> myTrips = TripCalculator.CalculateTripsByCarId(1);
            for (int i = 1; i < myTrips.Count() + 1; i++) {
                dbc.UpdateWithNewIdWithMultipleEntries(i, myTrips[i-1].allTimestamps);
            }



            dbc.Close();
        }


    }
}