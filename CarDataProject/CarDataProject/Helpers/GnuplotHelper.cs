using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace CarDataProject {
    class GnuplotHelper {

        /*
        public static void PlotGraph(int target, string graphname) {
            string solutionPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string dataPath = @"\data\";

            // string Pgm = @"E:\University\Programs\gnuplot\bin\gnuplot.exe";
            string Pgm = @"C:\Program Files (x86)\gnuplot\bin\gnuplot.exe";
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
                    gnupStWr.WriteLine("plot '" + solutionPath + dataPath + "kmprtrip.dat' using 1:2 with boxes t \"Kilometers pr trip\"");
                    break;
                case 2:
                    gnupStWr.WriteLine("plot '" + solutionPath + dataPath + "mprtrip.dat' using 1:2 with boxes t \"Minutes pr trip\"");
                    break;
                case 3:
                    gnupStWr.WriteLine("plot '" + solutionPath + dataPath + "weeklykm.dat' using 1:3:xtic(2) with boxes t \"km/trip pr Week\"");
                    break;
                case 4:
                    gnupStWr.WriteLine("plot '" + solutionPath + dataPath + "weeklyPlots.dat' using 1:2 with boxes t \"Entries per weekday\"");
                    break;
                case 5:
                    gnupStWr.WriteLine("plot '" + solutionPath + dataPath + "weeklyTime.dat' using 1:2 with boxes t \"Time driven per weekday\"");
                    break;
                case 6:
                    gnupStWr.WriteLine("plot '" + solutionPath + dataPath + "hourlyTime.dat' using 1:2 with boxes t \"Time(in minutes) driven per hour\"");
                    break;
                case 7:
                    gnupStWr.WriteLine("plot '" + solutionPath + dataPath + "qualityHdopSat.dat' using 1:3 with lines t \"Sat\", '" + solutionPath + dataPath + "qualityHdopSat.dat' using 1:2 with lines t \"HDOP\"");
                    break;
                case 8:
                    gnupStWr.WriteLine("plot '" + solutionPath + dataPath + "distancePlots.dat' using 1:2 with lines t \"Distance\"");
                    break;
                case 9:
                    gnupStWr.WriteLine("plot '" + solutionPath + dataPath + "timePlots.dat' using 1:2 with lines t \"Time-difference\"");
                    break;
                case 10:
                    gnupStWr.WriteLine("plot '" + solutionPath + dataPath + "acceleration.dat' using 1:2 with lines t \"Acceleration\"");
                    break;
                default:
                    break;
            }

            gnupStWr.WriteLine("set term png size 3500,2000 enhanced font \"Arial, 20\"");
            gnupStWr.WriteLine("set output '" + solutionPath + dataPath + graphname + ".png'");
            gnupStWr.WriteLine("replot");
            gnupStWr.WriteLine("set term win");

            gnupStWr.Flush();
        }
        */


        public static void PlotGraph(string desiredname, string foldername, string filename, bool graphtype, int xvalue, int yvalue, string subject) {

            string solutionPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string dataPath = @"\data\";
            string typestring;

            string Pgm = @"E:\University\Programs\gnuplot\bin\gnuplot.exe";
            //string Pgm = @"C:\Program Files (x86)\gnuplot\bin\gnuplot.exe";
            Process extPro = new Process();
            extPro.StartInfo.FileName = Pgm;
            extPro.StartInfo.UseShellExecute = false;
            extPro.StartInfo.RedirectStandardInput = true;
            extPro.Start();

            StreamWriter gnupStWr = extPro.StandardInput;

            gnupStWr.WriteLine("unset output");

            if (graphtype) {
                gnupStWr.WriteLine("set boxwidth 0.5");
                gnupStWr.WriteLine("set style fill solid");
                typestring = "boxes";
            } else {
                typestring = "lines";
            }

            gnupStWr.WriteLine("plot '" + solutionPath + dataPath + foldername + "\\" + filename + ".dat' using " + xvalue + ":" + yvalue + " with " + typestring + " t \"" + subject + "\"");
            gnupStWr.WriteLine("set term png size 3500,2000 enhanced font \"Arial, 20\"");
            gnupStWr.WriteLine("set output '" + solutionPath + dataPath + foldername + "\\" + desiredname + ".png'");
            gnupStWr.WriteLine("replot");
            gnupStWr.WriteLine("set term win");

            gnupStWr.Flush();
        } 

    }
}
