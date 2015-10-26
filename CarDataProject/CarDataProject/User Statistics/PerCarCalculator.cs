using System;
using System.Collections.Generic;
using System.IO;

namespace CarDataProject {
    class PerCarCalculator {
        public static void SaveAllCardata(Int16 carid) {
            string solutionPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string dataPath = @"\data\";
            string foldername = "OldCar" + carid;

            string pathString = Path.Combine(solutionPath + dataPath, foldername);
            Directory.CreateDirectory(pathString);

            FileWriter.KilometersPerTrip(GetKPTPlot(carid), foldername);
            FileWriter.MinutesPerTrip(GetMPTPlot(carid), foldername);
            FileWriter.DefaultCarStatistics(carid, foldername);

            GnuplotHelper.PlotGraph("KilometersPerTripGraph", foldername, "KilometersPerTrip", true, 1, 2, "Kilometers per trip");
            GnuplotHelper.PlotGraph("MinutesPerTripGraph", foldername, "MinutesPerTrip", true, 1, 2, "Minutes per trip");
        }

        public static int GetTripsTaken(Int16 carId) {
            DBController dbc = new DBController();
            int tripsTaken = Convert.ToInt32(dbc.GetAmountOfTrips(carId));
            dbc.Close();
            return tripsTaken;
            
        }

        //Kilometers Per Trip
        public static List<double> GetKPTPlot(Int16 carid) {
            Int64 amountOfTrips = PerCarCalculator.GetTripsTaken(carid);
            List<double> kmprtrip = new List<double>();

            for (int i = 1; i < amountOfTrips; i++) {
                kmprtrip.Add(PerTripCalculator.GetKilometersDriven(carid, i));
            }

            return kmprtrip;
        }

        //Minutes Per Trip
        public static List<TimeSpan> GetMPTPlot(Int16 carid) {
            Int64 amountOfTrips = PerCarCalculator.GetTripsTaken(carid);
            List<TimeSpan> mprtrip = new List<TimeSpan>();

            for (int i = 1; i < amountOfTrips; i++) {
                mprtrip.Add(PerTripCalculator.GetTime(carid, i));
            }

            return mprtrip;
        }
    }
}
