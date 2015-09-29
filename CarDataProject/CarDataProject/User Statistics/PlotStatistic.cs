using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace CarDataProject {
    class PlotStatistic {

        static string path = @"C:\data\";

        public static void GetKMprTripPlot(int carid) {

            Int64 amountOfTrips = DefaultStatistic.TripsTaken(carid);
            List<double> kmprtrip = new List<double>();

            for(int i = 1; i < amountOfTrips; i++) {
                kmprtrip.Add(DefaultStatistic.KilometersDriven(carid, i));
            }

            WriteKMprTripToFile(kmprtrip);
            Gnuplot(1);

        }

        public static void GetMinutesprTripPlot(int carid) {

            Int64 amountOfTrips = DefaultStatistic.TripsTaken(carid);
            List<TimeSpan> mprtrip = new List<TimeSpan>();

            for (int i = 1; i < amountOfTrips; i++) {
                mprtrip.Add(DefaultStatistic.TimePerTrip(carid, i));
            }

            WriteTimeprTripToFile(mprtrip);
            Gnuplot(2);

        }

        public static void WriteTimeprTripToFile(List<TimeSpan> timespan) {
            using (StreamWriter writer = new StreamWriter(path + "mprtrip.dat")) {
                writer.WriteLine("#X, title , Distance");
                for (int i = 0; i < timespan.Count; i++) {
                    writer.WriteLine(i + " " + timespan[i].Minutes);
                }
            }
        }

        public static void WriteKMprTripToFile(List<double> kilometers) {
            using (StreamWriter writer = new StreamWriter(path + "kmprtrip.dat")) {
                writer.WriteLine("#X, title , Distance");
                for (int i = 0; i < kilometers.Count; i++) {
                    writer.WriteLine(i + " " + kilometers[i]);
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
                    gnupStWr.WriteLine("plot 'C:\\data\\kmprtrip.dat' using 1:2 with boxes t \"Kilometers / trip\"");
                    break;
                case 2:
                    gnupStWr.WriteLine("plot 'C:\\data\\mprtrip.dat' using 1:2 with boxes t \"Minutes / trip\"");
                    break;
                default:
                    break;
            }

            gnupStWr.Flush();


        }
    }
}
