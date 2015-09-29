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

            for(int i = 0; i < amountOfTrips - 1; i++) {
                kmprtrip.Add(DefaultStatistic.KilometersDriven(carid, i));
            }

            WriteKMprTripToFile(kmprtrip);
            Gnuplot(1);

        }

        public static void WriteKMprTripToFile(List<double> kilometers) {
            using (StreamWriter writer = new StreamWriter(path + "kmprtrip.dat")) {
                writer.WriteLine("#X, title , Distance");
                for (int i = 0; i < kilometers.Count; i++) {
                    writer.WriteLine(i + " " + "\"Trip " + i + "\"" +  " " + kilometers[i]);
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

            switch (target) {
                case 1:
                    gnupStWr.WriteLine("set boxwidth 0.5");
                    gnupStWr.WriteLine("set style fill solid");
                    gnupStWr.WriteLine("plot 'C:\\data\\kmprtrip.dat' using 1:3 with boxes t \"Kilometers / trip\"");
                    break;

                default:
                    break;
            }

            gnupStWr.Flush();


        }
    }
}
