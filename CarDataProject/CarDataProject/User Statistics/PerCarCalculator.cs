using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    class PerCarCalculator {

        public static int GetTripsTaken(Int16 carId) {

            DBController dbc = new DBController();
            int tripsTaken = Convert.ToInt32(dbc.GetAmountOfTrips(carId));
            dbc.Close();
            return tripsTaken;
            
        }

        //Kilometers Per Trip
        public static void GetKPTPlot(Int16 carid) {

            Int64 amountOfTrips = PerCarCalculator.GetTripsTaken(carid);
            List<double> kmprtrip = new List<double>();

            for (int i = 1; i < amountOfTrips; i++) {
                kmprtrip.Add(PerTripCalculator.GetKilometersDriven(carid, i));
            }

            FileWriter.KilometersPerTrip(kmprtrip);
            GnuplotHelper.PlotGraph(1);

        }

        //Minutes Per Trip
        public static void GetMPTPlot(Int16 carid) {

            Int64 amountOfTrips = PerCarCalculator.GetTripsTaken(carid);
            List<TimeSpan> mprtrip = new List<TimeSpan>();

            for (int i = 1; i < amountOfTrips; i++) {
                mprtrip.Add(PerTripCalculator.GetTime(carid, i));
            }

            FileWriter.MinutesPerTrip(mprtrip);
            GnuplotHelper.PlotGraph(2);

        }

    }
}
