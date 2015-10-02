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

            List<double> timestampDifferences = new List<double>();
            for (int i = 0; i < timeData.Count - 1; i++) {
                double timeDifference = Math.Abs(DateTimeHelper.ToUnixTime(timeData[i].timestamp) - DateTimeHelper.ToUnixTime(timeData[i + 1].timestamp));
                timestampDifferences.Add(timeDifference);
            }

            FileWriter.DifferenceInTime(timestampDifferences);
            GnuplotHelper.PlotGraph(9);
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
            FileWriter.DifferenceOutliers(outofscopePoints);
            FileWriter.DifferenceInDistance(distanceMeasures);

            GnuplotHelper.PlotGraph(8);
        }

        public static List<SatHdop> GetSatHdopData(int carid, int tripid) {
            DBController dbc = new DBController();
            List<SatHdop> data = dbc.GetSatHdopForTrip(carid, tripid);
            dbc.Close();
            return data;
        }

        public static void GetHdopSatPlot(int carid, int tripid) {
            List<SatHdop> SatHdopData = GetSatHdopData(carid, tripid);

            List<Int64> Hdop = new List<Int64>();
            List<Int64> Sat = new List<Int64>();

            for (int i = 0; i < SatHdopData.Count; i++) {
                Hdop.Add(SatHdopData[i].Hdop);
                Sat.Add(SatHdopData[i].Sat);
            }

            FileWriter.HdopAndSatPerPoint(SatHdopData);
            GnuplotHelper.PlotGraph(7);
        }

    }
}
