using System;
using System.IO;

namespace CarDataProject {

    /*
    * Globally used constant variables
    */
    public static class Global {

        /*
        * Database related information
        */
        public static class Database {
            public const string Host = "localhost";
            public const string Name = "CarDB";
            public const string User = "casper";
            public const string Password = "1234";
        }

        /*
        * Filtype extensions
        */
        public static class FileType {
            public const string txt = ".txt";
            public const string dat = ".dat";
            public const string png = ".png";
            public const string exe = ".exe";
        }

        /*
        * Enumerations
        */
        public static class Enums {
            public enum Direction { forward = 0, FORWARD = 0, backward = 0, BACKWARD = 0, both = 1, BOTH = 1 }
            public enum RoadType {
                motorway = 1, trunk, primary, secondary, tertiary, motorway_link, primary_link, unclassified, road,
                residential, service, track, pedestrian, unpaved, living_street, trunk_link, secondary_link, ferry,
                tertiary_link, motorway_link_entry = 6, motorway_link_exit = 6, noinfo = 99
            };
        }

        /*
        * Directory paths relative from the project folder
        */
        public static class Path {
            public static string Solution = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            public static string Data = Solution + @"\Data\";
            public static string Batch = Data + @"\Batches\";
            public static string CarStatistics = Data + @"CarStatistics\";
            public static string GnuPlotExecutable = @"E:\University\Programs\gnuplot\bin\gnuplot.exe";
            //public static string GnuPlotExecutable = @"C:\Program Files (x86)\gnuplot\bin\gnuplot.exe";
        }

        /*
        * Folder and file names
        */
        public static class CarStatistics {
            private const string CarFolderPrefix = "Car";
            private const string TripFolderPrefix = "Trip";
            private const string DefaultFile = @"\Default" + FileType.dat;
            public static string CarPath(Int16 carId) {
                return Path.CarStatistics + CarFolderPrefix + carId;
            }
            public static string TripPath(Int16 carId, Int64 tripId) {
                return Path.CarStatistics + CarFolderPrefix + carId + TripFolderPrefix + tripId;
            }

            //.dat-files
            public static string DefaultCarStatisticFile(Int16 carId) {
                return CarPath(carId) + DefaultFile;
            }
            public static string DefaultTripStatisticFile(Int16 carId, Int64 tripId) {
                return TripPath(carId, tripId) + DefaultFile;
            }
            public static string WeeklyAverageTripDistanceFile(Int16 carId) {
                return CarPath(carId) + @"\WeeklyAverageTripDistance" + FileType.dat;
            }
            public static string KilometersPerTripFile(Int16 carId) {
                return CarPath(carId) + @"\KilometersPerTrip" + FileType.dat;
            }
            public static string MinutesPerTripFile(Int16 carId) {
                return CarPath(carId) + @"\MinutesPerTrip" + FileType.dat;
            }
            public static string PlotsPerWeekdayFile(Int16 carId) {
                return CarPath(carId) + @"\PlotsPerWeekday" + FileType.dat;
            }
            public static string TimePerWeekdayFile(Int16 carId) {
                return CarPath(carId) + @"\TimePerWeekday" + FileType.dat;
            }
            public static string TimePerHourPerWeekdayFile(Int16 carId) {
                return CarPath(carId) + @"\TimePerHourPerWeekday" + FileType.dat;
            }
            public static string DifferenceInTimeFile(Int16 carId) {
                return CarPath(carId) + @"\DifferenceInTime" + FileType.dat;
            }
            public static string DifferenceInDistanceFile(Int16 carId) {
                return CarPath(carId) + @"\DifferenceInDistance" + FileType.dat;
            }
            public static string SatHdopPerPointFile(Int16 carId) {
                return CarPath(carId) + @"\SatHdopPerPoint" + FileType.dat;
            }
            public static string OutliersFile(Int16 carId) {
                return CarPath(carId) + @"\Outliers" + FileType.dat;
            }
            public static string AccelerationFile(Int16 carId) {
                return CarPath(carId) + @"\Acceleration" + FileType.dat;
            }

            //.png-files
            public static string KilometersPerTripGraph(Int16 carId) {
                return CarPath(carId) + @"\KilometersPerTrip" + FileType.png;
            }
            public static string MinutesPerTripGraph(Int16 carId) {
                return CarPath(carId) + @"\MinutesPerTrip" + FileType.png;
            }
            public static string WeeklyAverageTripDistanceGraph(Int16 carId) {
                return CarPath(carId) + @"\WeeklyAverageTripDistance" + FileType.png;
            }
            public static string PlotsPerWeekdayGraph(Int16 carId) {
                return CarPath(carId) + @"\PlotsPerWeekday" + FileType.png;
            }
        }

        /*
        * INFATI specific variables
        */
        public static class Batch {
            public static class INFATI {
                public static string Path = Global.Path.Batch + @"INFATI\";
                public const char RowSeperator = '#';
                public static string LogFile(Int16 teamId, Int16 carId) {
                    return String.Format("team{0}_car{1:D2}_no_home{2}", teamId, carId, FileType.txt);
                }
            }
        }
    }
}
