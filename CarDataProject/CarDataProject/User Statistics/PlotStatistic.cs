using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CarDataProject.User_Statistics {
    class PlotStatistic {

        static string path = @"\data\";

        public static void WriteKMprTripToFile(List<double> kilometers) {
            using (StreamWriter writer = new StreamWriter(path + "kmprtrip.dat")) {
                writer.WriteLine("#X, Distance");
                for (int i = 0; i < kilometers.Count; i++) {
                    writer.WriteLine(i + " " + kilometers[i]);
                }
            }
        }
    }
}
