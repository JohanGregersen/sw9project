using System.Collections.Generic;
using System.IO;

namespace CarDataProject {
    class Program {
        static string CarLogFile = "t01c01";

        static void Main(string[] args) {


            int breakpoint = 0;
        }

        private static void InsertCarLogIntoDB() {
            List<CarLogEntry> entries = FileHandler.ReadCarLog(CarLogFile);

            DBController dbc = new DBController();

            foreach (CarLogEntry entry in entries) {
                dbc.AddCarLogEntry(entry);
            }

            dbc.Close();
        }
    }
}
