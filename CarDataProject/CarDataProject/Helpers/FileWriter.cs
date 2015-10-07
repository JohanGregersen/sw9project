﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    static class FileWriter {

        static string solutionPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        static string dataPath = @"\data\";
        static string filetype = ".dat";

        public static void WeeklyKilometersPerTrip(Dictionary<int, double> weeklykm) {
            using (StreamWriter writer = new StreamWriter(solutionPath + dataPath + "weeklykm" + filetype)) {
                writer.WriteLine("#X, Week, Distance");
                foreach (KeyValuePair<int, double> kvp in weeklykm) {
                    writer.WriteLine(kvp.Key + " " + "Week" + kvp.Key + " " + weeklykm[kvp.Key]);
                }
            }
        }

        public static void KilometersPerTrip(List<double> kilometers) {
            using (StreamWriter writer = new StreamWriter(solutionPath + dataPath + "kmprtrip" + filetype)) {
                writer.WriteLine("#X, title , Distance");
                for (int i = 0; i < kilometers.Count; i++) {
                    writer.WriteLine(i + " " + kilometers[i]);
                }
            }
        }

        public static void MinutesPerTrip(List<TimeSpan> timespan) {
            using (StreamWriter writer = new StreamWriter(solutionPath + dataPath + "mprtrip" + filetype )) {
                writer.WriteLine("#X, title , Distance");
                for (int i = 0; i < timespan.Count; i++) {
                    writer.WriteLine(i + " " + timespan[i].Minutes);
                }
            }
        }

        public static void PlotsPerWeekday(Dictionary<DayOfWeek, int> plotsPerWeekday) {
            string path = @"C:\data\";
            using (StreamWriter writer = new StreamWriter(solutionPath + dataPath + "weeklyPlots" + filetype)) {
                writer.WriteLine("#DayOfWeek, Entries");
                foreach (KeyValuePair<DayOfWeek, int> kvp in plotsPerWeekday) {
                    writer.WriteLine(((int)kvp.Key + 1) + " " + plotsPerWeekday[kvp.Key]);
                }
            }
        }

        public static void TimePerWeekday(Dictionary<DayOfWeek, TimeSpan> timePerWeekday) {
            string path = @"C:\data\";
            using (StreamWriter writer = new StreamWriter(solutionPath + dataPath + "weeklytime" + filetype)) {
                writer.WriteLine("#DayOfWeek, Time");
                foreach (KeyValuePair<DayOfWeek, TimeSpan> kvp in timePerWeekday) {
                    writer.WriteLine(((int)kvp.Key + 1) + " " + timePerWeekday[kvp.Key].TotalMinutes);
                }
            }
        }

        public static void TimePerHourPerWeekday(Dictionary<DayOfWeek, Dictionary<int, TimeSpan>> timePerHourPerWeekday) {
            string path = @"C:\data\";
            using (StreamWriter writer = new StreamWriter(solutionPath + dataPath + "hourlytime" + filetype)) {
                writer.WriteLine("#Hour, Time");

                foreach (KeyValuePair<int, TimeSpan> kvp in timePerHourPerWeekday[DayOfWeek.Monday]) {
                    writer.WriteLine(kvp.Key + " " + kvp.Value.TotalMinutes);
                }
            }
        }

        public static void DifferenceInTime(List<double> timePlots) {
            using (StreamWriter writer = new StreamWriter(solutionPath + dataPath + "timePlots" + filetype)) {
                writer.WriteLine("#X, Time-difference");
                for (int i = 0; i < timePlots.Count; i++) {
                    writer.WriteLine(i + " " + timePlots[i]);
                }
            }
        }

        public static void DifferenceInDistance(List<double> distancePlots) {
            using (StreamWriter writer = new StreamWriter(solutionPath + dataPath + "distanceplots" + filetype)) {
                writer.WriteLine("#X, Distance");
                for (int i = 0; i < distancePlots.Count; i++) {
                    writer.WriteLine(i + " " + distancePlots[i]);
                }
            }
        }

        public static void HdopAndSatPerPoint(List<SatHdop> qualityPlots) {
            using (StreamWriter writer = new StreamWriter(solutionPath + dataPath + "hdopsat" + filetype)) {
                writer.WriteLine("#X, Hdop, Sat");
                for (int i = 0; i < qualityPlots.Count; i++) {
                    writer.WriteLine(i + " " + qualityPlots[i].Hdop + " " + qualityPlots[i].Sat);
                }
            }
        }

        public static void DifferenceOutliers(List<Tuple<GeoCoordinate, GeoCoordinate>> failPoints) {
            using (StreamWriter writer = new StreamWriter(solutionPath + dataPath + "failpoints" + filetype)) {
                writer.WriteLine("#point1.lat, point1.lng, point2.lat, point2.lng");
                for (int i = 0; i < failPoints.Count; i++) {
                    writer.WriteLine(failPoints[i].Item1.Latitude + "," + failPoints[i].Item1.Longitude + " " + failPoints[i].Item2.Latitude + "," + failPoints[i].Item2.Longitude);
                }
            }
        }

        public static void Acceleration(List<double> acceleration) {
            using (StreamWriter writer = new StreamWriter(path + "acceleration.dat")) {
                writer.WriteLine("#X, acceleration");
                for (int i = 0; i < acceleration.Count; i++) {
                    writer.WriteLine(i + " " + acceleration[i]);
                }
            }
        }


    }
}
