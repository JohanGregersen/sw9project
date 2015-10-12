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

        public static void SaveAllTripData(Int16 carid, int tripid) {

            string solutionPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string dataPath = @"\data\";
            string foldername = "Car" + carid + "\\Trip" + tripid;

            string pathString = System.IO.Path.Combine(solutionPath + dataPath, foldername);
            System.IO.Directory.CreateDirectory(pathString);

            FileWriter.DefaultTripStatistics(carid, tripid, foldername);
        }

        public static TimeSpan GetTime (Int16 carid, int tripid) {

            DBController dbc = new DBController();
            List<Timestamp> timestamps = dbc.GetTimestampsByCarAndTripId(carid, tripid);


            TimeSpan triptime = timestamps[timestamps.Count - 1].timestamp - timestamps[0].timestamp;


            dbc.Close();

            return triptime;
        }

        public static double GetKilometersDriven(Int16 carid, int tripid) {

            double kmdriven = 0;

            List<Point> MPointData = ValidationPlots.GetMPointData(carid, tripid);

            for (int i = 0; i < MPointData.Count - 1; i++) {
                kmdriven += MPointData[i].Mpoint.GetDistanceTo(MPointData[i + 1].Mpoint);
            }

            kmdriven = kmdriven / 1000;
            return kmdriven;
        }

        public static List<Tuple<int, double>> GetAccelerationCalcultions(Int16 carId, int tripId) {
            DBController dbc = new DBController();
            List<Tuple<int, Timestamp, int>> accelerationData = dbc.GetAccelerationDataByTrip(carId, tripId);
            dbc.Close();

            List<Tuple<int, double>> accelerationCalculations = new List<Tuple<int, double>>();

            for(int i = 0; i < accelerationData.Count() - 1; i++) {
                //Item1 = TimeStamp (DateTime object)
                //Item2 = speed
                // a = VELCOCITY CHANGE / TIME TAKEN;
                double velocityChange = accelerationData[i + 1].Item3 - accelerationData[i].Item3;
                    
                double timeTaken = Convert.ToDouble((DateTimeHelper.ToUnixTime(accelerationData[i + 1].Item2.timestamp) - DateTimeHelper.ToUnixTime(accelerationData[i].Item2.timestamp)));

                double acceleration = velocityChange / timeTaken;

                accelerationCalculations.Add(new Tuple<int, double>(accelerationData[i].Item1, acceleration));

            }

            // FileWriter.Acceleration(accelerationCalculations);
            // GnuplotHelper.PlotGraph(10, "acceleration");

            return accelerationCalculations;
        }

    }
}
