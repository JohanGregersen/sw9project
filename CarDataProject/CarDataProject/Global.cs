using System;
using System.IO;

namespace CarDataProject {
    static class Global {
        public static class FileType {
            public const string txt = @".txt";
        }

        public static class Path {
            public static string SolutionPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        }

        public static class Batch {
            public static class INFATI {
                public const string Path = @"\Data\Batches\INFATI";
                public const char RowSeperator = '#';
                public static string LogFile(int team, int car) {
                    return String.Format("team'{0}'_car'{1:D2}'_no_home", team, car);
                }
            }
        }
    }
}
