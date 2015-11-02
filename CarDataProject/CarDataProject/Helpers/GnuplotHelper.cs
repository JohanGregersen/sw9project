using System;
using System.Diagnostics;
using System.IO;

namespace CarDataProject {
    class GnuplotHelper {
        public static void Plot(string datFile, string pngFile, bool graphType, int xValue, int yValue, string subject) {
            //Start GnuPlot
            Process gnuPlot = new Process();
            gnuPlot.StartInfo.FileName = Global.Path.GnuPlotExecutable;
            gnuPlot.StartInfo.UseShellExecute = false;
            gnuPlot.StartInfo.RedirectStandardInput = true;
            gnuPlot.Start();

            StreamWriter writer = gnuPlot.StandardInput;

            writer.WriteLine("unset output");

            string typeString;
            if (graphType) {
                writer.WriteLine("set boxwidth 0.5");
                writer.WriteLine("set style fill solid");
                typeString = "boxes";
            } else {
                typeString = "lines";
            }

            writer.WriteLine(String.Format(@"plot '{'0'}' using {'1'}:{'2'} with {'3'} t ""{'4'}""", datFile, xValue, yValue, typeString, subject)); 
            writer.WriteLine(@"set term png size 3500,2000 enhanced font ""Arial, 20""");
            writer.WriteLine(@"set output '{'0'}'", pngFile);
            writer.WriteLine("replot");
            writer.WriteLine("set term win");

            writer.Flush();
        } 
    }
}
