using System;
using System.Collections.Generic;
using System.IO;
using System.Device.Location;

namespace CarDataProject {
    static class FileWriter {
        public static void DefaultCarStatistics(Int16 carId) {
            using (StreamWriter writer = new StreamWriter(Global.CarStatistics.DefaultCarStatisticFile(carId))) {
                writer.WriteLine("Amount Of Trips Taken : " + CarStatistics.TripCount(carId));
            }
        }

        public static void DefaultTripStatistics(Int16 carId, Int64 tripId) {
            using (StreamWriter writer = new StreamWriter(Global.CarStatistics.DefaultTripStatisticFile(carId, tripId))) {
                writer.WriteLine("Amount Of Minutes Driven : " + TripStatistics.Duration(carId, tripId));
                writer.WriteLine("Amount Of Kilmoters Driven : " + TripStatistics.Distance(carId, tripId));
            }
        }

        public static void WeeklyKilometersPerTrip(Int16 carId, Dictionary<int, double> kilometersPerWeek) {
            using (StreamWriter writer = new StreamWriter(Global.CarStatistics.WeeklyKilometersFile(carId))) {
                writer.WriteLine("#X, Week, Distance");

                foreach (KeyValuePair<int, double> kvp in kilometersPerWeek) {
                    writer.WriteLine(kvp.Key + " " + "Week" + kvp.Key + " " + kilometersPerWeek[kvp.Key]);
                }
            }
        }

        public static void KilometersPerTrip(Int16 carId, List<double> kilometers) {
            using (StreamWriter writer = new StreamWriter(Global.CarStatistics.KilometersPerTripFile(carId))) {
                writer.WriteLine("#X, title , Distance");

                for (int i = 0; i < kilometers.Count; i++) {
                    writer.WriteLine(i + " " + kilometers[i]);
                }
            }
        }

        public static void MinutesPerTrip(Int16 carId, List<TimeSpan> tripTime) {
            using (StreamWriter writer = new StreamWriter(Global.CarStatistics.MinutesPerTripFile(carId))) {
                writer.WriteLine("#X, title , Distance");

                for (int i = 0; i < tripTime.Count; i++) {
                    writer.WriteLine(i + " " + tripTime[i].Minutes);
                }
            }
        }

        public static void PlotsPerWeekday(Int16 carId, Dictionary<DayOfWeek, int> plotsPerWeekday) {
            using (StreamWriter writer = new StreamWriter(Global.CarStatistics.PlotsPerWeekdayFile(carId))) {
                writer.WriteLine("#DayOfWeek, Entries");

                foreach (KeyValuePair<DayOfWeek, int> kvp in plotsPerWeekday) {
                    writer.WriteLine(((int)kvp.Key + 1) + " " + plotsPerWeekday[kvp.Key]);
                }
            }
        }

        public static void TimePerWeekday(Int16 carId, Dictionary<DayOfWeek, TimeSpan> timePerWeekday) {
            using (StreamWriter writer = new StreamWriter(Global.CarStatistics.TimePerWeekdayFile(carId))) {
                writer.WriteLine("#DayOfWeek, Time");

                foreach (KeyValuePair<DayOfWeek, TimeSpan> kvp in timePerWeekday) {
                    writer.WriteLine(((int)kvp.Key + 1) + " " + timePerWeekday[kvp.Key].TotalMinutes);
                }
            }
        }

        //TODO: Printer den her ikke kun mandag ud?
        public static void TimePerHourPerWeekday(Int16 carId, Dictionary<DayOfWeek, Dictionary<int, TimeSpan>> timePerHourPerWeekday) {
            using (StreamWriter writer = new StreamWriter(Global.CarStatistics.TimePerHourPerWeekdayFile(carId))) {
                writer.WriteLine("#Hour, Time");

                foreach (KeyValuePair<int, TimeSpan> kvp in timePerHourPerWeekday[DayOfWeek.Monday]) {
                    writer.WriteLine(kvp.Key + " " + kvp.Value.TotalMinutes);
                }
            }
        }

        public static void DifferenceInTime(Int16 carId, List<double> timePlots) {
            using (StreamWriter writer = new StreamWriter(Global.CarStatistics.DifferenceInTimeFile(carId))) {
                writer.WriteLine("#X, Time-difference");

                for (int i = 0; i < timePlots.Count; i++) {
                    writer.WriteLine(i + " " + timePlots[i]);
                }
            }
        }

        public static void DifferenceInDistance(Int16 carId, List<double> distancePlots) {
            using (StreamWriter writer = new StreamWriter(Global.CarStatistics.DifferenceInDistanceFile(carId))) {
                writer.WriteLine("#X, Distance");

                for (int i = 0; i < distancePlots.Count; i++) {
                    writer.WriteLine(i + " " + distancePlots[i]);
                }
            }
        }

        public static void SatHdopPerPoint(Int16 carId, List<QualityInformation> qualities) {
            using (StreamWriter writer = new StreamWriter(Global.CarStatistics.SatHdopPerPointFile(carId))) {
                writer.WriteLine("#X, Hdop, Sat");

                for (int i = 0; i < qualities.Count; i++) {
                    writer.WriteLine(i + " " + qualities[i].Hdop + " " + qualities[i].Sat);
                }
            }
        }

        public static void Outliers(Int16 carId, Dictionary<GeoCoordinate, GeoCoordinate> outliers) {
            using (StreamWriter writer = new StreamWriter(Global.CarStatistics.OutliersFile(carId))) {
                writer.WriteLine("#point1.lat, point1.lng, point2.lat, point2.lng");

                foreach(KeyValuePair<GeoCoordinate, GeoCoordinate> outlier in outliers) {
                    writer.WriteLine(outlier.Key.Latitude + "," + outlier.Key.Longitude + " " + outlier.Value.Latitude + "," + outlier.Value.Longitude);
                }
            }
        }

        public static void Acceleration(Int16 carId, List<double> acceleration) {
            using (StreamWriter writer = new StreamWriter(Global.CarStatistics.AccelerationFile(carId))) {
                writer.WriteLine("#X, acceleration");

                for (int i = 0; i < acceleration.Count; i++) {
                    writer.WriteLine(i + " " + acceleration[i]);
                }
            }
        }
    }
}
