using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    class DefaultStatistic {

        public static double Price() {

            // Some evaluation on previous statstics
       
            double finalprice;
            return finalprice = 0.0;
        }

        // Method for calculating the amount of kilometers driven on a certain trip
        public static double KilometersDriven(int carid, int tripid) {

            double kmdriven = 0;
            
            List<Point> MPointData = ValidationPlots.GetMPointData(carid, tripid);

            for (int i = 0; i < MPointData.Count - 1; i++) {
                kmdriven += MPointData[i].Mpoint.GetDistanceTo(MPointData[i + 1].Mpoint);
            }

            kmdriven = kmdriven / 1000;
            return kmdriven;
        }
        
        // Method for fetching the amount of trips taken for a certain car
        public static Int64 TripsTaken(int carid) {

            DBController dbc = new DBController();
            return dbc.GetAmountOfTrips(carid);

        }


        public static TimeSpan TimePerTrip(int carid, int tripid) {

            DBController dbc = new DBController();
            List<Timestamp> timestamps = dbc.GetTimestampsByCarAndTripId(carid, tripid);

            TimeSpan triptime = timestamps[timestamps.Count - 1].timestamp - timestamps[0].timestamp;
            dbc.Close();

            return triptime;
        }

        public static int OptPercentileDriven() {

            // some evaluation of the optimal route and how the driver scored

            int optpercentile;
            return optpercentile = 0;
        }

        public static int DataSecurityPercentile() {

            // some evaluation of our standards on data security

            int datasecurity;
            return datasecurity = 0;
        }

    }
}
