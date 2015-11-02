using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CarDataProject {
    class PerWeekCalculator {
        public static void SaveAllPerWeekData(Int16 carId) {
            string solutionPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string dataPath = @"\data\";
            string foldername = "Car" + carId;

            string pathString = Path.Combine(solutionPath + dataPath, foldername);
            Directory.CreateDirectory(pathString);

            //FileWriter.DefaultTripStatistics(carid, tripid, foldername);
            FileWriter.WeeklyKilometersPerTrip(carId, GetWeeklyKPTPlot(carId));
            GnuplotHelper.PlotGraph("WeeklyKilometerGraph", foldername, "weeklykm", true, 1, 3, "Kilometers Per Week");
        }

        public static Dictionary<int, double> GetWeeklyKPTPlot(Int16 carid) {
            DBController dbc = new DBController();
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            Dictionary<int, double> weeklykm = new Dictionary<int, double>();
            Dictionary<int, int> tripsofweek = new Dictionary<int, int>();
            Dictionary<int, double> finaldict = new Dictionary<int, double>();
            int weekno;
            Int64 amountOfTrips = CarStatistic.GetTripsTaken(carid);

            for (int i = 1; i < amountOfTrips; i++) {
                List<Timestamp> time = dbc.GetTimestampsByCarAndTripId(carid, i);
                weekno = cal.GetWeekOfYear(time[0].timestamp, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                if (!weeklykm.ContainsKey(weekno)) {
                    weeklykm.Add(weekno, TripStatistic.GetKilometersDriven(carid, i));
                    tripsofweek.Add(weekno, 1);
                } else {
                    weeklykm[weekno] = weeklykm[weekno] + TripStatistic.GetKilometersDriven(carid, i);
                    tripsofweek[weekno] = tripsofweek[weekno] + 1;
                }
            }
            dbc.Close();

            foreach (KeyValuePair<int, double> kvp in weeklykm) {
                finaldict.Add(kvp.Key, (kvp.Value / tripsofweek[kvp.Key]));
            }

            // FileWriter.WeeklyKilometersPerTrip(finaldict);
            // GnuplotHelper.PlotGraph(3, "WeeklyKMpertrip");

            return finaldict;
        }
    }
}
