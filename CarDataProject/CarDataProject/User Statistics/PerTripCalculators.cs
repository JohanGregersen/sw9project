using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace CarDataProject {
    class PerTripCalculator {

        static string path = @"C:\data\";

        public static TimeSpan GetTime (int carid, int tripid) {

            DBController dbc = new DBController();
            List<Timestamp> timestamps = dbc.GetTimestampsByCarAndTripId(carid, tripid);

            TimeSpan triptime = timestamps[timestamps.Count - 1].timestamp - timestamps[0].timestamp;
            dbc.Close();

            return triptime;
        }

        public static double GetKilometersDriven(int carid, int tripid) {

            double kmdriven = 0;

            List<Point> MPointData = ValidationPlots.GetMPointData(carid, tripid);

            for (int i = 0; i < MPointData.Count - 1; i++) {
                kmdriven += MPointData[i].Mpoint.GetDistanceTo(MPointData[i + 1].Mpoint);
            }

            kmdriven = kmdriven / 1000;
            return kmdriven;
        }

        public static void GetKPTPlot(int carid) {

            Int64 amountOfTrips = PerCarCalculator.GetTripsTaken(carid);
            List<double> kmprtrip = new List<double>();

            for (int i = 1; i < amountOfTrips; i++) {
                kmprtrip.Add(GetKilometersDriven(carid, i));
            }

            FileWriter.KilometersPerTrip(kmprtrip);
            GnuplotHelper.PlotGraph(1);

        }

        public static void GetMPTPlot(int carid) {

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
