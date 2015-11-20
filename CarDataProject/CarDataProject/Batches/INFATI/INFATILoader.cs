using System;
using System.Collections.Generic;
using System.Linq;

using NpgsqlTypes;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    public static class INFATILoader {

        public static void LoadCarData(int numberOfCarsToLoad) {
            DBController dbc = new DBController();
            for (Int16 i = 1; i <= numberOfCarsToLoad; i++) {
                INFATI.AdjustLog(1, i);
                INFATILoader.DBLoader(INFATI.ReadLog(1, i));
                List<INFATITrip> trips = TripCalculator.CalculateTripsByCarId(i);


                foreach (INFATITrip trip in trips) {
                    dbc.InsertTripAndUpdateFactTable(trip);
                }
            }
            dbc.Close();
        }

        private static void DBLoader(List<INFATIEntry> entries) {
            DBController dbc = new DBController();

            //CarInformation

            int carId = dbc.AddCarInformation();





            foreach (INFATIEntry entry in entries) {
                entry.CarId = carId;
                //TemportalInformation
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
