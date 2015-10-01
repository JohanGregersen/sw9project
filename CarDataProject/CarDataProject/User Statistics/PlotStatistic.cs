using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace CarDataProject {
    class PlotStatistic {

        static string path = @"C:\data\";

        public static void GetWeeklyKPTPlot(int carid) {
            DBController dbc = new DBController();
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            Dictionary<int, double> weeklykm = new Dictionary<int, double>();
            Dictionary<int, int> tripsofweek = new Dictionary<int, int>();
            Dictionary<int, double> finaldict = new Dictionary<int, double>();
            int weekno;
            Int64 amountOfTrips = DefaultStatistic.TripsTaken(carid);

            for(int i = 1; i < amountOfTrips; i++) {
                List<Timestamp> time = dbc.GetTimestampsByCarAndTripId(carid, i);
                weekno = cal.GetWeekOfYear(time[0].timestamp, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                if (!weeklykm.ContainsKey(weekno)) {
                    weeklykm.Add(weekno, DefaultStatistic.KilometersDriven(carid, i));
                    tripsofweek.Add(weekno, 1);
                } else {
                    weeklykm[weekno] = weeklykm[weekno] + DefaultStatistic.KilometersDriven(carid, i);
                    tripsofweek[weekno] = tripsofweek[weekno] + 1;
                }
            }
            dbc.Close();

            foreach(KeyValuePair<int,double> kvp in weeklykm) {
                finaldict.Add(kvp.Key, (kvp.Value / tripsofweek[kvp.Key]));
            }

            WriteWeeklyKPTToFile(finaldict);
            Gnuplot(3);

        }

        public static void GetKPTPlot(int carid) {

            Int64 amountOfTrips = DefaultStatistic.TripsTaken(carid);
            List<double> kmprtrip = new List<double>();

            for(int i = 1; i < amountOfTrips; i++) {
                kmprtrip.Add(DefaultStatistic.KilometersDriven(carid, i));
            }

            WriteKPTToFile(kmprtrip);
            Gnuplot(1);

        }

        public static void GetMPTPlot(int carid) {

            Int64 amountOfTrips = DefaultStatistic.TripsTaken(carid);
            List<TimeSpan> mprtrip = new List<TimeSpan>();

            for (int i = 1; i < amountOfTrips; i++) {
                mprtrip.Add(DefaultStatistic.TimePerTrip(carid, i));
            }

            WriteMPTToFile(mprtrip);
            Gnuplot(2);

        }

        public static void WriteWeeklyKPTToFile(Dictionary<int, double> weeklykm) {
            using (StreamWriter writer = new StreamWriter(path + "weeklykm.dat")) {
                writer.WriteLine("#X, Week, Distance");
                foreach (KeyValuePair<int, double> kvp in weeklykm) {
                    writer.WriteLine(kvp.Key + " " + "Week" + kvp.Key + " " + weeklykm[kvp.Key]);
                }
            }
        }

        public static void WriteKPTToFile(List<double> kilometers) {
            using (StreamWriter writer = new StreamWriter(path + "kmprtrip.dat")) {
                writer.WriteLine("#X, title , Distance");
                for (int i = 0; i < kilometers.Count; i++) {
                    writer.WriteLine(i + " " + kilometers[i]);
                }
            }
        }

        public static void WriteMPTToFile(List<TimeSpan> timespan) {
            using (StreamWriter writer = new StreamWriter(path + "mprtrip.dat")) {
                writer.WriteLine("#X, title , Distance");
                for (int i = 0; i < timespan.Count; i++) {
                    writer.WriteLine(i + " " + timespan[i].Minutes);
                }
            }
        }

        public static void Gnuplot(int target) {

            string Pgm = @"E:\University\Programs\gnuplot\bin\gnuplot.exe";
            Process extPro = new Process();
            extPro.StartInfo.FileName = Pgm;
            extPro.StartInfo.UseShellExecute = false;
            extPro.StartInfo.RedirectStandardInput = true;
            extPro.Start();

            StreamWriter gnupStWr = extPro.StandardInput;
            gnupStWr.WriteLine("set boxwidth 0.5");
            gnupStWr.WriteLine("set style fill solid");

            switch (target) {
                case 1:
                    gnupStWr.WriteLine("plot 'C:\\data\\kmprtrip.dat' using 1:2 with boxes t \"Kilometers pr trip\"");
                    break;
                case 2:
                    gnupStWr.WriteLine("plot 'C:\\data\\mprtrip.dat' using 1:2 with boxes t \"Minutes pr trip\"");
                    break;
                case 3:
                    gnupStWr.WriteLine("plot 'C:\\data\\weeklykm.dat' using 1:3:xtic(2) with boxes t \"km/trip pr Week\"");
                    break;
                default:
                    break;
            }

            gnupStWr.Flush();


        }
    }
}
