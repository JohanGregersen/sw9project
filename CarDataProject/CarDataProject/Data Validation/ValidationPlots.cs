using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Device.Location;
using System.Diagnostics;

namespace CarDataProject {
    class ValidationPlots {

        static string path = @"\data\";

        public static void GetAllPlots(int carid, int tripid) {
            GetMpointPlot(carid, tripid);
            GetTimePlot(carid, tripid);
            GetHdopSatPlot(carid, tripid);
        }

        public static List<Timestamp> GetTimeData(int carid, int tripid) {
            DBController dbc = new DBController();
            List<Timestamp> data = dbc.GetTimestampsByCarAndTripId(carid, tripid);
            dbc.Close();
            return data;
        }

        public static void GetTimePlot(int carid, int tripid) {
            List<Timestamp> timeData = GetTimeData(carid, tripid);

            foreach (Timestamp ts in timeData) {
                ts.StoreAsDateTimeFormat();
            }

            List<double> timestampDifferences = new List<double>();
            for (int i = 0; i < timeData.Count - 1; i++) {
                double timeDifference = Math.Abs(DateTimeHelper.ToUnixTime(timeData[i].timestamp) - DateTimeHelper.ToUnixTime(timeData[i + 1].timestamp));
                timestampDifferences.Add(timeDifference);
            }

            WriteTimeDifferenceToFile(timestampDifferences);
            Gnuplot(3);
        }

        public static List<Point> GetMPointData(int carid, int tripid) {
            DBController dbc = new DBController();
            List<Point> data = dbc.GetMPointByCarAndTripId(carid, tripid);
            dbc.Close();
            return data;
        }

        public static void GetMpointPlot(int carid, int tripid) {
            List<Point> MpointData = GetMPointData(carid, tripid);

            List<double> distanceMeasures = new List<double>();
            List<Tuple<GeoCoordinate, GeoCoordinate>> outofscopePoints = new List<Tuple<GeoCoordinate, GeoCoordinate>>();


            for (int i = 0; i < MpointData.Count - 1; i++) {
                distanceMeasures.Add(MpointData[i].Mpoint.GetDistanceTo(MpointData[i + 1].Mpoint));
                if (MpointData[i].Mpoint.GetDistanceTo(MpointData[i + 1].Mpoint) > 250) {
                    outofscopePoints.Add(new Tuple<GeoCoordinate, GeoCoordinate>(MpointData[i].Mpoint, MpointData[i + 1].Mpoint));
                }
            }
            WriteFailPointsToFile(outofscopePoints);
            WritePointDistancesToFile(distanceMeasures);

            Gnuplot(2);
        }


        public static List<SatHdop> GetSatHdopData(int carid, int tripid) {
            DBController dbc = new DBController();
            List<SatHdop> data = dbc.GetSatHdopForTrip(carid, tripid);
            dbc.Close();
            return data;
        }

        public static List<SatHdop> GetLowQualityPlots(int carid, int tripid) {
            List<SatHdop> SatHdopData = GetSatHdopData(carid, tripid);

            List<SatHdop> lowQualityEntries = new List<SatHdop>();
            foreach (SatHdop qual in SatHdopData) {
                if (qual.Sat <= 3 || qual.Hdop >= 3) {
                    lowQualityEntries.Add(qual);
                }

            }

            return lowQualityEntries;
        }

        public static void GetHdopSatPlot(int carid, int tripid) {
            List<SatHdop> SatHdopData = GetSatHdopData(carid, tripid);

            List<Int64> Hdop = new List<Int64>();
            List<Int64> Sat = new List<Int64>();

            for (int i = 0; i < SatHdopData.Count; i++) {
                Hdop.Add(SatHdopData[i].Hdop);
                Sat.Add(SatHdopData[i].Sat);
            }

            WriteQualityHdopSatToFile(SatHdopData);
            Gnuplot(1);
        }

        public static void Gnuplot(int target) {

            string Pgm = @"C:\Program Files (x86)\gnuplot\bin\gnuplot.exe";
            Process extPro = new Process();
            extPro.StartInfo.FileName = Pgm;
            extPro.StartInfo.UseShellExecute = false;
            extPro.StartInfo.RedirectStandardInput = true;
            extPro.Start();

            StreamWriter gnupStWr = extPro.StandardInput;

            switch (target) {
                case 1:
                    gnupStWr.WriteLine("plot 'C:\\data\\qualityHdopSat.dat' using 1:3 with lines t \"Sat\", 'C:\\data\\qualityHdopSat.dat' using 1:2 with lines t \"HDOP\"");
                    break;
                case 2:
                    gnupStWr.WriteLine("plot 'C:\\data\\distancePlots.dat' using 1:2 with lines t \"Distance\"");
                    break;
                case 3:
                    gnupStWr.WriteLine("plot 'C:\\data\\timePlots.dat' using 1:2 with lines t \"Time-difference\"");
                    break;
                default:
                    break;
            }

            gnupStWr.Flush();


        }

        public static void WriteTimeDifferenceToFile(List<double> timePlots) {
            using (StreamWriter writer = new StreamWriter(path + "timePlots.dat")) {
                writer.WriteLine("#X, Time-difference");
                for (int i = 0; i < timePlots.Count; i++) {
                    writer.WriteLine(i + " " + timePlots[i]);
                }
            }
        }

        public static void WritePointDistancesToFile(List<double> distancePlots) {
            using (StreamWriter writer = new StreamWriter(path + "distancePlots.dat")) {
                writer.WriteLine("#X, Distance");
                for (int i = 0; i < distancePlots.Count; i++) {
                    writer.WriteLine(i + " " + distancePlots[i]);
                }
            }
        }

        public static void WriteQualityHdopSatToFile(List<SatHdop> qualityPlots) {
            using (StreamWriter writer = new StreamWriter(path + "qualityHdopSat.dat")) {
                writer.WriteLine("#X, Hdop, Sat");
                for (int i = 0; i < qualityPlots.Count; i++) {
                    writer.WriteLine(i + " " + qualityPlots[i].Hdop + " " + qualityPlots[i].Sat);
                }
            }
        }

        public static void WriteFailPointsToFile(List<Tuple<GeoCoordinate, GeoCoordinate>> failPoints) {
            using (StreamWriter writer = new StreamWriter(path + "failPoints.dat")) {
                writer.WriteLine("#point1.lat, point1.lng, point2.lat, point2.lng");
                for (int i = 0; i < failPoints.Count; i++) {
                    writer.WriteLine(failPoints[i].Item1.Latitude + "," + failPoints[i].Item1.Longitude + " " + failPoints[i].Item2.Latitude + "," + failPoints[i].Item2.Longitude);
                }
            }
        }

    }
}
