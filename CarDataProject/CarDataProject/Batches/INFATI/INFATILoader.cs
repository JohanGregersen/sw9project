using System;
using System.Collections.Generic;

namespace CarDataProject {
    public static class INFATILoader {
        public static void LoadCarData(Int16 teamId, Int16 carId) {
            
            INFATI.AdjustLog(teamId, carId);
            INFATILoader.DBLoader(carId, INFATI.ReadLog(teamId, carId));
            
            DBController dbc = new DBController();
            List<TemporalInformation> datapoints = dbc.GetTimestampsByCarId(carId);
            dbc.Close();
            Console.WriteLine("Found " + datapoints.Count + " gps-entries for car " + carId);
            List<INFATITrip> trips = TripCalculator.CalculateTripsByCarId(carId);
            Console.WriteLine("Found " + trips.Count + " trips for car " + carId);

            dbc = new DBController();
            foreach (INFATITrip trip in trips) {
                dbc.InsertTripAndUpdateFactTable(trip);
            }

            dbc.Close();


            

        }

        private static void DBLoader(Int16 carId, List<INFATIEntry> entries) {
            DBController dbc = new DBController();

            //CarInformation
            dbc.AddCarInformation(carId);

            foreach (INFATIEntry entry in entries) {
                entry.CarId = carId;
                //TemporalInformation
                //Handled in DBController

                //QualityInformation
                entry.QualityId = dbc.GetQualityInformationIdBySatHdop(entry.Sat, entry.Hdop);

                //SegmentInformation
                dbc.AddINFATIEntry(entry);
            }

            dbc.Close();
        }
    }
}
