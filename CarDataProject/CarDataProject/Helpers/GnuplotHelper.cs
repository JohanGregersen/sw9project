using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace CarDataProject {
    class GnuplotHelper {

        public static void PlotGraph(int target) {

            string Pgm = @"E:\University\Programs\gnuplot\bin\gnuplot.exe";
            // Caspers Path string Pgm = @"C:\Program Files (x86)\gnuplot\bin\gnuplot.exe";
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
                case 4:
                    gnupStWr.WriteLine("plot 'C:\\data\\weeklyPlots.dat' using 1:2 with boxes t \"Entries per weekday\"");
                    break;
                case 5:
                    gnupStWr.WriteLine("plot 'C:\\data\\weeklyTime.dat' using 1:2 with boxes t \"Time driven per weekday\"");
                    break;
                case 6:
                    gnupStWr.WriteLine("plot 'C:\\data\\hourlyTime.dat' using 1:2 with boxes t \"Time(in minutes) driven per hour\"");
                    break;
                case 7:
                    gnupStWr.WriteLine("plot 'C:\\data\\qualityHdopSat.dat' using 1:3 with lines t \"Sat\", 'C:\\data\\qualityHdopSat.dat' using 1:2 with lines t \"HDOP\"");
                    break;
                case 8:
                    gnupStWr.WriteLine("plot 'C:\\data\\distancePlots.dat' using 1:2 with lines t \"Distance\"");
                    break;
                case 9:
                    gnupStWr.WriteLine("plot 'C:\\data\\timePlots.dat' using 1:2 with lines t \"Time-difference\"");
                    break;
                default:
                    break;
            }

            gnupStWr.Flush();
        }
    }
}
